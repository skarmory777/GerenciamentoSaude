using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes
{
    public class DivisaoAppService : SWMANAGERAppServiceBase, IDivisaoAppService
    {
        [UnitOfWork]
        public async Task<DivisaoDto> CriarOuEditar(DivisaoDto input)
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var divisao = DivisaoDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await divisaoRepositorio.Object.InsertAndGetIdAsync(divisao).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            unitOfWorkManager.Object.Current.SaveChanges();
                        }
                        return input;
                    }
                    else
                    {
                        //Eliminando filhos caso o registro tenha sido rebaixado de divisão principal para comum/subdivisao
                        if (!input.IsDivisaoPrincipal)
                        {
                            var subs = await divisaoRepositorio.Object
                                           .GetAll()
                                           .Where(m => m.DivisaoPrincipalId == input.Id)
                                           .ToListAsync().ConfigureAwait(false);

                            using (var unitOfWork = unitOfWorkManager.Object.Begin())
                            {
                                foreach (var sub in subs)
                                {
                                    sub.DivisaoPrincipalId = null;
                                    await divisaoRepositorio.Object.UpdateAsync(sub).ConfigureAwait(false);
                                }
                                unitOfWork.Complete();
                                unitOfWorkManager.Object.Current.SaveChanges();
                                unitOfWork.Dispose();
                            }
                        }
                        else
                        {
                            if (input.DivisaoPrincipalId.HasValue)
                            {
                                divisao.DivisaoPrincipalId = null;
                            }
                        }

                        //salvando registro editado
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            var divisaoAtual = divisaoRepositorio.Object.GetAll()
                                                                  .FirstOrDefault(w => w.Id == input.Id);

                            if (divisaoAtual != null)
                            {
                                divisaoAtual.Codigo = input.Codigo;
                                divisaoAtual.Descricao = input.Descricao;
                                divisaoAtual.DivisaoPrincipalId = input.DivisaoPrincipalId;
                                divisaoAtual.IsAcm = input.IsAcm;
                                divisaoAtual.IsAgora = input.IsAgora;
                                divisaoAtual.IsControlaVolume = input.IsControlaVolume;
                                divisaoAtual.IsCopiarPrescricao = input.IsCopiarPrescricao;
                                divisaoAtual.IsDataInicio = input.IsDataInicio;
                                divisaoAtual.IsDiasAplicacao = input.IsDiasAplicacao;
                                divisaoAtual.IsDivisaoPrincipal = input.IsDivisaoPrincipal;
                                divisaoAtual.IsDuracao = input.IsDuracao;
                                //divisaoAtual.IsEstoque = input.ise;
                                divisaoAtual.IsExameImagem = input.IsExameImagem;
                                divisaoAtual.IsExameLaboratorial = input.IsExameLaboratorial;
                                //divisaoAtual.IsFaturamento = input.f
                                divisaoAtual.IsFormaAplicacao = input.IsFormaAplicacao;
                                divisaoAtual.IsFrequencia = input.IsFrequencia;
                                divisaoAtual.IsMedicamento = input.IsMedicamento;
                                divisaoAtual.IsMedico = input.IsMedico;
                                divisaoAtual.IsObservacao = input.IsObservacao;
                                divisaoAtual.IsProdutoEstoque = input.IsProdutoEstoque;
                                divisaoAtual.IsQuantidade = input.IsQuantidade;
                                divisaoAtual.IsSangueDerivado = input.IsSangueDerivado;
                                divisaoAtual.IsSeNecessario = input.IsSeNecessario;
                                divisaoAtual.IsSetorExame = input.IsSetorExame;
                                divisaoAtual.IsTipoMedicacao = input.IsTipoMedicacao;
                                divisaoAtual.IsUnidadeMedida = input.IsUnidadeMedida;
                                divisaoAtual.IsUniddeOrganizacional = input.IsUniddeOrganizacional;
                                divisaoAtual.IsUrgente = input.IsUrgente;
                                divisaoAtual.IsVelocidadeInfusao = input.IsVelocidadeInfusao;
                                divisaoAtual.Ordem = input.Ordem;
                                divisaoAtual.TipoPrescricaoId = input.TipoPrescricaoId;
                                divisaoAtual.IsDoseUnica = input.IsDoseUnica;


                                await divisaoRepositorio.Object.UpdateAsync(divisaoAtual).ConfigureAwait(false);
                                unitOfWork.Complete();
                                unitOfWorkManager.Object.Current.SaveChanges();
                                unitOfWork.Dispose();

                            }
                        }

                        return input;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task<DivisaoDto> SalvarSubDivisao(DivisaoDto input)
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var divisao = DivisaoDto.Mapear(input); 

                    //registro original (sem os filhos)
                    var divisaoSalvar = await divisaoRepositorio.Object
                                            .GetAll()
                                            //.Include(m => m.TiposRespostas)
                                            .Where(m => m.Id == input.Id)
                                            .FirstOrDefaultAsync().ConfigureAwait(false);

                    //alterando o registro original com os dados enviados pelo usuário
                    divisaoSalvar.Codigo = input.Codigo;
                    divisaoSalvar.CreationTime = divisao.CreationTime;
                    divisaoSalvar.CreatorUserId = divisao.CreatorUserId;
                    divisaoSalvar.DeleterUserId = divisao.DeleterUserId;
                    divisaoSalvar.DeletionTime = divisao.DeletionTime;
                    divisaoSalvar.Descricao = input.Descricao;
                    divisaoSalvar.DivisaoPrincipalId = input.IsDivisaoPrincipal ? null : input.DivisaoPrincipalId;
                    divisaoSalvar.IsDeleted = divisao.IsDeleted;
                    divisaoSalvar.IsDivisaoPrincipal = input.IsDivisaoPrincipal;
                    divisaoSalvar.TipoPrescricaoId = input.TipoPrescricaoId;
                    divisaoSalvar.IsSistema = divisao.IsSistema;
                    divisaoSalvar.LastModificationTime = divisao.LastModificationTime;
                    divisaoSalvar.LastModifierUserId = divisao.LastModifierUserId;
                    divisaoSalvar.Ordem = input.Ordem;

                    //salvando registro editado
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        divisao = await divisaoRepositorio.Object.UpdateAsync(divisaoSalvar).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }

                    return input;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(DivisaoDto input)
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        await divisaoRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<DivisaoDto>> Listar(ListarDivisoesInput input)
        {
            var contarDivisao = 0;
            List<Divisao> divisao;
            List<DivisaoDto> DivisaoDtos = new List<DivisaoDto>();
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = divisaoRepositorio.Object
                    .GetAll()
                    .Where(m => m.IsDivisaoPrincipal)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro) ||
                        m.DivisaoPrincipal.Codigo.Contains(input.Filtro) ||
                        m.DivisaoPrincipal.Descricao.Contains(input.Filtro)
                        );

                    contarDivisao = await query
                                        .CountAsync().ConfigureAwait(false);

                    divisao = await query
                                  .AsNoTracking()
                                  .OrderBy(input.Sorting)
                                  .PageBy(input)
                                  .ToListAsync().ConfigureAwait(false);

                    DivisaoDtos = DivisaoDto.Mapear(divisao).ToList();

                    return new PagedResultDto<DivisaoDto>(
                        contarDivisao,
                        DivisaoDtos
                        );
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<DivisaoDto>> ListarSubDivisoes(ListarDivisoesInput input)
        {
            var contarDivisao = 0;
            List<Divisao> divisao;
            List<DivisaoDto> DivisaoDtos = new List<DivisaoDto>();
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = divisaoRepositorio.Object
                    .GetAll()
                    .Where(m => m.DivisaoPrincipalId == input.DivisaoPrincipalId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarDivisao = await query
                                        .CountAsync().ConfigureAwait(false);

                    divisao = await query
                                  .AsNoTracking()
                                  .OrderBy(input.Sorting)
                                  .PageBy(input)
                                  .ToListAsync().ConfigureAwait(false);

                    DivisaoDtos = DivisaoDto.Mapear(divisao).ToList();

                    return new PagedResultDto<DivisaoDto>(
                        contarDivisao,
                        DivisaoDtos
                        );
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<DivisaoDto>> ListarDivisoesSemRelacao(ListarDivisoesInput input)
        {
            var contarDivisao = 0;
            List<Divisao> divisao;
            List<DivisaoDto> DivisaoDtos = new List<DivisaoDto>();
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = divisaoRepositorio.Object
                    .GetAll()
                    .Where(m => m.DivisaoPrincipalId == null && !m.IsDivisaoPrincipal)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarDivisao = await query
                                        .CountAsync().ConfigureAwait(false);

                    divisao = await query
                                  .AsNoTracking()
                                  .OrderBy(input.Sorting)
                                  .PageBy(input)
                                  .ToListAsync().ConfigureAwait(false);

                    DivisaoDtos = DivisaoDto.Mapear(divisao).ToList();


                    return new PagedResultDto<DivisaoDto>(
                        contarDivisao,
                        DivisaoDtos
                        );
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IEnumerable<DivisaoDto>> ObterPorIds(List<long> ids)
        {
            try
            {
                var query = $@"
                SELECT
                    {QueryHelper.CreateQueryFields<Divisao>().TableAlias("Divisao").GetFields()},
                    {QueryHelper.CreateQueryFields<Divisao>().TableAlias("DivisaoPrincipal").GetFields()},
                    {QueryHelper.CreateQueryFields<TipoPrescricao>().TableAlias("TipoPrescricao").GetFields()}
                FROM 
                    AssDivisao AS Divisao 
                    LEFT JOIN AssDivisao AS DivisaoPrincipal ON Divisao.AssDivisaoId = DivisaoPrincipal.Id
                    LEFT JOIN AssTipoPrescricao AS TipoPrescricao ON Divisao.AssTipoPrescricaoId = TipoPrescricao.Id
                WHERE 
                    Divisao.IsDeleted = @deleted AND Divisao.Id IN(@ids)";
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    return await connection.QueryAsync<DivisaoDto, DivisaoDto, TipoPrescricaoDto, DivisaoDto>(query,
                    (divisao, divisaoPrincipal, tipoPrescricao) =>
                    {
                        if (divisao == null)
                        {
                            return null;
                        }

                        divisao.DivisaoPrincipal = divisaoPrincipal;
                        divisao.TipoPrescricao = tipoPrescricao;
                        return divisao;
                    },
                    new { deleted = false, ids }).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [UnitOfWork(false)]
        public async Task<DivisaoDto> Obter(long id)
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                using (var tipoPrescricaoService = IocManager.Instance.ResolveAsDisposable<ITipoPrescricaoAppService>())
                {
                    var divisaoPrincipalDto = new DivisaoDto();
                    var m = await divisaoRepositorio.Object
                                .GetAll()
                                .Include(d => d.DivisaoPrincipal)
                                .Include(d => d.TipoPrescricao)
                                .Where(d => d.Id == id)
                                .FirstOrDefaultAsync().ConfigureAwait(false);

                    var dto = DivisaoDto.Mapear(m);

                    if (dto.TipoPrescricaoId.HasValue)
                    {
                        var tipoPrescricao = await tipoPrescricaoService.Object.Obter(dto.TipoPrescricaoId.Value).ConfigureAwait(false);
                        dto.TipoPrescricao = tipoPrescricao;
                    }

                    return dto;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IEnumerable<DivisaoDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "Divisao").GetFields()},
                        {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "DivisaoPrincipal").GetFields()},
                        {QueryHelper.CreateQueryFields<TipoPrescricao>(tableAlias: "TipoPrescricao").GetFields()}
                    FROM AssDivisao AS Divisao
                        LEFT JOIN  AssDivisao AS DivisaoPrincipal ON DivisaoPrincipal.AssDivisaoId = Divisao.Id
                        LEFT JOIN AssTipoPrescricao  AS TipoPrescricao ON TipoPrescricao.Id = Divisao.AssTipoPrescricaoId
                    WHERE 
                        Divisao.Id IN @ids
                        AND Divisao.IsDeleted = 0
                    ";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync(query, (Func<DivisaoDto, DivisaoDto, TipoPrescricaoDto, DivisaoDto>)DapperMapper, new { ids = ids.Distinct().ToList() });
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private static DivisaoDto DapperMapper(DivisaoDto divisao, DivisaoDto divisaoPrincipal, TipoPrescricaoDto tipoPrescricao)
        {
            if (divisao == null)
            {
                return null;
            }

            if (divisaoPrincipal != null)
            {
                divisao.DivisaoPrincipal = divisaoPrincipal;
            }

            if (tipoPrescricao != null)
            {
                divisao.TipoPrescricao = tipoPrescricao;
            }

            return divisao;
        }

        [UnitOfWork(false)]
        public async Task<DivisaoDto> Obter(string divisao)
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = divisaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!divisao.IsNullOrWhiteSpace(), d => d.Codigo.Contains(divisao) || d.Descricao.Contains(divisao));
                    var m = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                    var _divisao = DivisaoDto.Mapear(m);

                    return _divisao;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<DivisaoDto>> ListarTodos()
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = divisaoRepositorio.Object
                    .GetAll()
                    .Include(m => m.DivisaoPrincipal);

                    var divisoes = await query
                                       // .AsNoTracking()
                                       .ToListAsync().ConfigureAwait(false);

                    var divisoesDto = DivisaoDto.Mapear(divisoes).ToList();

                    return new ListResultDto<DivisaoDto>
                    {
                        Items = divisoesDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<DivisaoDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = divisaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                    var divisao = await query
                                      .AsNoTracking()
                                      .ToListAsync().ConfigureAwait(false);

                    var divisoesDto = DivisaoDto.Mapear(divisao).ToList();

                    return new ListResultDto<DivisaoDto>
                    {
                        Items = divisoesDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var divisaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Divisao, long>>())
                {
                    var query = (from p in divisaoRepositorio.Object.GetAll().AsNoTracking()
                             .Where(m => m.IsDivisaoPrincipal && m.Subdivisoes.Count() == 0)
                             .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.ToString().Contains(dropdownInput.search))
                                 orderby p.Ordem ascending //, p.Codigo ascending
                                 select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) })
                             .Union(from p in divisaoRepositorio.Object.GetAll().AsNoTracking()
                                    .Where(m => !m.IsDivisaoPrincipal && m.DivisaoPrincipalId.HasValue)
                                    .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.ToString().Contains(dropdownInput.search))
                                    orderby p.Ordem ascending //, p.Codigo ascending
                                    select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) });

                    var queryResultPage = query
                        .OrderBy(m => m.text)
                        .Skip(numberOfObjectsPerPage * pageInt)
                        .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public Task<FileDto> ListarParaExcel(ListarDivisoesInput input)
        {
            throw new NotImplementedException();
        }
    }
}
