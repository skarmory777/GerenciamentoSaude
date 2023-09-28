// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalancoHidricoAppService.cs" company="">
//   
// </copyright>
// <summary>
//   The balanco hidrico app service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.SqlClient;
using Abp.Domain.Uow;
using Abp.UI;
using Castle.Core.Internal;
using Dapper;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.Helper;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.EntityFramework.Repositories;
    using Abp.Extensions;
    using ClassesAplicacao.AtendimentosLeitosMov;
    using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.BalancoHidrico;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
    using Dto;
    using Atendimentos;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto.BalancoHidricos;
    using System.Web;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;

    /// <inheritdoc />
    public class BalancoHidricoAppService : SWMANAGERAppServiceBase, IBalancoHidricoAppService
    {
        /// <inheritdoc />
        public async Task<BalancoHidricoDto> ObterAsync(long atendId, DateTime dataBalanco)
        {
            var dto = await ObterBalancoHidrico(atendId, dataBalanco).ConfigureAwait(false);
            if (dto == null)
            {
                return dto;
            }
            MapConferencia(dto);
            dto.EnableDesconferir = await CheckAcaoDesconferir(dto);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atendId"></param>
        /// <param name="balancoHidricoId"></param>
        /// <returns></returns>
        public async Task<BalancoHidricoDto> ObterBalancoHidricoAnterior(long atendId, long? balancoHidricoId)
        {
            var id = await ObterIdBalancoHidricoAnterior(atendId, balancoHidricoId).ConfigureAwait(false);

            return id == 0 ? null : await ObterIdAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atendId"></param>
        /// <param name="balancoHidricoId"></param>
        /// <returns></returns>
        public async Task<long> ObterIdBalancoHidricoAnterior(long atendId, long? balancoHidricoId)
        {
            using (var connection = new SqlConnection(GetConnection()))
            {
                var queryWhere = "WHERE BH.AtendimentoId = @atendId  AND BH.IsDeleted = @isDeleted";
                const string queryBh = @" SELECT TOP 1 BH.Id FROM AteBalancoHidricos (NOLOCK) BH ";
                var parameters = new
                {
                    atendId,
                    balancoHidricoId = balancoHidricoId ?? 0,
                    isDeleted = false
                };

                if (balancoHidricoId.HasValue)
                {
                    queryWhere += " AND BH.Id < @balancoHidricoId";
                }
                var query = queryBh + " " + queryWhere + " ORDER BY bh.Id DESC";
                return await connection.QueryFirstOrDefaultAsync<long>(query, parameters).ConfigureAwait(false);
            }
        }

        public async Task<long> ObterIdBalancoHidricoAnterior(long atendId, DateTime dateBalancoHidrico)
        {
            dateBalancoHidrico = dateBalancoHidrico.Date.AddDays(-1);
            using (var balancoHidricoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<BalancoHidrico, long>>())
            {
                var iqResult = balancoHidricoRepository.Object.GetAll().AsNoTracking();
                return (await iqResult.OrderByDescending(x => x.Id).FirstOrDefaultAsync(c =>
                    c.AtendimentoId == atendId && DbFunctions.TruncateTime(c.DataBalancoHidrico) <= DbFunctions.TruncateTime(dateBalancoHidrico)))?.Id ?? 0;
            }
        }

        /// <summary>
        /// The obter historico async.
        /// </summary>
        /// <param name="atendId">
        /// The atend id.
        /// </param>
        /// <param name="dataBalanco">
        /// The data balanco.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<IEnumerable<BalancoHidricoDto>> ObterHistoricoAsync(long atendId, DateTime dataBalanco)
        {
            using (var connection = new SqlConnection(GetConnection()))
            {
                const string queryBh = @" SELECT BH.Id FROM AteBalancoHidricos (NOLOCK) BH WHERE BH.AtendimentoId = @atendId AND CAST(bh.DataBalancoHidrico AS DATE) < CAST(@dataBalanco AS DATE) AND BH.IsDeleted = @isDeleted;";
                var bhIds = await connection.QueryAsync<long>(queryBh, new { atendId, dataBalanco, isDeleted = false }).ConfigureAwait(false);

                if (!bhIds.Any())
                {
                    return null;
                }

                return await ObterPorIdsAsync(bhIds).ConfigureAwait(false);
            }

        }

        /// <inheritdoc />
        public BalancoHidricoDto GerarNovoBalancoHidrico(long atendId, DateTime dataBalanco, int horaIntervalo, int numSolucoes)
        {
            var model = new BalancoHidrico
            {
                AtendimentoId = atendId,
                DataBalancoHidrico = dataBalanco,
                BalancoHidricoItems = new List<BalancoHidricoItem>(),
                BalancoHidricoSolucoes = new List<BalancoHidricoSolucoes>()
            };

            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
            {
                var atendimento = atendimentoAppService.Object.Obter(atendId).Result;
                var atendimentoLeitoMovs = atendimentoLeitoMovRepository.Object.GetAll()
                    .Include(x => x.Leito)
                    .Include(x => x.Leito.TipoAcomodacao)
                    .Include(x => x.Leito.TipoAcomodacao)
                    .Where(x => x.AtendimentoId == atendId && x.DataInicial <= dataBalanco)
                    .OrderByDescending(x => x.DataInicial);

                var dataAcomodacao = atendimento.DataRegistro;
                var tipoAcomodacao = "Local de Atendimento";
                var leitoTipoAcomodacaoId = -1D;
                foreach (var leitoMov in atendimentoLeitoMovs)
                {
                    if (leitoTipoAcomodacaoId == -1)
                    {
                        leitoTipoAcomodacaoId = leitoMov.Leito.TipoAcomodacao.Id;
                    }

                    if (leitoMov.Leito.TipoAcomodacao.Id == leitoTipoAcomodacaoId)
                    {
                        dataAcomodacao = leitoMov.DataInicial.Value;
                        tipoAcomodacao = leitoMov.Leito.TipoAcomodacao.Descricao;
                    }
                    else
                    {
                        break;
                    }
                }

                model.DiasNaAcomodacao = (dataBalanco.Date - dataAcomodacao.Date).TotalDays + 1;
                model.TipoAcomodacao = tipoAcomodacao;

                for (var index = 1; index <= numSolucoes; index++)
                {
                    model.BalancoHidricoSolucoes.Add(new BalancoHidricoSolucoes { IndiceSolucao = index });
                }

                // int totalHoras =0;

                // if (horaIntervalo == 6)
                // {
                // totalHoras = 30;
                // }

                // if (horaIntervalo == 7)
                // {
                // totalHoras = 31;
                // }
                for (var index = horaIntervalo; index < 24 + horaIntervalo; index++)
                {
                    var hora = index + 1;

                    if (hora >= 24)
                    {
                        hora -= 24;
                    }

                    model.BalancoHidricoItems.Add(CriarItem(hora, model.BalancoHidricoSolucoes));

                    if (hora % horaIntervalo != 0 || hora == horaIntervalo)
                    {
                        continue;
                    }

                    model.BalancoHidricoItems.Add(CriarItem(hora, model.BalancoHidricoSolucoes, totalTransporte: true));
                    model.BalancoHidricoItems.Add(CriarItem(hora, model.BalancoHidricoSolucoes, totalParcial: true));
                }

                if (model.BalancoHidricoItems.Any())
                {
                    model.BalancoHidricoItems.Add(CriarItem(horaIntervalo, model.BalancoHidricoSolucoes, totalTransporte: true));
                    model.BalancoHidricoItems.Add(CriarItem(horaIntervalo, model.BalancoHidricoSolucoes, totalParcial: true));

                }

                model.BalancoHidricoItems.Add(CriarItem(horaIntervalo, model.BalancoHidricoSolucoes, totalGeral: true));

                model.HoraIntervalo = horaIntervalo;

                return BalancoHidricoDto.Mapear(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BalancoHidricoDto> ObterIdAsync(long id)
        {
            var queryBh = @$"
                SELECT  {QueryHelper.CreateQueryFields<BalancoHidrico>().TableAlias("BH").GetFields()}
                FROM AteBalancoHidricos (NOLOCK) BH WHERE BH.Id = @balancoHidricoId AND BH.IsDeleted = @isDeleted;";
            var queryBhSolucoes = @$"SELECT
                {QueryHelper.CreateQueryFields<BalancoHidricoSolucoes>().TableAlias("BHItensSolucoes").GetFields()} 
                FROM AteBalancoHidricoSolucoes (NOLOCK) AS BHItensSolucoes WHERE BHItensSolucoes.BalancoHidricoId = @bhId AND BHItensSolucoes.IsDeleted = @isDeleted;";
            var queryBhItens = @$"
            SELECT 
                   {QueryHelper.CreateQueryFields<BalancoHidricoItem>().TableAlias("BHItens").GetFields()},
                   {QueryHelper.CreateQueryFields<BalancoHidricoEndovenoso>().TableAlias("BHItensEndovenosos").GetFields()},
                   {QueryHelper.CreateQueryFields<BalancoHidricoSinaisVitais>().TableAlias("BHItensSinaisVitais").GetFields()}
            FROM AteBalancoHidricoItens (NOLOCK) AS BHItens
            LEFT JOIN AteBalancoHidricoEndovenosos (NOLOCK) AS BHItensEndovenosos ON BHItensEndovenosos.BalancoHidricoItemId = BHItens.Id AND BHItensEndovenosos.IsDeleted = @isDeleted
            LEFT JOIN AteBalancoHidricoSinaisVitais (NOLOCK) AS BHItensSinaisVitais ON BHItensSinaisVitais.Id = BHItens.SinaisVitaisId AND BHItensSinaisVitais.IsDeleted = @isDeleted
            WHERE BHItens.BalancoHidricoId = @bhId AND BHItens.IsDeleted = @isDeleted;";

            using (var connection = new SqlConnection(GetConnection()))
            {
                var bh = await connection.QueryFirstOrDefaultAsync<BalancoHidricoDto>(queryBh, new { balancoHidricoId = id, isDeleted = false }).ConfigureAwait(false);

                if (bh == null || bh.Id == 0)
                {
                    return bh;
                }

                bh.BalancoHidricoSolucoes = (ICollection<BalancoHidricoSolucoesDto>)await connection.QueryAsync<BalancoHidricoSolucoesDto>(queryBhSolucoes, new { bhId = bh.Id, isDeleted = false }).ConfigureAwait(false);

                bh.BalancoHidricoItems =
                    ((ICollection<BalancoHidricoItemDto>)await connection
                        .QueryAsync<BalancoHidricoItemDto, BalancoHidricoEndovenosoDto, BalancoHidricoSinaisVitaisDto, BalancoHidricoItemDto>(queryBhItens, MapBalancoItens, new { bhId = bh.Id, isDeleted = false })
                        .ConfigureAwait(false)).GroupBy(x => x.Id).Select(x =>
                    {
                        var bhItem = x.First();
                        bhItem.Endovenosos = x.SelectMany(z => z.Endovenosos).ToList();
                        return bhItem;
                    }).ToList();

                return bh;
            }
        }


        private async Task<IEnumerable<BalancoHidricoDto>> ObterPorIdsAsync(IEnumerable<long> bhIds)
        {
            if (bhIds.IsNullOrEmpty())
            {
                return null;
            }

            var queryBh = @$"
                SELECT  {QueryHelper.CreateQueryFields<BalancoHidrico>().TableAlias("BH").GetFields()}
                FROM AteBalancoHidricos (NOLOCK) BH WHERE BH.Id = @balancoHidricoId AND BH.IsDeleted = @isDeleted;";
            var queryBhSolucoes = @$" SELECT
                {QueryHelper.CreateQueryFields<BalancoHidricoSolucoes>().TableAlias("BHItensSolucoes").GetFields()} 
                FROM AteBalancoHidricoSolucoes (NOLOCK) AS BHItensSolucoes WHERE BHItensSolucoes.BalancoHidricoId = @bhId AND BHItensSolucoes.IsDeleted = @isDeleted;";
            var queryBhItens = @$"
            SELECT 
                   {QueryHelper.CreateQueryFields<BalancoHidricoItem>().TableAlias("BHItens").GetFields()},
                   {QueryHelper.CreateQueryFields<BalancoHidricoEndovenoso>().TableAlias("BHItensEndovenosos").GetFields()},
                   {QueryHelper.CreateQueryFields<BalancoHidricoSinaisVitais>().TableAlias("BHItensSinaisVitais").GetFields()}
            FROM AteBalancoHidricoItens (NOLOCK) AS BHItens
            LEFT JOIN AteBalancoHidricoEndovenosos (NOLOCK) AS BHItensEndovenosos ON BHItensEndovenosos.BalancoHidricoItemId = BHItens.Id AND BHItensEndovenosos.IsDeleted = @isDeleted
            LEFT JOIN AteBalancoHidricoSinaisVitais (NOLOCK) AS BHItensSinaisVitais ON BHItensSinaisVitais.Id = BHItens.SinaisVitaisId AND BHItensSinaisVitais.IsDeleted = @isDeleted
            WHERE BHItens.BalancoHidricoId = @bhId AND BHItens.IsDeleted = @isDeleted;";

            using (var connection = new SqlConnection(GetConnection()))
            {
                try
                {
                    var bhs = await connection
                        .QueryAsync<BalancoHidricoDto>(queryBh, new { bhIds = bhIds, isDeleted = false })
                        .ConfigureAwait(false);

                    if (bhs.IsNullOrEmpty())
                    {
                        return null;
                    }

                    var solucoes =
                        (await connection
                            .QueryAsync<BalancoHidricoSolucoesDto>(queryBhSolucoes,
                                new { bhIds = bhIds, isDeleted = false }).ConfigureAwait(false))
                        .GroupBy(x => x.BalancoHidricoId);

                    var balancoHidricoItems =
                        ((ICollection<BalancoHidricoItemDto>)await connection
                            .QueryAsync<BalancoHidricoItemDto, BalancoHidricoEndovenosoDto,
                                BalancoHidricoSinaisVitaisDto,
                                BalancoHidricoItemDto>(queryBhItens, MapBalancoItens,
                                new { bhIds = bhIds, isDeleted = false })
                            .ConfigureAwait(false)).GroupBy(x => x.BalancoHidricoId);

                    foreach (var bh in bhs)
                    {
                        bh.BalancoHidricoSolucoes = (ICollection<BalancoHidricoSolucoesDto>)solucoes.FirstOrDefault(x => x.Key == bh.Id);
                        bh.BalancoHidricoItems = balancoHidricoItems.FirstOrDefault(x => x.Key == bh.Id).ToList()
                            .GroupBy(x => x.Id).Select(x =>
                         {
                             var bhItem = x.FirstOrDefault();
                             if (bhItem == null)
                             {
                                 return null;
                             }
                             bhItem.Endovenosos = x.SelectMany(z => z.Endovenosos).ToList();
                             return bhItem;
                         }).ToList();
                    }

                    return bhs;
                }
                catch (Exception e)
                {

                }

                return null;
            }
        }


        private async Task<BalancoHidricoDto> ObterBalancoHidrico(long atendId, DateTime dataBalanco)
        {
            using (var connection = new SqlConnection(GetConnection()))
            {
                const string queryBh = @" SELECT BH.Id FROM AteBalancoHidricos (NOLOCK) BH WHERE BH.AtendimentoId = @atendId AND CAST(bh.DataBalancoHidrico AS DATE) = CAST(@dataBalanco AS DATE) AND BH.IsDeleted = @isDeleted ORDER BY BH.Id DESC;";
                var bh = await connection.QueryFirstOrDefaultAsync<long>(queryBh, new { atendId, dataBalanco, isDeleted = false }).ConfigureAwait(false);

                if (bh == 0)
                {
                    return null;
                }

                return await ObterIdAsync(bh).ConfigureAwait(false);
            }
        }

        private static BalancoHidricoItemDto MapBalancoItens(BalancoHidricoItemDto bhItem, BalancoHidricoEndovenosoDto bhEndovenoso, BalancoHidricoSinaisVitaisDto bhSinaisVitais)
        {
            if (bhItem == null)
            {
                return null;
            }

            if (bhItem.Endovenosos == null)
            {
                bhItem.Endovenosos = new List<BalancoHidricoEndovenosoDto>();
            }

            if (bhEndovenoso != null)
            {
                bhItem.Endovenosos.Add(bhEndovenoso);
            }

            bhItem.SinaisVitais = bhSinaisVitais;

            return bhItem;
        }

        private static BalancoHidricoItem CriarItem(int hora, IEnumerable<BalancoHidricoSolucoes> solucoes, bool totalGeral = false, bool totalParcial = false, bool totalTransporte = false)
        {
            var balancoHidricoItem = new BalancoHidricoItem
            {
                Hora = new TimeSpan(hora, 0, 0),
                SinaisVitais = new BalancoHidricoSinaisVitais(),
                TotalGeral = totalGeral,
                TotalParcial = totalParcial,
                TotalTransporte = totalTransporte,
                Endovenosos = new List<BalancoHidricoEndovenoso>()
            };
            foreach (var balancoHidricoSolucao in solucoes)
            {
                balancoHidricoItem.Endovenosos.Add(new BalancoHidricoEndovenoso { IndiceSolucao = balancoHidricoSolucao.IndiceSolucao });
            }

            return balancoHidricoItem;
        }

        /// <inheritdoc />
        [UnitOfWork(IsDisabled = true)]
        public async Task UpSertBalancoHidricoAsync(BalancoHidricoDto modelDto)
        {
            var model = BalancoHidricoDto.Mapear(modelDto);
            var culture = new CultureInfo("pt-br");
            var userId = GetCurrentUser().Id;
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var balancoHidricoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<BalancoHidrico, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var context = balancoHidricoRepository.Object.GetDbContext();

                    foreach (var balancoHidricoSolucao in model.BalancoHidricoSolucoes)
                    {
                        if (!balancoHidricoSolucao.IsTransient())
                        {
                            context.Entry(balancoHidricoSolucao).State = EntityState.Modified;
                        }
                        else
                        {
                            context.Entry(balancoHidricoSolucao).State = EntityState.Added;
                        }
                    }

                    foreach (var balancoHidricoItem in model.BalancoHidricoItems)
                    {
                        if (!balancoHidricoItem.SinaisVitais.IsTransient())
                        {
                            balancoHidricoItem.SinaisVitaisId = balancoHidricoItem.SinaisVitais.Id;
                            context.Entry(balancoHidricoItem.SinaisVitais).State = EntityState.Modified;
                        }

                        if (!balancoHidricoItem.IsTransient())
                        {
                            context.Entry(balancoHidricoItem).State = EntityState.Modified;
                        }

                        foreach (var balancoHidricoItemEndovenoso in balancoHidricoItem.Endovenosos)
                        {
                            if (!balancoHidricoItemEndovenoso.IsTransient())
                            {
                                context.Entry(balancoHidricoItemEndovenoso).State = EntityState.Modified;
                            }
                            else
                            {
                                context.Entry(balancoHidricoItemEndovenoso).State = EntityState.Added;
                            }
                        }
                    }

                    var itemGeral = model.BalancoHidricoItems.FirstOrDefault(x => x.TotalGeral);

                    if (itemGeral != null)
                    {
                        var items = model.BalancoHidricoItems.Where(x => x.TotalParcial).OrderBy(x => x.Hora).ToList();

                        foreach (var itemEndovenoso in itemGeral.Endovenosos)
                        {
                            itemEndovenoso.BalancoHidricoItemId = itemGeral.Id;
                            if (!itemEndovenoso.IsTransient())
                            {
                                context.Entry(itemEndovenoso).State = EntityState.Modified;
                            }
                            else
                            {
                                context.Entry(itemEndovenoso).State = EntityState.Added;
                            }

                            itemEndovenoso.Valor = items.SelectMany(x => x.Endovenosos.Where(y => y.IndiceSolucao == itemEndovenoso.IndiceSolucao))
                                .Sum(x => FormatterHelper.StringToDouble(x.Valor, culture)).ToString(culture);
                        }

                        itemGeral.Diurese = items.Sum(x => FormatterHelper.StringToDouble(x.Diurese, culture)).ToString(culture);
                        itemGeral.Enteral = items.Sum(x => FormatterHelper.StringToDouble(x.Enteral, culture)).ToString(culture);
                        itemGeral.Hd = items.Sum(x => FormatterHelper.StringToDouble(x.Hd, culture)).ToString(culture);
                        itemGeral.Dreno = items.Sum(x => FormatterHelper.StringToDouble(x.Dreno, culture)).ToString(culture);
                        itemGeral.Dreno2 = items.Sum(x => FormatterHelper.StringToDouble(x.Dreno2, culture)).ToString(culture);
                        itemGeral.IngestVoSne = items.Sum(x => FormatterHelper.StringToDouble(x.IngestVoSne, culture)).ToString(culture);
                        itemGeral.SangueDerivados = items.Sum(x => FormatterHelper.StringToDouble(x.SangueDerivados, culture)).ToString(culture);
                        model.BalancoHidricoItems.Add(itemGeral);
                    }

                    model.Id = await balancoHidricoRepository.Object.InsertOrUpdateAndGetIdAsync(model).ConfigureAwait(false);

                    await UpdatePesoAlturaAsync(model, modelDto.Altura, modelDto.Peso);
                    

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current?.SaveChanges();

                    unitOfWork.Dispose();
                }

                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await SalvarArquivoBalancoHidricoAsync(model.Id, model.AtendimentoId, model.DataBalancoHidrico).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current?.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Logger.Error("BALANCO HIDRICO: " + e.Message + " " + e.InnerException.Message, e);
                }
                else
                {
                    Logger.Error("BALANCO HIDRICO: " + e.Message, e);
                }
            }
        }

        public async Task<bool> Conferir(long balancoHidricoId)
        {
            BalancoHidrico balancoHidrico = null; 
            using (var balancoHidricoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<BalancoHidrico, long>>())
            {
                balancoHidrico = await balancoHidricoRepository.Object.FirstOrDefaultAsync(balancoHidricoId).ConfigureAwait(false);
                if (balancoHidrico == null)
                {
                    throw new UserFriendlyException("Balanço Hídrico inexistente");
                }

                if (!balancoHidrico.ConferidoManha)
                {
                    balancoHidrico.ConferidoManha = true;
                    balancoHidrico.ConferidoManhaUserId = AbpSession.UserId;
                    balancoHidrico.DtConferidoManha = DateTime.Now;
                }
                else if (!balancoHidrico.ConferidoNoite)
                {
                    balancoHidrico.ConferidoNoite = true;
                    balancoHidrico.ConferidoNoiteUserId = AbpSession.UserId;
                    balancoHidrico.DtConferidoNoite = DateTime.Now;
                }
                else if (balancoHidrico.ConferidoManha && balancoHidrico.ConferidoNoite)
                {
                    balancoHidrico.ConferidoTotal = true;
                    balancoHidrico.ConferidoTotalUserId = AbpSession.UserId;
                    balancoHidrico.DtConferidoTotal = DateTime.Now;
                }

                await balancoHidricoRepository.Object.InsertOrUpdateAndGetIdAsync(balancoHidrico).ConfigureAwait(false);
            }

            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                await SalvarArquivoBalancoHidricoAsync(balancoHidrico.Id, balancoHidrico.AtendimentoId, balancoHidrico.DataBalancoHidrico).ConfigureAwait(false);
                unitOfWork.Complete();
                unitOfWorkManager.Object.Current?.SaveChanges();
                unitOfWork.Dispose();
            }

            return true;
        }

        public async Task<bool> Desconferir(long balancoHidricoId)
        {
            BalancoHidrico balancoHidrico = null;
            using (var balancoHidricoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<BalancoHidrico, long>>())
            {
                balancoHidrico = await balancoHidricoRepository.Object.FirstOrDefaultAsync(balancoHidricoId).ConfigureAwait(false);
                if (balancoHidrico == null)
                {
                    throw new UserFriendlyException("Balanço Hídrico inexistente");
                }
                if (balancoHidrico.ConferidoManha && balancoHidrico.ConferidoNoite && balancoHidrico.ConferidoTotal)
                {
                    balancoHidrico.ConferidoTotal = false;
                    balancoHidrico.DesConferidoTotalUserId = AbpSession.UserId;
                    balancoHidrico.DtDesConferidoTotal = DateTime.Now;
                }
                else if (balancoHidrico.ConferidoNoite)
                {
                    balancoHidrico.ConferidoNoite = false;
                    balancoHidrico.DesConferidoNoiteUserId = AbpSession.UserId;
                    balancoHidrico.DtDesConferidoNoite = DateTime.Now;
                }
                else if (balancoHidrico.ConferidoManha)
                {
                    balancoHidrico.ConferidoManha = false;
                    balancoHidrico.DesConferidoManhaUserId = AbpSession.UserId;
                    balancoHidrico.DtDesConferidoManha = DateTime.Now;
                }

                await balancoHidricoRepository.Object.InsertOrUpdateAndGetIdAsync(balancoHidrico).ConfigureAwait(false);
            }

            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                await SalvarArquivoBalancoHidricoAsync(balancoHidrico.Id, balancoHidrico.AtendimentoId, balancoHidrico.DataBalancoHidrico).ConfigureAwait(false);
                unitOfWork.Complete();
                unitOfWorkManager.Object.Current?.SaveChanges();
                unitOfWork.Dispose();
            }
            return true;
        }
        /// <inheritdoc />
        /// <summary>
        /// The obter balanco 24 hrs async.
        /// </summary>
        /// <param name="modelId">
        /// The model id.
        /// </param>
        /// <param name="dateValue">
        /// The date value.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" />.
        /// </returns>
        public async Task<BalancoHidrico24HrsViewModel> ObterBalanco24HrsAsync(long modelId, DateTime dateValue)
        {
            var culture = new CultureInfo("pt-BR");
            var model = new BalancoHidrico24HrsViewModel { Atualizado = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") };

            var ultimoBalanco = await ObterAsync(modelId, dateValue.Date.AddDays(-1)).ConfigureAwait(false);

            var geral = ultimoBalanco?.BalancoHidricoItems?.FirstOrDefault(x => x.TotalGeral);

            if (geral != null)
            {
                var iv = geral.Endovenosos.Sum(x => FormatterHelper.StringToDouble(x.Valor, culture));
                var iEvo = FormatterHelper.StringToDouble(geral.IngestVoSne, culture);
                var sEd = FormatterHelper.StringToDouble(geral.SangueDerivados, culture);
                var enteral = FormatterHelper.StringToDouble(geral.Enteral, culture);

                var diur = FormatterHelper.StringToDouble(geral.Diurese, culture);
                var dreno = FormatterHelper.StringToDouble(geral.Dreno, culture);
                var dreno2 = FormatterHelper.StringToDouble(geral.Dreno2, culture);
                var hd = FormatterHelper.StringToDouble(geral.Hd, culture);


                var tpIntro = iv + iEvo + sEd + enteral;
                var tpEli = diur + dreno + dreno2 + hd;
                var tG = tpIntro - tpEli;

                model.Iv = iv.ToString(culture);
                model.IeVO = iEvo.ToString(culture);
                model.SeD = sEd.ToString(culture);
                model.Enteral = enteral.ToString(culture);
                model.TpIntro = tpIntro.ToString(culture);

                model.Hd = hd.ToString(culture);
                model.Dreno = dreno.ToString(culture);
                model.Dreno2 = dreno2.ToString(culture);
                model.Diur = diur.ToString(culture);



                model.TpEli = tpEli.ToString(culture);

                model.TG = tG.ToString(culture);
            }

            var balancoAtual = await ObterAsync(modelId, dateValue.Date).ConfigureAwait(false);

            var geralAtual = balancoAtual?.BalancoHidricoItems?.FirstOrDefault(x => x.TotalGeral);

            if (geralAtual != null)
            {
                var iv = geralAtual.Endovenosos.Sum(x => FormatterHelper.StringToDouble(x.Valor, culture));
                var iEvo = FormatterHelper.StringToDouble(geralAtual.IngestVoSne, culture);
                var sEd = FormatterHelper.StringToDouble(geralAtual.SangueDerivados, culture);
                var enteral = FormatterHelper.StringToDouble(geralAtual.Enteral, culture);
                var diur = FormatterHelper.StringToDouble(geralAtual.Diurese, culture);
                var dreno = FormatterHelper.StringToDouble(geralAtual.Dreno, culture);
                var dreno2 = FormatterHelper.StringToDouble(geralAtual.Dreno2, culture);
                var hd = FormatterHelper.StringToDouble(geralAtual.Hd, culture);

                var tpIntro = iv + iEvo + sEd + enteral;
                var tpEli = diur + dreno + dreno2 + hd;
                var tG = tpIntro - tpEli;
                model.BalancoAtual = tG.ToString(culture);
            }

            var historico = await ObterHistoricoAsync(modelId, dateValue).ConfigureAwait(false);
            if (historico.IsNullOrEmpty())
            {
                return model;
            }
            var balancoCumulativo = 0d;
            foreach (var totalGeral in historico.SelectMany(c => c.BalancoHidricoItems.Where(x => x.TotalGeral)))
            {
                var historicoIv = totalGeral.Endovenosos.Sum(x => FormatterHelper.StringToDouble(x.Valor, culture));
                var historicoIng = FormatterHelper.StringToDouble(totalGeral.IngestVoSne, culture) + FormatterHelper.StringToDouble(totalGeral.SangueDerivados, culture);
                var historicoDiur = FormatterHelper.StringToDouble(totalGeral.Diurese, culture);
                var historicoDreno = FormatterHelper.StringToDouble(totalGeral.Dreno, culture);
                var historicoDreno2 = FormatterHelper.StringToDouble(totalGeral.Dreno2, culture);

                var historicoTpIntro = historicoIv + historicoIng;
                var historicoTpEli = historicoDiur + historicoDreno + historicoDreno2;
                var historicoTg = historicoTpIntro - historicoTpEli;

                balancoCumulativo += historicoTg;
            }

            model.BalancoCumulativo = balancoCumulativo.ToString(culture);

            return model;
        }


        public static async Task<byte[]> SalvarArquivoBalancoHidricoAsync(long id, long atendimentoId, DateTime? date)
        {
            using (var RegistroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
            {
                var arquivo = await BalancoHidricoRelatorio(atendimentoId, date).ConfigureAwait(false);
                var registroArquivo = new RegistroArquivo
                {
                    RegistroTabelaId = (long)EnumArquivoTabela.BalancoHidrico,
                    RegistroId = id,
                    AtendimentoId = atendimentoId
                };

                var fileName = $"{Guid.NewGuid()}.pdf";
                var uploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.UploadFilesPath"]);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                File.WriteAllBytes(Path.Combine(uploadPath, fileName), arquivo);

                registroArquivo.ArquivoNome = fileName;
                registroArquivo.ArquivoTipo = "application/pdf";

                await RegistroArquivoRepository.Object.InsertOrUpdateAndGetIdAsync(registroArquivo).ConfigureAwait(false);

                return arquivo;
            }
        }

        public static async Task<byte[]> BalancoHidricoRelatorio(long id, DateTime? date)
        {
            try
            {
                var baseUrl = ConfigurationManager.AppSettings.Get("baseUrl");
                var viewModel = await BalancoHidricoGetData(id, date).ConfigureAwait(false);
                var viewModelReport = new BalancoHidricoReportViewModel(viewModel, baseUrl);

                viewModelReport.BalancoHidrico24Hrs = viewModelReport.BalancoHidrico24Hrs;
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                var templateContent = File.ReadAllText(Path.Combine(baseDir, "Areas\\Mpa\\Views\\Aplicacao\\Assistenciais\\BalancoHidrico\\BalancoHidricoRelatorio.template"));

                var htmlConteudo = BuildBalancoHidricoFile(templateContent, viewModelReport);

                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
                {
                    PageFooterHtml =
                        $@"<div style=""width:100%;text-align:right""><span style=""text-align:left;left:0px"">SWManager - TSW Tecnologia em Saúde</span> <span></span> <span class=""page""></span>/<span class=""topage""></span></div>",
                    CustomWkHtmlArgs = " --javascript-delay 100"
                };

                return htmlToPdf.GeneratePdf(htmlConteudo.ToString());
            }
            catch (Exception e)
            {

            }
            return null;
        }

        private static StringBuilder BuildBalancoHidricoFile(string templateContent, BalancoHidricoReportViewModel viewModelReport)
        {
            viewModelReport.Model.BalancoHidricoItems = viewModelReport.Model.BalancoHidricoItems
                .OrderBy(c => c.Hora, new BalancoHidricoComparer(TimeSpan.FromHours(viewModelReport.Model.HoraIntervalo)))
                .ThenBy(c => (c.TotalGeral ? 1 : 0))
                .ThenBy(c => (c.TotalTransporte ? 1 : 0))
                .ThenBy(c => (c.TotalParcial ? 1 : 0)).ToList();

            viewModelReport.Model.BalancoHidricoSolucoes = viewModelReport.Model.BalancoHidricoSolucoes.OrderBy(c => c.IndiceSolucao).ToList();

            viewModelReport.Model.BalancoHidricoItems = viewModelReport.Model.BalancoHidricoItems.Select(x =>
            {
                x.Endovenosos = x.Endovenosos.OrderBy(c => c.IndiceSolucao).ToList();
                return x;
            }).ToList();


            viewModelReport.Atendimento.Paciente.PacienteDiagnosticos = viewModelReport.Atendimento.Paciente.PacienteDiagnosticos.OrderByDescending(c => c.DataDiagnostico).ToList();
            var currentDiagnostico = viewModelReport.Atendimento.Paciente.PacienteDiagnosticos.FirstOrDefault(c => c.AtendimentoId == viewModelReport.Atendimento.Id);

            var balancoHidrico24Evacuacoes = "";

            if (viewModelReport.BalancoHidrico24 != null && viewModelReport.BalancoHidrico24.Evacuacoes.HasValue && viewModelReport.BalancoHidrico24.Evacuacoes.Value)
            {
                balancoHidrico24Evacuacoes = "Sim";
            }
            else if (viewModelReport.BalancoHidrico24 != null && viewModelReport.BalancoHidrico24.Evacuacoes.HasValue && !viewModelReport.BalancoHidrico24.Evacuacoes.Value)
            {
                balancoHidrico24Evacuacoes = "Não";
            }

            var textoIdade = string.Empty;
            var idade = DateDifference.GetExtendedDifference(viewModelReport.Atendimento.Paciente.Nascimento ?? DateTime.Today, DateTime.Today);

            if (idade != null)
            {
                textoIdade = string.Format("{0} anos, {1} meses e {2} dias", idade.Ano, idade.Mes, idade.Dia);
            }

            var imgFotoTag = "";
            if (viewModelReport.Atendimento.Paciente.Foto == null || viewModelReport.Atendimento.Paciente.Foto.Length == 0)
            {
                imgFotoTag = @$"<img src=""{viewModelReport.Url}/Common/Images/default-profile-picture.png"" class=""img-circle"" style=""height: 65px""/>";
            }
            else
            {
                var base64 = Convert.ToBase64String(viewModelReport.Atendimento.Paciente.Foto);
                var medicoImgSrc = string.Format("data:{0};base64,{1}", viewModelReport.Atendimento.Paciente.FotoMimeType, base64);
                imgFotoTag = @$"<img src=""{medicoImgSrc}"" class=""img-circle"" style=""height: 65px; padding: 10px""/>";
            }

            var mergeProperties = new Dictionary<string, string>
            {
                { "baseUrl", viewModelReport.Url  },
                { "balancoDate", viewModelReport.BalancoDate.Value.ToString("dd/MM/yyyy")},
                { "nomeCompleto", viewModelReport.Atendimento.Paciente.NomeCompleto },
                { "imagemFoto", imgFotoTag },
                { "idade", textoIdade },
                { "sexo", viewModelReport.Atendimento.Paciente.SisPessoa.Sexo.Codigo ?? "-" },
                { "altura", viewModelReport.Atendimento.Paciente.PacientePesos?.Count != 0 ? viewModelReport.Atendimento.Paciente.PacientePesos.OrderByDescending(c => c.DataPesagem).FirstOrDefault().Altura.ToString("F") + "m" : "-" },
                { "peso", viewModelReport.Atendimento.Paciente.PacientePesos?.Count != 0 ? viewModelReport.Atendimento.Paciente.PacientePesos.OrderByDescending(c => c.DataPesagem).FirstOrDefault().Valor.ToString("F") + " Kg" : "-" },
                { "codigoAtendimento", viewModelReport.Atendimento.Codigo },
                { "dataRegistro", viewModelReport.Atendimento.DataRegistro.ToString("dd/MM/yyyy") ?? "" },
                { "convenio", viewModelReport.Atendimento.Convenio?.SisPessoa != null ? viewModelReport.Atendimento.Convenio.SisPessoa.NomeFantasia : string.Empty },
                { "unidadeOrganizacional", viewModelReport.Atendimento.UnidadeOrganizacional != null ? viewModelReport.Atendimento.UnidadeOrganizacional.Descricao : "-" },
                { "leito", viewModelReport.Atendimento.Leito != null ? viewModelReport.Atendimento.Leito.Descricao : "" },
                { "diagnostico", currentDiagnostico != null && currentDiagnostico.GrupoCID != null ? currentDiagnostico.GrupoCID.Codigo + " - " + currentDiagnostico.GrupoCID.Descricao : "-" },
                { "tipoAcomodacao", viewModelReport.Model.TipoAcomodacao },
                { "diasNaAcomodacao", viewModelReport.Model.DiasNaAcomodacao.ToString() },
                { "evacuacoes",  viewModelReport.Model.Evacuacoes.HasValue && viewModelReport.Model.Evacuacoes.Value ? "Sim" : viewModelReport.Model.Evacuacoes.HasValue && !viewModelReport.Model.Evacuacoes.Value ? "Não" : ""},
                { "aspecto", viewModelReport.Model.Aspecto },
                // balancoHidrico24
                { "balancoHidrico24HrsIv", viewModelReport.BalancoHidrico24Hrs.Iv },
                { "balancoHidrico24HrsDiur", viewModelReport.BalancoHidrico24Hrs.Diur },
                { "balancoHidrico24HrsSeD", viewModelReport.BalancoHidrico24Hrs.SeD },
                { "balancoHidrico24HrsDreno", viewModelReport.BalancoHidrico24Hrs.Dreno },
                { "balancoHidrico24HrsIeVO", viewModelReport.BalancoHidrico24Hrs.IeVO },
                { "balancoHidrico24HrsDreno2", viewModelReport.BalancoHidrico24Hrs.Dreno2 },
                { "balancoHidrico24HrsEnteral", viewModelReport.BalancoHidrico24Hrs.Enteral },
                { "balancoHidrico24HrsHd", viewModelReport.BalancoHidrico24Hrs.Hd },
                { "balancoHidrico24HrsTpIntro", viewModelReport.BalancoHidrico24Hrs.TpIntro },
                { "balancoHidrico24HrsTpEli", viewModelReport.BalancoHidrico24Hrs.TpEli },
                { "balancoHidrico24HrsTG", viewModelReport.BalancoHidrico24Hrs.TG },
                { "balancoHidrico24HrsBalancoCumulativo", viewModelReport.BalancoHidrico24Hrs.BalancoCumulativo },
                { "balancoHidrico24HrsBalancoAtual", viewModelReport.BalancoHidrico24Hrs.BalancoAtual },
                { "balancoHidrico24HrsBalancoAtualizado", viewModelReport.BalancoHidrico24Hrs.Atualizado },
                { "balancoHidrico24Evacuacoes", balancoHidrico24Evacuacoes },
                { "balancoHidrico24Aspecto", viewModelReport.BalancoHidrico24 != null ? viewModelReport.BalancoHidrico24.Aspecto: "" },
                { "balancoHidricoSolucoesContent", CriaBalancoHidricoSolucoes(viewModelReport) },
                { "balancoHidricoSolucoesColSpan" , viewModelReport.Model.BalancoHidricoSolucoes.Count().ToString()},
                { "balancoHidricoColSpan" , (viewModelReport.Model.BalancoHidricoSolucoes.Count() + 9).ToString()},
                { "balancoHidricoInElColSpan" , (viewModelReport.Model.BalancoHidricoSolucoes.Count() + 3).ToString()},
                { "balancoHidricoSolucoesWidth", (viewModelReport.Model.BalancoHidricoSolucoes.Count() * 65).ToString()},
                { "balancoHidricoSolucoesTHeads", CriarBalancoHidricoSolucoesTHeads(viewModelReport) },
                { "balancoHidricoTbody", CriarBalancoHidricoTbody(viewModelReport) }
            };

            CriarSinaisVitaisSumario(mergeProperties, viewModelReport);

            var sb = new StringBuilder(templateContent);

            foreach (var property in mergeProperties)
            {
                sb.Replace($"{{{{{property.Key}}}}}", property.Value);
            }

            // baseUrl
            // balancoDate
            // nomeCompleto
            // idade
            // sexo
            // altura
            // peso
            // codigoAtendimento
            // dataRegistro
            // convenio
            // unidadeOrganizacional
            // leito
            // diagnostico
            // tipoAcomodacao
            // diasNaAcomodacao

            // balancoHidrico24HrsIv
            // balancoHidrico24HrsDiur
            // balancoHidrico24HrsSeD
            // balancoHidrico24HrsDreno
            // balancoHidrico24HrsIeVO
            // balancoHidrico24HrsDreno2
            // balancoHidrico24HrsEnteral
            // balancoHidrico24HrsHd
            // balancoHidrico24HrsTpIntro
            // balancoHidrico24HrsTpEli
            // balancoHidrico24HrsTG
            // balancoHidrico24HrsBalancoCumulativo
            // balancoHidrico24Evacuacoes
            // balancoHidrico24Aspecto
            //sb.Replace()

            return sb;

        }

        private static void CriarSinaisVitaisSumario(Dictionary<string, string> mergeProperties, BalancoHidricoReportViewModel viewModelReport)
        {
            if (viewModelReport.BalancoHidrico24 != null && !viewModelReport.BalancoHidrico24.BalancoHidricoItems.IsNullOrEmpty())
            {
                var items = viewModelReport.BalancoHidrico24.BalancoHidricoItems.Where(x => x.SinaisVitaisId != 0);
                var balancoHidrico24SinaisVitaisSumarioTemperaturaLimitViewModel = new LimitViewModel
                {
                    MinDangerValue = 35.4,
                    MaxDangerValue = 37.8,
                    MinLowerWarningValue = 35.6,
                    MaxLowerWarningValue = 35.9,
                    MinHighWarningValue = 37.3,
                    MaxHighWarningValue = 37.7
                };

                mergeProperties.Add(
                    "balancoHidrico24SinaisVitaisSumarioTemperatura",
                     BalancoHidricoViewModel.SinaisVitaisSumario("t", "T", "Temperatura",
                        items.Where(x => !x.TotalGeral && !x.TotalParcial).Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.Temperatura)), false, balancoHidrico24SinaisVitaisSumarioTemperaturaLimitViewModel).ToString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPulso",
                    BalancoHidricoViewModel.SinaisVitaisSumario("p", "P", "Pulso", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.Pulso))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioRespiracao",
                    BalancoHidricoViewModel.SinaisVitaisSumario("r", "R", "Respiração", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.Respiracao))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioSpo2",
                    BalancoHidricoViewModel.SinaisVitaisSumario("spo2", "SPO2", "Saturação do oxigênio no sangue", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.Spo2))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPressaoSistolica",
                    BalancoHidricoViewModel.SinaisVitaisSumario("pas", "PAS", "Pressão arterial sistólica ", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.PressaoSistolica))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPressaoDiastolica",
                    BalancoHidricoViewModel.SinaisVitaisSumario("pad", "PAD", "Pressão arterial diastólica ", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.PressaoDiastolica))).ToHtmlString());


                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioEscalaDeDor",
                    BalancoHidricoViewModel.SinaisVitaisSumario("eva", "EVA", "Escala de dor ", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.EscalaDeDor))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioHemoglucoteste",
                    BalancoHidricoViewModel.SinaisVitaisSumario("hgt", "HGT", "Hemoglucoteste", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.Hemoglucoteste))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioIns",
                    BalancoHidricoViewModel.SinaisVitaisSumario("ins", "INS", "INS ", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.Ins))).ToHtmlString());

                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPressaoIntracraniana",
                    BalancoHidricoViewModel.SinaisVitaisSumario("pic", "PIC", "Pressão Intracraniana", items.Select(x => new SinalVitalViewModel(x.Hora, x.SinaisVitais.PressaoIntracraniana))).ToHtmlString());
            }
            else
            {
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioTemperatura", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPulso", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioRespiracao", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioSpo2", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPressaoSistolica", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPressaoDiastolica", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioEscalaDeDor", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioHemoglucoteste", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioIns", "");
                mergeProperties.Add("balancoHidrico24SinaisVitaisSumarioPressaoIntracraniana", "");
            }
        }

        private static string CriaBalancoHidricoSolucoes(BalancoHidricoReportViewModel viewModelReport)
        {
            var solucoesArray = viewModelReport.Model.BalancoHidricoSolucoes.ToArray();
            var totalSolucoes = solucoesArray.Length + 1;

            var totalColunas = totalSolucoes / 2;
            var sb = new StringBuilder();
            for (var index = 0; index < totalColunas; index++)
            {
                sb.Append(@"<div class=""col-xs-2"">");
                if (solucoesArray.ElementAtOrDefault(index) != null)
                {
                    sb.Append(@$"
                        <div class=""row"">
                            <div class=""col-xs-12"">
                                <span class=""number-size b-5""> {(solucoesArray[index].IndiceSolucao)}. </span>
                                <span class=""input-number-size""> {solucoesArray[index].Valor}</span>
                            </div>
                        </div>");
                }

                if (solucoesArray.ElementAtOrDefault(index + totalColunas) != null)
                {
                    sb.Append(@$"
                        <div class=""row"">
                            <div class=""col-xs-12"">
                                <span class=""number-size b-5""> {solucoesArray[index + totalColunas].IndiceSolucao}. </span>
                                <span class=""input-number-size"">{solucoesArray[index + totalColunas].Valor}</span>
                            </div>
                        </div>");
                }
                sb.Append(@"</div>");
            }

            return sb.ToString();
        }

        private static string CriarBalancoHidricoSolucoesTHeads(BalancoHidricoReportViewModel viewModelReport)
        {

            var sb = new StringBuilder();
            foreach (var balancoHidricoSolucao in viewModelReport.Model.BalancoHidricoSolucoes)
            {
                sb.Append(@$"
                    <th class=""header-padding text-center solucao num_{balancoHidricoSolucao.IndiceSolucao}"">
                        <span style=""width:50px""> {balancoHidricoSolucao.IndiceSolucao} </span>
                    </th>");
            }
            return sb.ToString();
        }


        private static string CriarBalancoHidricoSolucoesTd(BalancoHidricoItemDto balancoHidricoItem)
        {
            var sb = new StringBuilder();

            foreach (var balancoHidricoEndovenoso in balancoHidricoItem.Endovenosos)
            {
                sb.Append(@$"<td class=""header-padding solucao num_{balancoHidricoEndovenoso.IndiceSolucao}""> <span class=""td-padding"">{balancoHidricoEndovenoso.Valor}</span></td>");
            }

            return sb.ToString();

        }

        private static string CriarBalancoConferidoManha(BalancoHidricoItemDto balancoHidricoItem, BalancoHidricoReportViewModel viewModelReport)
        {
            var sb = new StringBuilder();

            if (balancoHidricoItem.Hora == TimeSpan.FromHours(6))
            {
                var totalManha = viewModelReport.Model.BalancoHidricoItems.Count(x => x.Hora >= TimeSpan.FromHours(7) && x.Hora <= TimeSpan.FromHours(18)).ToString();
                sb.Append($@"<td class=""vertical-table-column vertical-table-column-manha"" style=""border-bottom: 1px solid black !important;background-color: white !important;color:black !important"" rowspan=""{totalManha}"">
                                <span class=""text-content"" style=""margin-left: 34px;"">");
                if (viewModelReport.Model.ConferidoManha)
                {
                    sb.Append($@"<span class=""user-name"" style=""margin: 0px 14px 0px 8px;"">{viewModelReport.Model.ConferidoManhaUserName} </span>");
                    if (viewModelReport.Model.DtConferidoManha.HasValue)
                    {
                        sb.Append($@"<span class=""date"">{viewModelReport.Model.DtConferidoManha.Value.ToString("dd/MM/yyyy HH:mm:ss")}</span>");
                    }
                }
                else
                {
                    sb.Append($@"<span class=""user-name"" > Não Conferido </span>");
                }
                sb.Append($@"</span> </td>");
            }

            return sb.ToString();

        }

        private static string CriarBalancoConferidoTotal(BalancoHidricoItemDto balancoHidricoItem, BalancoHidricoReportViewModel viewModelReport)
        {
            var sb = new StringBuilder();

            if (balancoHidricoItem.Hora == TimeSpan.FromHours(6))
            {
                sb.Append($@"
                    <td class=""vertical-table-column vertical-table-column-total"" style=""background-color: white !important;color:black !important"" rowspan=""{viewModelReport.Model.BalancoHidricoItems.Count + 2}"">
                        <span class=""text-content"" style=""margin-left: 34px;"">");
                if (viewModelReport.Model.ConferidoTotal)
                {
                    sb.Append($@"<span class=""user-name"" style=""margin: 0px 14px 0px 8px;"">{viewModelReport.Model.ConferidoTotalUserName} </span>");
                    if (viewModelReport.Model.DtConferidoTotal.HasValue)
                    {
                        sb.Append($@"<span class=""date"">{viewModelReport.Model.DtConferidoTotal.Value.ToString("dd/MM/yyyy HH:mm:ss")} </span>");
                    }
                }
                else
                {
                    sb.Append($@"<span class= ""user-name"" >Não Conferido </span>");
                }
                sb.Append($@"</span> </td>");
            }

            return sb.ToString();

        }

        private static string CriarBalancoHidricoTbody(BalancoHidricoReportViewModel viewModelReport)
        {
            var sb = new StringBuilder();

            var balancoHidricoItemFirstParcial = viewModelReport.Model.BalancoHidricoItems.Where(x => x.TotalTransporte && x.Hora == TimeSpan.FromHours(6)).FirstOrDefault();
            var BalancoHidricoItems = viewModelReport.Model.BalancoHidricoItems.ToList();

            if (balancoHidricoItemFirstParcial != null)
            {
                if (balancoHidricoItemFirstParcial.SinaisVitais == null)
                {
                    balancoHidricoItemFirstParcial.SinaisVitais = new BalancoHidricoSinaisVitaisDto();
                }
                sb.Append($@"
                    <tr class=""transporte"" 
                        data-index=""{balancoHidricoItemFirstParcial.Hora.Hours}"" 
                        data-hora=""{balancoHidricoItemFirstParcial.Hora.ToString(@"hh\:mm")}"" 
                        data-transp=""{balancoHidricoItemFirstParcial.TotalTransporte.ToString().ToLower()}"" 
                        data-tp=""{balancoHidricoItemFirstParcial.TotalParcial.ToString().ToLower()}"" 
                        data-tg=""{balancoHidricoItemFirstParcial.TotalGeral.ToString().ToLower()}"" 
                        data-id=""{balancoHidricoItemFirstParcial.Id}"" 
                        data-sinais-id=""{balancoHidricoItemFirstParcial.SinaisVitais.Id}"">
                        <td class=""header-padding hora sticky-col first-col""> <h6 class=""header-padding text-center"">Transp</h6> </td>
                        <td class=""header-padding temp""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.Temperatura}</span> </td>
                        <td class=""header-padding pulso""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.Pulso}</span> </td>
                        <td class=""header-padding resp""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.Respiracao}</span> </td>
                        <td class=""header-padding spo2""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.Spo2}</span> </td>
                        <td class=""header-padding pa"">
                            <span class=""td-padding"" style=""width: 40%;float: left;display: inline-block;padding-right: 0px;"">{balancoHidricoItemFirstParcial.SinaisVitais.PressaoSistolica}</span>
                            <span class=""td-padding"" style=""width: 15%;padding: 2px 0px;display: inline-block;"">/</span>
                            <span class=""td-padding"" style=""width: 40%; float: right;display: inline-block;padding-left: 0px;"">{balancoHidricoItemFirstParcial.SinaisVitais.PressaoDiastolica}</span>
                        </td>
                        <td class=""header-padding eva""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.EscalaDeDor}</span> </td>
                        <td class=""header-padding hgt""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.Hemoglucoteste}</span></td>
                        <td class=""header-padding ins""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.Ins}</span> </td>                        
                        <td class=""header-padding pic""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.SinaisVitais.PressaoIntracraniana}</span> </td>
                        {CriarBalancoHidricoSolucoesTd(balancoHidricoItemFirstParcial)}
                        <td class=""header-padding sangue""> <span class=""td-padding"" style=""width: 100%"">{balancoHidricoItemFirstParcial.SangueDerivados}</span> </td>
                        <td class=""header-padding enteral""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.Enteral}</span> </td>
                        <td class=""header-padding ingesta""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.IngestVoSne}</span> </td>
                        <td class=""header-padding diurese""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.Diurese}</span> </td>
                        <td class=""header-padding dreno_""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.Hd}</span> </td>
                        <td class=""header-padding dreno_""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.Dreno}</span> </td>
                        <td class=""header-padding dreno_2_""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.Dreno2}</span> </td>
                        <td class=""header-padding ie""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.IrrigacaodeEntrada}</span> </td>
                        <td class=""header-padding is""> <span class=""td-padding"">{balancoHidricoItemFirstParcial.IrrigacaodeSaida}</span> </td>
                        {CriarBalancoConferidoManha(balancoHidricoItemFirstParcial, viewModelReport)}
                        {CriarBalancoConferidoTotal(balancoHidricoItemFirstParcial, viewModelReport)}
                    </tr>");
            }

            foreach (var balancoHidricoItem in BalancoHidricoItems)
            {
                if (balancoHidricoItem == balancoHidricoItemFirstParcial)
                {
                    continue;
                }

                if (balancoHidricoItem.SinaisVitais == null)
                {
                    balancoHidricoItem.SinaisVitais = new BalancoHidricoSinaisVitaisDto();
                }

                if (!balancoHidricoItem.TotalParcial && !balancoHidricoItem.TotalGeral && !balancoHidricoItem.TotalTransporte)
                {
                    sb.Append($@"
                        <tr>
                            <td class=""header-padding hora""> <h6 class=""header-padding text-center"" > {balancoHidricoItem.Hora.ToString(@"hh\:mm")} </h6> </td>
                            <td class=""header-padding temp""> <span class=""td-padding""> {balancoHidricoItem.SinaisVitais.Temperatura} </span> </td>
                            <td class=""header-padding pulso""> <span class= ""td-padding""> {balancoHidricoItem.SinaisVitais.Pulso} </span> </td>
                            <td class=""header-padding resp""> <span class=""td-padding""> {balancoHidricoItem.SinaisVitais.Respiracao} </span> </td>
                            <td class=""header-padding spo2""> <span class=""td-padding""> {balancoHidricoItem.SinaisVitais.Spo2} </span></td>
                            <td class=""header-padding pa"">
                                <span class=""td-padding"" style=""width: 40%;float: left;display: inline-block;padding-right: 0px;""> {balancoHidricoItem.SinaisVitais.PressaoSistolica}</span>
                                <span class=""td-padding"" style=""width: 15%;padding: 2px 0px;display: inline-block;"">/</span>
                                <span class=""td-padding"" style=""width: 40%; float: right;display: inline-block;padding-left: 0px;""> {balancoHidricoItem.SinaisVitais.PressaoDiastolica}</span>
                            </td>
                            <td class=""header-padding eva""> <span class=""td-padding""> {balancoHidricoItem.SinaisVitais.EscalaDeDor} </span> </td>
                            <td class=""header-padding hgt""><span class=""td-padding"">{balancoHidricoItem.SinaisVitais.Hemoglucoteste} </span> </td>
                            <td class=""header-padding ins""><span class=""td-padding"">{balancoHidricoItem.SinaisVitais.Ins}</span></td>
                            <td class=""header-padding pic""><span class=""td-padding"" > {balancoHidricoItem.SinaisVitais.PressaoIntracraniana}</span></td>
                            {CriarBalancoHidricoSolucoesTd(balancoHidricoItem)}
                            <td class=""header-padding sangue""> <span class=""td-padding"" style=""width: 100%""> {balancoHidricoItem.SangueDerivados} </span> </td>
                            <td class=""header-padding enteral""><span class=""td-padding""> {balancoHidricoItem.Enteral} </span> </td>
                            <td class=""header-padding ingesta""> <span class=""td-padding""> {balancoHidricoItem.IngestVoSne}</span> </td>
                            <td class=""header-padding diurese""> <span class=""td-padding""> {balancoHidricoItem.Diurese} </span> </td>
                            <td class=""header-padding dreno_""> <span class=""td-padding"" > {balancoHidricoItem.Hd} </span> </td>
                            <td class=""header-padding dreno_""> <span class= ""td-padding""> {balancoHidricoItem.Dreno} </span> </td>
                            <td class=""header-padding dreno_2_""> <span class=""td-padding""> {balancoHidricoItem.Dreno2} </span></td>
                            <td class=""header-padding ie""> <span class=""td-padding"" > {balancoHidricoItem.IrrigacaodeEntrada} </span></td>
                            <td class=""header-padding is""> <span class= ""td-padding"" > {balancoHidricoItem.IrrigacaodeSaida} </span> </td>");

                    if (balancoHidricoItem.Hora == TimeSpan.FromHours(7) && balancoHidricoItemFirstParcial == null)
                    {
                        var totalManha = viewModelReport.Model.BalancoHidricoItems.Count(x => x.Hora >= TimeSpan.FromHours(7) && x.Hora <= TimeSpan.FromHours(18));
                        sb.Append($@"<td class=""vertical-table-column vertical-table-column-manha"" style=""border-bottom: 1px solid black !important;"" rowspan=""{totalManha}"">
                                    <span class= ""text-content"" style=""margin-left: 34px;"">");
                        if (viewModelReport.Model.ConferidoManha)
                        {
                            sb.Append($@"<span class=""user-name"" style=""margin: 0px 14px 0px 8px;"" >{viewModelReport.Model.ConferidoManhaUserName} </span>");
                            if (viewModelReport.Model.DtConferidoManha.HasValue)
                            {
                                sb.Append($@"<span class=""date""> {viewModelReport.Model.DtConferidoManha.Value.ToString("dd/MM/yyyy HH:mm:ss")} </span>");
                            }
                        }
                        else
                        {
                            sb.Append($@"<span class=""user-name"" > Não Conferido </span>");
                        }
                        sb.Append($@"</span></td>");
                    }

                    if (balancoHidricoItem.Hora == TimeSpan.FromHours(19))
                    {
                        var totalNoite = viewModelReport.Model.BalancoHidricoItems.Count - viewModelReport.Model.BalancoHidricoItems.Count(x => x.Hora >= TimeSpan.FromHours(7) && x.Hora <= TimeSpan.FromHours(18));
                        sb.Append($@"<td class=""vertical-table-column vertical-table-column-noite"" rowspan =""{totalNoite + 2}"">
                                  <span class=""text-content"" style=""margin-left: 34px;"">");
                        if (viewModelReport.Model.ConferidoNoite)
                        {
                            sb.Append($@"<span class=""user-name"" style=""margin: 0px 14px 0px 8px;"">{viewModelReport.Model.ConferidoNoiteUserName} </span>");
                            if (viewModelReport.Model.DtConferidoNoite.HasValue)
                            {
                                sb.Append($@"<span class=""date""> {viewModelReport.Model.DtConferidoNoite.Value.ToString("dd/MM/yyyy HH:mm:ss")} </span>");
                            }
                        }
                        else
                        {
                            sb.Append($@"<span class=""user-name"">Não Conferido</span>");
                        }
                        sb.Append($@"</span></td>");
                    }
                    sb.Append($@"</tr>");
                }
                if (balancoHidricoItem.TotalParcial || balancoHidricoItem.TotalGeral || balancoHidricoItem.TotalTransporte)
                {
                    sb.Append($@"
                        <tr>
                            <td class=""header-padding hora""> <h6 class=""header-padding text-center"">{(balancoHidricoItem.TotalTransporte ? "Transp" : balancoHidricoItem.TotalParcial ? "T.P" : "T.G")} </h6></td>
                            <td class=""header-padding temp"" ><span class=""td-padding""> - </span>  </td>
                            <td class=""header-padding pulso""> <span class=""td-padding""> - </span> </td>
                            <td class=""header-padding resp"" ><span class=""td-padding""> - </span> </td>
                            <td class=""header-padding spo2""> <span class= ""td-padding""> - </span> </td>
                            <td class=""header-padding pa""> <span class=""td-padding""> - </span> </td>
                            <td class=""header-padding eva""> <span class=""td-padding""> - </span> </td>
                            <td class=""header-padding hgt""> <span class=""td-padding""> - </span> </td>
                            <td class=""header-padding ins""><span class=""td-padding""> - </span> </td>
                            <td class=""header-padding pic""> <span class=""td-padding""> {balancoHidricoItem.SinaisVitais.PressaoIntracraniana} </span> </td>
                            {CriarBalancoHidricoSolucoesTd(balancoHidricoItem)}
                            <td class=""header-padding sangue""> <span class=""td-padding"">{balancoHidricoItem.SangueDerivados}</span> </td>
                            <td class=""header-padding enteral""> <span class=""td-padding""> {balancoHidricoItem.Enteral} </span> </td>
                            <td class=""header-padding ingesta"" > <span class=""td-padding"" > {balancoHidricoItem.IngestVoSne}</span></td>
                            <td class=""header-padding diurese""> <span class=""td-padding"">{balancoHidricoItem.Diurese}</span> </td>
                            <td class=""header-padding dreno_""> <span class=""td-padding""> {balancoHidricoItem.Hd}</span> </td>
                            <td class=""header-padding dreno_"" > <span class=""td-padding"" > {balancoHidricoItem.Dreno}</span> </td>
                            <td class=""header-padding dreno_2_"" > <span class=""td-padding""> {balancoHidricoItem.Dreno2}</span> </td>
                            <td class=""header-padding ie""> <span class=""td-padding""> {balancoHidricoItem.IrrigacaodeEntrada}</span></td>
                            <td class=""header-padding is""> <span class=""td-padding""> {balancoHidricoItem.IrrigacaodeSaida}</span> </td>
                        </tr>");
                }
            }


            return sb.ToString();
        }

        public static async Task<BalancoHidricoViewModel> BalancoHidricoGetData(long id, DateTime? date)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var balancoHidricoAppService = IocManager.Instance.ResolveAsDisposable<IBalancoHidricoAppService>())
            using (var pacientePesoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PacientePeso, long>>())
            {
                var culture = new CultureInfo("en-US");
                var viewModel = new BalancoHidricoViewModel { Atendimento = await atendimentoAppService.Object.Obter(id).ConfigureAwait(false), BalancoDate = date };
                viewModel.Model = await balancoHidricoAppService.Object.ObterAsync(viewModel.Atendimento.Id, date.Value).ConfigureAwait(false)
                                  ?? balancoHidricoAppService.Object.GerarNovoBalancoHidrico(viewModel.Atendimento.Id, date.Value, BalancoHidricoDto.BalancoHoraIntervalo, BalancoHidricoViewModel.NumSolucoes);

                viewModel.BalancoHidricoAnteriorId = await balancoHidricoAppService.Object.ObterIdBalancoHidricoAnterior(viewModel.Atendimento.Id, viewModel.Model.Id).ConfigureAwait(false);

                var pacientePeso = await pacientePesoRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.PacienteId == viewModel.Atendimento.PacienteId && DbFunctions.TruncateTime(x.DataPesagem) <= DbFunctions.TruncateTime(date)).ConfigureAwait(false);

                viewModel.Model.Altura = pacientePeso?.Altura.ToString("F", culture);
                viewModel.Model.Peso = pacientePeso?.Valor.ToString("F", culture);
                viewModel.BalancoHidrico24Hrs = await balancoHidricoAppService.Object.ObterBalanco24HrsAsync(viewModel.Atendimento.Id, date.Value).ConfigureAwait(false);

                var balancoAnteriorId = await balancoHidricoAppService.Object.ObterIdBalancoHidricoAnterior(viewModel.Atendimento.Id, date.Value).ConfigureAwait(false);

                if (balancoAnteriorId != 0)
                {
                    var bhAnterior = await balancoHidricoAppService.Object.ObterIdAsync(balancoAnteriorId).ConfigureAwait(false);
                    viewModel.BalancoHidrico24 = bhAnterior;
                }


                return viewModel;


            }
        }

        private static void MapConferencia(BalancoHidricoDto dto)
        {
            using (var userRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            {
                if (dto.ConferidoTotal && dto.ConferidoTotalUserId.HasValue)
                {
                    dto.ConferidoTotalUserName =
                        userRepository.Object.FirstOrDefault(dto.ConferidoTotalUserId.Value)?.FullName;
                }

                if (dto.ConferidoNoite && dto.ConferidoNoiteUserId.HasValue)
                {
                    dto.ConferidoNoiteUserName =
                        userRepository.Object.FirstOrDefault(dto.ConferidoNoiteUserId.Value)?.FullName;
                }

                if (dto.ConferidoManha && dto.ConferidoManhaUserId.HasValue)
                {
                    dto.ConferidoManhaUserName =
                        userRepository.Object.FirstOrDefault(dto.ConferidoManhaUserId.Value)?.FullName;
                }
            }
        }

        private async Task<bool> CheckAcaoDesconferir(BalancoHidricoDto dto)
        {
            using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
            {
                var user = await GetCurrentUserAsync().ConfigureAwait(false);
                var permissoes = new List<string>();
                var permissions = await userManager.Object.GetGrantedPermissionsAsync(user).ConfigureAwait(false);
                permissoes = permissions.Where(x => x.Name.Contains(AppPermissions.BalancoHidrico_Desbloqueio) || x.Name.Contains(AppPermissions.BalancoHidrico_ConferenciaTotal)).Select(x => x.Name).ToList();

                if (dto.ConferidoTotal)
                {
                    return CheckDesbloqueioTotal(permissoes);
                }

                if (dto.ConferidoNoite && dto.DtConferidoNoite.HasValue && dto.ConferidoNoiteUserId.HasValue)
                {
                    return CheckDesbloqueioPermission(dto.DtConferidoNoite.Value, dto.ConferidoNoiteUserId.Value, permissoes, user.Id);
                }

                if (dto.ConferidoManha && dto.DtConferidoManha.HasValue && dto.ConferidoManhaUserId.HasValue)
                {
                    return CheckDesbloqueioPermission(dto.DtConferidoManha.Value, dto.ConferidoManhaUserId.Value, permissoes, user.Id);
                }

                return false;
            }

        }

        private static async Task UpdatePesoAlturaAsync(BalancoHidrico model, string altura, string peso)
        {
            if (!altura.IsNullOrEmpty() || !peso.IsNullOrEmpty())
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var pacientePesoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PacientePeso, long>>())
                {
                    var atendimento = await atendimentoRepository.Object.GetAll().Select(x => new { x.Id, x.PacienteId })
                        .FirstOrDefaultAsync(x => x.Id == model.AtendimentoId).ConfigureAwait(false);

                    if (atendimento == null || atendimento.Id == 0)
                    {
                        return;
                    }

                    var pacientePeso = await pacientePesoRepository.Object.FirstOrDefaultAsync(
                                               x => x.PacienteId == atendimento.PacienteId
                                                    && DbFunctions.TruncateTime(x.DataPesagem)
                                                    == DbFunctions.TruncateTime(model.DataBalancoHidrico))
                                           .ConfigureAwait(false) ?? new PacientePeso();

                    pacientePeso.DataPesagem = model.DataBalancoHidrico;
                    pacientePeso.Altura = FormatterHelper.StringToDouble(altura, new CultureInfo("en-US"));
                    pacientePeso.Valor = FormatterHelper.StringToDouble(peso, new CultureInfo("en-US"));

                    // if (atendimento.Paciente.PacientePesos == null)
                    // {
                    //     modelDto.Atendimento.Paciente.PacientePesos = new List<PacientePeso>();
                    // }

                    if (pacientePeso.IsTransient())
                    {
                        pacientePeso.PacienteId = atendimento.PacienteId ?? 0;

                    }

                    await pacientePesoRepository.Object.InsertOrUpdateAsync(pacientePeso).ConfigureAwait(false);
                }
            }
        }

        private static bool CheckDesbloqueioTotal(IEnumerable<string> permissions)
        {
            return permissions.Any(x => x.EndsWith(AppPermissions.BalancoHidrico_ConferenciaTotal));
        }

        private static bool CheckDesbloqueioPermission(DateTime dtConferido, long conferidoUserId, IEnumerable<string> permissions, long currentUserId)
        {
            if (permissions.IsNullOrEmpty())
            {
                return false;
            }

            if (permissions.Any(x => x.EndsWith(AppPermissions.BalancoHidrico_Desbloqueio, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            if (DateTime.Now.Subtract(dtConferido).TotalHours <= 24 && permissions.Any(x =>
                x.EndsWith(AppPermissions.BalancoHidrico_Desbloqueio_Proprio_24hrs, StringComparison.InvariantCultureIgnoreCase) &&
                conferidoUserId == currentUserId))
            {
                return true;
            }

            if (DateTime.Now.Subtract(dtConferido).TotalHours <= 24 &&
                permissions.Any(x => x.EndsWith(AppPermissions.BalancoHidrico_Desbloqueio_24hrs, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            return false;
        }

    }
}