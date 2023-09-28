using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturamentoItemAtendimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturamentoItemAtendimento.dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using Abp.EntityFramework.Repositories;
    using Abp.Threading;
    using Castle.Core.Internal;
    using Dapper;
    using RestSharp;
    //using RestSharp;
    using SW10.SWMANAGER.Authorization.Users;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.ModelosPrescricoes;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
    using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
    using SW10.SWMANAGER.Dto;
    using SW10.SWMANAGER.Helpers;
    using SW10.SWMANAGER.Organizations.Dto;
    using System.Configuration;
    using System.Data.SqlClient;
    using static SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.PrescricaoItemRespostaAppService;

    public class PrescricaoMedicaAppService : SWMANAGERAppServiceBase, IPrescricaoMedicaAppService
    {
        [UnitOfWork(IsDisabled = true)]
        public async Task AtualizaArquivoPrescricaoMedica(long prescricaoId)
        {
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
            using (var registroArquivoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var prescricao = await prescricaoMedicaRepository.Object.GetAll().AsNoTracking().Select(x => new {x.Id, x.AtendimentoId}).FirstOrDefaultAsync(x => x.Id == prescricaoId).ConfigureAwait(false);
                if (!prescricao.AtendimentoId.HasValue)
                {
                    return;
                }
                    
                var pdfBytes = prescricaoMedicaAppService.Object.RetornaArquivoPrescricaoMedica(prescricaoId, false);
                var registroArquivo = new RegistroArquivo
                {
                    Arquivo = pdfBytes,
                    RegistroTabelaId = (long) EnumArquivoTabela.PrescricaoMedica,
                    RegistroId = (long) (prescricao?.Id ?? 0),
                    AtendimentoId = prescricao?.AtendimentoId ?? 0
                };
                await registroArquivoRepository.Object.InsertAndGetIdAsync(registroArquivo).ConfigureAwait(false);
                unitOfWork.Complete();
                unitOfWork.Dispose();
            }
            
            // TODO: Ver os casos em q se encaixa fazer em background a criação da prescrição.
            // using(var backgroundJob = IocManager.Instance.ResolveAsDisposable<IBackgroundJobManager>())
            // {
            //     var args = new AtualizaArquivoPrescricaoMedicaJobArgs
            //     {
            //         PrescricaoMedicaId = prescricaoId,
            //         TenantId = this.GetCurrentTenant().Id
            //     };
            //     await backgroundJob.Object.EnqueueAsync<AtualizaArquivoPrescricaoMedicaJob,AtualizaArquivoPrescricaoMedicaJobArgs>(args, BackgroundJobPriority.High).ConfigureAwait(false);
            // }
        }

        public async Task<RetornoValidacaoDuplicidadeNaPrescricao> ValidaDuplicidadeItemNaPrescricao(long prescricaoMedicaId, long prescricaoItemId)
        {
            using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            {
                var items = await prescricaoItemRespostaRepository.Object.GetAll()
                    .Where(x => x.PrescricaoMedicaId == prescricaoMedicaId && x.PrescricaoItemId == prescricaoItemId)
                    .Include(x=> x.PrescricaoItem)
                    .ToListAsync();
                if (items.Any(c=> c.IsSuspenso))
                {
                    return new RetornoValidacaoDuplicidadeNaPrescricao(true,
                        $" Deseja incluir mesmo assim?",
                        $"O item {items.FirstOrDefault().PrescricaoItem.Descricao} já foi prescrito e suspenso.");
                }
                
                if (items.Any())
                {
                    return new RetornoValidacaoDuplicidadeNaPrescricao(true,
                        $"Deseja incluir mesmo assim?",
                        $"O item {items.FirstOrDefault().PrescricaoItem.Descricao} já foi prescrito.");
                }
                
                return new RetornoValidacaoDuplicidadeNaPrescricao();
            }
        }

        public byte[] RetornaArquivoPrescricaoMedica(long prescricaoId, bool imprimirResumido = false, DateTime? dataAgrupamento = null, int tentantId = 0)
        {
            var url = ConfigurationManager.AppSettings.Get("reportBaseUrl");
            var dominio = tentantId == 0? this.GetConnectionStringName(): this.GetConnectionStringName(tentantId);

            var client = new RestClient($"{url}");
            var request = new RestRequest("PrescricaoMedica", Method.POST);
            request
                .AddParameter("prescricaoMedicaId", prescricaoId)
                .AddParameter("imprimirResumido", imprimirResumido.ToString().ToLower())
                .AddParameter("Dominio", dominio);

            var whereDataAgrupamentoCondition = "WHERE 1=1";
            var dataAgrupamentoParam = "";
            if (dataAgrupamento.HasValue)
            {
                whereDataAgrupamentoCondition +=  $"AND DataAgrupamento  = '{dataAgrupamento.Value.ToString("yyyyMMdd HH:mm")}'";
                dataAgrupamentoParam = dataAgrupamento.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
            
            request.AddParameter("dataAgrupamento",dataAgrupamentoParam);
            request.AddParameter("whereDataAgrupamentoCondition",whereDataAgrupamentoCondition);

            var data = client.DownloadData(request);

            return data;
        }

        [UnitOfWork(IsDisabled = false)]
        public async Task<PrescricaoMedicaDto> CriarOuEditar(PrescricaoMedicaDto input, bool atualizaOuCriaArquivo = true)
        {
            try
            {
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prescricaoMedica = PrescricaoMedicaDto.Mapear(input);
                    
                    if (input.Id.Equals(0))
                    {
                        input.Id = await prescricaoMedicaRepository.Object.InsertAndGetIdAsync(prescricaoMedica).ConfigureAwait(false);
                    }
                    else
                    {
                        var entidade = await prescricaoMedicaRepository.Object.GetAsync(prescricaoMedica.Id).ConfigureAwait(false);
                        entidade.AtendimentoId = prescricaoMedica.AtendimentoId;
                        entidade.Codigo = prescricaoMedica.Codigo;
                        entidade.CreationTime = prescricaoMedica.CreationTime;
                        entidade.CreatorUserId = prescricaoMedica.CreatorUserId;
                        entidade.DataPrescricao = prescricaoMedica.DataPrescricao;
                        entidade.DeleterUserId = prescricaoMedica.DeleterUserId;
                        entidade.DeletionTime = prescricaoMedica.DeletionTime;
                        entidade.Descricao = prescricaoMedica.Descricao;
                        //entidade.Id = prescricaoMedica.Id;
                        entidade.Observacao = prescricaoMedica.Observacao;
                        entidade.PrescricaoStatusId = prescricaoMedica.PrescricaoStatusId;
                        //entidade.MedicoId = prescricaoMedica.MedicoId;
                        entidade.UnidadeOrganizacionalId = prescricaoMedica.UnidadeOrganizacionalId;
                        entidade.LeitoId = prescricaoMedica.LeitoId;

                        await prescricaoMedicaRepository.Object.UpdateAsync(entidade).ConfigureAwait(false);

                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }

                if (atualizaOuCriaArquivo)
                {
                    await this.AtualizaArquivoPrescricaoMedica(input.Id).ConfigureAwait(false);
                }

                return input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message + " - " + ex.InnerException?.Message);
                // throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var prescricaoMedicaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await prescricaoMedicaRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }


        [UnitOfWork]
        public async Task ExcluirItemResposta(long prescricaoItemRespostaId)
        {
            try
            {
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var item = await prescricaoItemRespostaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == prescricaoItemRespostaId).ConfigureAwait(false);

                    await prescricaoItemRespostaRepository.Object.DeleteAsync(prescricaoItemRespostaId).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    if (item != null && item.PrescricaoMedicaId.HasValue)
                    {
                        await this.AtualizaArquivoPrescricaoMedica(item.PrescricaoMedicaId.Value);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoMedicaDto>> ListarTodos()
        {
            try
            {
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                {
                    var query = prescricaoMedicaRepository.Object.GetAll().AsNoTracking();

                    var prescricoesMedicasDto = await query.ToListAsync().ConfigureAwait(false);

                    var result = PrescricaoMedicaDto.Mapear(prescricoesMedicasDto).ToList();

                    return new ListResultDto<PrescricaoMedicaDto> { Items = result };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoMedicaIndexDto>> Listar(PrescricaoMedicaListarInput input)
        {

            try
            {
                using(var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var modeloPrescricaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloPrescricao, long>>())
                {
                    var principalId = string.IsNullOrWhiteSpace(input.PrincipalId)
                                          ? 0
                                          : Convert.ToInt64(input.PrincipalId);

                    var queryModeloPrescricao = modeloPrescricaoRepository.Object.GetAll();

                    var query = prescricaoMedicaRepository.Object.GetAll()
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.Atendimento)
                        .Include(m => m.Atendimento.Paciente).Include(m => m.Atendimento.Paciente.SisPessoa)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa).Include(m => m.PrescricaoStatus)
                        .WhereIf(principalId > 0, m => m.AtendimentoId == principalId)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Observacao.Contains(input.Filtro))
                        .Where(w => !queryModeloPrescricao.Any(a => a.PrescricaoMedicaId == w.Id));
                    
                    var prescricoesMedicas =
                        await query.Select(m => new PrescricaoMedicaIndexDto
                        {
                            Codigo = m.Codigo,
                            DataPrescricao = m.DataPrescricao,
                            Id = m.Id,
                            Medico = m.Medico != null ? m.Medico.NomeCompleto : null,
                            Paciente = m.Atendimento != null && m.Atendimento.Paciente != null ? m.Atendimento.Paciente.NomeCompleto : null,
                            PrescricaoStatusId = m.PrescricaoStatusId,
                            Status = m.PrescricaoStatus != null ? m.PrescricaoStatus.Descricao : null,
                            StatusCor = m.PrescricaoStatus != null ? m.PrescricaoStatus.Cor : null
                        }).OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var contarPrescricoesMedicas = await query.CountAsync().ConfigureAwait(false);
                    var prescricoesMedicasIds = prescricoesMedicas.Select(pm => pm.Id).ToList();
                    var imprimeAcrescimosESuspensoesByprescricoesMedicasIds = prescricaoItemRespostaRepository.Object.GetAll().AsNoTracking()
                        .Where(x => x.PrescricaoMedicaId.HasValue &&
                                    prescricoesMedicasIds.Contains(x.PrescricaoMedicaId.Value))
                        .GroupBy(x=> x.PrescricaoMedicaId)
                        .Select(gp => new
                        {
                            PrescricaoMedicaId = gp.Key.Value,
                            ImprimeAcrescimosESuspensoes = gp.Any(x => x.IsAcrescimo || x.IsSuspenso)
                        }).ToList();

                    if (!imprimeAcrescimosESuspensoesByprescricoesMedicasIds.IsNullOrEmpty())
                    {
                        foreach (var prescricoesMedica in prescricoesMedicas)
                        {
                            var item = imprimeAcrescimosESuspensoesByprescricoesMedicasIds.FirstOrDefault(x => x.PrescricaoMedicaId == prescricoesMedica.Id);
                            if (item != null)
                            {
                                prescricoesMedica.ImprimeAcrescimosESuspensoes = item.ImprimeAcrescimosESuspensoes;
                            }
                        }
                    }
                    

                    return new PagedResultDto<PrescricaoMedicaIndexDto>(
                        contarPrescricoesMedicas,
                        prescricoesMedicas);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PrescricaoMedicaDto> Obter(long id)
        {
            try
            {
                var model = new PrescricaoMedicaDto();
                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    model = await sqlConnection.QueryFirstOrDefaultAsync<PrescricaoMedicaDto>($@"
                        SELECT {QueryHelper.CreateQueryFields<PrescricaoMedica>().GetFields()} FROM  AssPrescricaoMedica  WHERE AssPrescricaoMedica.Id = @id", new { id }).ConfigureAwait(false);
                }
                var respostas = await this.ListarRespostasPorPrescricao(model.Id).ConfigureAwait(false);
                var list = respostas.Items.OrderBy(m => m.IdGridPrescricaoItemResposta).ToList();
                return model;

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoMedicaDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                {
                    var query = prescricaoMedicaRepository.Object.GetAll();
                    //.WhereIf(!filtro.IsNullOrWhiteSpace(), m =>
                    //     m.CreationTime.ToShortTimeString().ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Medico.NomeCompleto.ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Medico.Cpf.ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Medico.Nascimento.ToShortTimeString().ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Medico.Rg.ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Paciente.NomeCompleto.ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Paciente.Cpf.ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Paciente.Nascimento.ToShortTimeString().ToUpper().Contains(filtro.ToUpper()) ||
                    //     m.Atendimento.Paciente.Rg.ToUpper().Contains(filtro.ToUpper())
                    //);

                    var prescricoesMedicas = await query.ToListAsync().ConfigureAwait(false);
                    var prescricoesMedicasDto = PrescricaoMedicaDto.Mapear(prescricoesMedicas).ToList();

                    return new ListResultDto<PrescricaoMedicaDto> { Items = prescricoesMedicasDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoItemRespostaDto>> ListarRespostas(ListarPrescricaoMedicaInput input)
        {

            try
            {
                using (var prescricaoItemRespostaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                {
                    long id = 0;
                    var isValid = long.TryParse(input.PrincipalId, out id);
                    var query = prescricaoItemRespostaRepository.Object.GetAll().AsNoTracking().Include(m => m.PrescricaoItem)
                        .Include(m => m.Unidade).Include(m => m.Frequencia).Include(m => m.Divisao)
                        .Include(m => m.FormaAplicacao).Include(m => m.Medico).Include(m => m.PrescricaoItemStatus)
                        .Include(m => m.PrescricaoMedica).Include(m => m.UnidadeOrganizacional)
                        .Include(m => m.VelocidadeInfusao).Where(m => m.PrescricaoMedicaId == id)
                        //.WhereIf(input.DivisaoId > 0, m => m.DivisaoId == input.DivisaoId)
                        .WhereIf(
                            input.StartDate.HasValue && input.EndDate.HasValue,
                            m => m.CreationTime.IsBetween(input.StartDate.Value, input.EndDate.Value)).WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contar = await query.CountAsync().ConfigureAwait(false);

                    var list = await query
                                   //.AsNoTracking()
                                   //.OrderBy(input.Sorting)
                                   //.PageBy(input)
                                   .ToListAsync().ConfigureAwait(false);

                    var listDto = new List<PrescricaoItemRespostaDto>();
                    var idGrid = 1;
                    foreach (var m in list)
                    {
                        var item = PrescricaoItemRespostaDto.Mapear(m);
                        item.IdGridPrescricaoItemResposta = idGrid++;
                        listDto.Add(item);
                    }

                    var result = listDto.AsQueryable().WhereIf(input.DivisaoId > 0, m => m.DivisaoId == input.DivisaoId)
                        .AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToList();

                    return new PagedResultDto<PrescricaoItemRespostaDto>(
                        contar,
                        result.OrderBy(m => m.IdGridPrescricaoItemResposta).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoItemRespostaDto>> ListarRespostasJson(List<PrescricaoItemRespostaDto> list, long divisaoId)
        {
            try
            {
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                using (var frequenciaAppService = IocManager.Instance.ResolveAsDisposable<IFrequenciaAppService>())
                using (var prescricaoItemHoraRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                {
                    var count = 0;
                    if (list == null)
                    {
                        list = new List<PrescricaoItemRespostaDto>();
                    }

                    var idGrid = 1;
                    for (var i = 0; i < list.Count(); i++)
                    {
                        list[i].PrescricaoItem = new PrescricaoItemDto();
                        list[i].Unidade = new UnidadeDto();
                        list[i].Frequencia = new FrequenciaDto();
                        list[i].IdGridPrescricaoItemResposta = idGrid++;

                        var prescricaoItem = await prescricaoItemAppService.Object.Obter(list[i].PrescricaoItemId.Value).ConfigureAwait(false);
                        list[i].PrescricaoItem.Descricao = prescricaoItem.Descricao;
                        list[i].PrescricaoItem.Codigo = prescricaoItem.Codigo;
                        if (list[i].UnidadeId.HasValue)
                        {
                            var unidade = await unidadeAppService.Object.Obter(list[i].UnidadeId.Value).ConfigureAwait(false);
                            list[i].Unidade.Descricao = unidade.Descricao;
                            list[i].Unidade.Codigo = unidade.Codigo;
                        }

                        if (list[i].FrequenciaId.HasValue)
                        {
                            var frequencia = await frequenciaAppService.Object.Obter(list[i].FrequenciaId.Value).ConfigureAwait(false);
                            list[i].Frequencia.Descricao = frequencia.Descricao;
                            list[i].Frequencia.Codigo = frequencia.Codigo;
                            var id = list[i].Id;
                            var horas = await prescricaoItemHoraRepositorio.Object.GetAll().Where(p => p.PrescricaoItemRespostaId == id).ToListAsync().ConfigureAwait(false);
                            var horarios = string.Empty;
                            foreach (var hora in horas)
                            {
                                horarios += string.Format("{0:00}:00 ", hora.Descricao);
                            }

                            if (horarios.Length > 0)
                            {
                                horarios = horarios.Substring(0, horarios.Length - 1);
                            }

                            list[i].Horarios = horarios;
                        }
                    }

                    var result = list.Where(m => m.DivisaoId == divisaoId).ToList();
                    count = list.Count();
                    return new PagedResultDto<PrescricaoItemRespostaDto>(count, result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public bool ValidarPrescricaoFutura(long atendimentoId)
        {
            using(var parametrizacoesAppService = IocManager.Instance.ResolveAsDisposable<IParametrizacoesAppService>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                var parametrizacao = parametrizacoesAppService.Object.GetParametrizacoesSync();
                var atendimentoIsInternacao = atendimentoRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Where(x => x.Id == atendimentoId)
                    .Select(x => x.IsInternacao).FirstOrDefault();

                if (parametrizacao?.PrescricaoMedicaHoraOutroDia != null)
                {
                    return DateTime.Now.TimeOfDay >= parametrizacao.PrescricaoMedicaHoraOutroDia && atendimentoIsInternacao;
                }
            }
            return false;
        }


        //Melhorar a busca.....urgente
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<List<PrescricaoItemRespostaDto>> ListarPrescricaoCompleta(List<PrescricaoItemRespostaDto> list)
        {
            var result = new List<PrescricaoItemRespostaDto>();
            try
            {
                using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
                using (var formaAplicacaoAppService = IocManager.Instance.ResolveAsDisposable<IFormaAplicacaoAppService>())
                using (var frequenciaAppService = IocManager.Instance.ResolveAsDisposable<IFrequenciaAppService>())
                using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var velocidadeInfusaoAppService = IocManager.Instance.ResolveAsDisposable<IVelocidadeInfusaoAppService>())
                {
                    foreach (var resposta in list)
                    {
                        if (resposta.DivisaoId.HasValue)
                        {
                            resposta.Divisao = await divisaoAppService.Object.Obter(resposta.DivisaoId.Value)
                                                   .ConfigureAwait(false);
                        }

                        if (resposta.FormaAplicacaoId.HasValue)
                        {
                            resposta.FormaAplicacao = await formaAplicacaoAppService.Object
                                                          .Obter(resposta.FormaAplicacaoId.Value).ConfigureAwait(false);
                        }

                        if (resposta.FrequenciaId.HasValue)
                        {
                            resposta.Frequencia = await frequenciaAppService.Object.Obter(resposta.FrequenciaId.Value)
                                                      .ConfigureAwait(false);
                        }

                        if (resposta.MedicoId.HasValue)
                        {
                            resposta.Medico =
                                await medicoAppService.Object.Obter(resposta.MedicoId.Value).ConfigureAwait(false);
                        }

                        resposta.PrescricaoItem = await prescricaoItemAppService.Object.Obter(resposta.PrescricaoItemId.Value)
                                                      .ConfigureAwait(false);

                        if (resposta.UnidadeId.HasValue)
                        {
                            resposta.Unidade = await unidadeAppService.Object.Obter(resposta.UnidadeId.Value)
                                                   .ConfigureAwait(false);
                        }

                        if (resposta.UnidadeOrganizacionalId.HasValue)
                        {
                            resposta.UnidadeOrganizacional =
                                await unidadeOrganizacionalAppService.Object
                                    .ObterPorId(resposta.UnidadeOrganizacionalId.Value).ConfigureAwait(false);
                        }

                        if (resposta.VelocidadeInfusaoId.HasValue)
                        {
                            resposta.VelocidadeInfusao = await velocidadeInfusaoAppService.Object
                                                             .Obter(resposta.VelocidadeInfusaoId.Value)
                                                             .ConfigureAwait(false);
                        }

                        result.Add(resposta);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroListar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            {
                return await this.CreateSelect2(prescricaoItemRespostaRepository.Object).ExecuteAsync(dropdownInput)
                           .ConfigureAwait(false);
            }
        }


        public async Task<ListResultDto<PrescricaoItemRespostaDto>> ListarRespostasPorPrescricao(long id)
        {
            try
            {
                var query = $@"
                    SELECT
                        {QueryHelper.CreateQueryFields<PrescricaoItemResposta>(tableAlias: "PrescricaoItemResposta").IgnoreField(x => x.DiaAtual).GetFields()},
                        {QueryHelper.CreateQueryFields<PrescricaoItem>(tableAlias: "DiluenteItemResposta").GetFields()},
                        -- Divisão
                        {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "DivisaoItemResposta").GetFields()},
                        {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "DivisaoPrincipalItemResposta").GetFields()},
                        {QueryHelper.CreateQueryFields<TipoPrescricao>(tableAlias: "TipoPrescricaoItemResposta").GetFields()},
                        -- FormaAplicacao
                        {QueryHelper.CreateQueryFields<FormaAplicacao>(tableAlias: "FormaAplicacaoItemResposta").GetFields()},
                        -- Frequencia
                        {QueryHelper.CreateQueryFields<Frequencia>(tableAlias: "FrequenciaItemResposta").GetFields()},
                        -- Medico
                        {QueryHelper.CreateQueryFields<Medico>(tableAlias: "MedicoItemResposta")
                        .IgnoreField("Item")
                        .IgnoreFields(
                                x => x.NomeCompleto, x => x.Nascimento,
                                x => x.SexoId, x => x.Sexo,
                                x => x.CorPele, x => x.CorPeleId,
                                x => x.Profissao, x => x.ProfissaoId,
                                x => x.Escolaridade, x => x.EscolaridadeId,
                                x => x.Rg, x => x.Emissao, x => x.Cpf,
                                x => x.Nacionalidade, x => x.NacionalidadeId,
                                x => x.EstadoCivil, x => x.EstadoCivilId,
                                x => x.NomeMae, x => x.NomePai,
                                x => x.Religiao, x => x.ReligiaoId,
                                x => x.Foto, x => x.FotoMimeType,
                                x => x.Email, x => x.Email2, x => x.Email3, x => x.Email4,
                                x => x.Telefone1, x => x.TipoTelefone1Id, x => x.TipoTelefone1, x => x.DddTelefone1,
                                x => x.Telefone2, x => x.TipoTelefone2Id, x => x.TipoTelefone2, x => x.DddTelefone2,
                                x => x.Telefone3, x => x.TipoTelefone3Id, x => x.TipoTelefone3, x => x.DddTelefone3,
                                x => x.Telefone4, x => x.TipoTelefone4Id, x => x.TipoTelefone4, x => x.DddTelefone4,
                                x => x.Cep, x => x.Cidade, x => x.CidadeId, x => x.Complemento, x => x.EstadoId, x => x.EstadoId,
                                x => x.Pais, x => x.PaisId, x => x.Logradouro, x => x.Numero,
                                x => x.TipoLogradouro, x => x.TipoLogradouroId, x => x.Bairro
                                ).GetFields()},
                        {QueryHelper.CreateQueryFields<SisPessoa>(tableAlias: "SisPessoaMedicoItemResposta").IgnoreField(x => x.Descricao).GetFields()},
                        -- PrescricaoItem
                        {QueryHelper.CreateQueryFields<PrescricaoItem>(tableAlias: "PrescricaoItem").GetFields()},
                        {QueryHelper.CreateQueryFields<TipoPrescricao>(tableAlias: "TipoPrescricao").GetFields()},
                        {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "Divisao").GetFields()},
                        {QueryHelper.CreateQueryFields<FormaAplicacao>(tableAlias: "FormaAplicacao").GetFields()},
                        {QueryHelper.CreateQueryFields<Frequencia>(tableAlias: "Frequencia").GetFields()},
                        {QueryHelper.CreateQueryFields<TipoControle>(tableAlias: "TipoControle").GetFields()},
                        {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "Unidade").GetFields()},
                        {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "UnidadeRequisicao").GetFields()},
                        {QueryHelper.CreateQueryFields<VelocidadeInfusao>(tableAlias: "VelocidadeInfusao").GetFields()},
                        {QueryHelper.CreateQueryFields<Produto>(tableAlias: "Produto").GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoItem>(tableAlias: "FaturamentoItem").GetFields()},
                        {QueryHelper.CreateQueryFields<Estoque>(tableAlias: "Estoque").GetFields()},
                        -- PrescricaoStatus
                        {QueryHelper.CreateQueryFields<PrescricaoStatus>(tableAlias: "PrescricaoItemStatusItemResposta").GetFields()},
                        -- Unidade
                        {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "UnidadeItemResposta").GetFields()},
                        -- UnidadeOrganizacional
                        {QueryHelper.CreateQueryFields<UnidadeOrganizacional>(tableAlias: "UnidadeOrganizacionalItemResposta").IgnoreField(x => x.OrganizationUnit).GetFields()},
                        {QueryHelper.CreateQueryFields<Abp.Organizations.OrganizationUnit>(tableAlias: "OrganizationUnitItemResposta").IgnoreField(x => x.Parent).GetFields()},
                        {QueryHelper.CreateQueryFields<UnidadeInternacaoTipo>(tableAlias: "UnidadeInternacaoTipoItemResposta").GetFields()},
                         -- VelocidadeInfusao
                        {QueryHelper.CreateQueryFields<VelocidadeInfusao>(tableAlias: "VelocidadeInfusaoItemResposta").GetFields()}
                    FROM 
                         AssPrescricaoItemResposta AS PrescricaoItemResposta
                        LEFT JOIN AssPrescricaoItem AS DiluenteItemResposta ON PrescricaoItemResposta.DiluenteId = DiluenteItemResposta.Id
                        
                        -- Divisão
                        LEFT JOIN AssDivisao AS DivisaoItemResposta ON PrescricaoItemResposta.AssDivisaoId = DivisaoItemResposta.Id
                        LEFT JOIN  AssDivisao AS DivisaoPrincipalItemResposta ON DivisaoPrincipalItemResposta.AssDivisaoId = DivisaoItemResposta.Id
                        LEFT JOIN AssTipoPrescricao AS TipoPrescricaoItemResposta ON TipoPrescricaoItemResposta.Id = DivisaoItemResposta.AssTipoPrescricaoId
                        
                        -- FormaAplicacao
                        LEFT JOIN AssFormaAplicacao AS FormaAplicacaoItemResposta ON FormaAplicacaoItemResposta.Id = PrescricaoItemResposta.AssFormaAplicacaoId
                        
                        --  Frequencia
                        LEFT JOIN AssFrequencia AS FrequenciaItemResposta ON PrescricaoItemResposta.AssFrequenciaId = FrequenciaItemResposta.Id

                        -- Medico
                        LEFT JOIN SisMedico AS MedicoItemResposta ON PrescricaoItemResposta.SisMedicoId = MedicoItemResposta.Id
                        LEFT JOIN SisPessoa AS SisPessoaMedicoItemResposta  ON MedicoItemResposta.SisPessoaId = SisPessoaMedicoItemResposta.Id
                        -- PrescricaoItem
                        LEFT JOIN AssPrescricaoItem AS PrescricaoItem ON PrescricaoItemResposta.AssPrescricaoItemId = PrescricaoItem.Id
                        LEFT JOIN AssTipoPrescricao AS TipoPrescricao ON  PrescricaoItem.AssTipoPrescricaoId = TipoPrescricao.Id
                        LEFT JOIN AssDivisao AS Divisao ON  PrescricaoItem.AssDivisaoId = Divisao.Id
                        LEFT JOIN AssFormaAplicacao AS FormaAplicacao ON  PrescricaoItem.AssFormaAplicacaoId = FormaAplicacao.Id
                        LEFT JOIN AssFrequencia AS Frequencia ON  PrescricaoItem.AssFrequenciaId = Frequencia.Id
                        LEFT JOIN AssTipoControle AS TipoControle ON  PrescricaoItem.AssTipoControleId = TipoControle.Id
                        LEFT JOIN Est_Unidade AS Unidade ON  PrescricaoItem.EstUnidadeId = Unidade.Id
                        LEFT JOIN Est_Unidade AS UnidadeRequisicao ON  PrescricaoItem.EstUnidadeRequisicaoId = UnidadeRequisicao.Id
                        LEFT JOIN AssVelocidadeInfusao AS VelocidadeInfusao ON  PrescricaoItem.AssVelocidadeInfusaoId = VelocidadeInfusao.Id
                        LEFT JOIN Est_Produto AS Produto ON  PrescricaoItem.EstProdutoId = Produto.Id
                        LEFT JOIN FatItem AS FaturamentoItem ON  PrescricaoItem.FatItemId = FaturamentoItem.Id
                        LEFT JOIN Est_Estoque AS Estoque ON  PrescricaoItem.EstEstoqueId = Estoque.Id
                        
                        -- PrescricaoItemStatus
                        LEFT JOIN AssPrescricaoStatus AS PrescricaoItemStatusItemResposta ON  PrescricaoItemResposta.AssPrescricaoItemStatusId = PrescricaoItemStatusItemResposta.Id
                        
                        -- Unidade
                        LEFT JOIN Est_Unidade AS UnidadeItemResposta ON PrescricaoItemResposta.EstUnidadeId = UnidadeItemResposta.Id

                        -- UnidadeOrganizacional
                        LEFT JOIN SisUnidadeOrganizacional AS UnidadeOrganizacionalItemResposta ON PrescricaoItemResposta.SisUnidadeOrganizacionalId = UnidadeOrganizacionalItemResposta.Id
                        LEFT JOIN AbpOrganizationUnits as OrganizationUnitItemResposta ON UnidadeOrganizacionalItemResposta.SisOrganizationUnitId = OrganizationUnitItemResposta.Id
                        LEFT JOIN AteUnidadeInternacaoTipo as UnidadeInternacaoTipoItemResposta ON UnidadeOrganizacionalItemResposta.AteUnidadeInternacaoTipoId = UnidadeInternacaoTipoItemResposta.Id

                        -- VelocidadeInfusaoItemResposta
                        LEFT JOIN AssVelocidadeInfusao AS VelocidadeInfusaoItemResposta ON PrescricaoItemResposta.AssVelocidadeInfusaoId = VelocidadeInfusaoItemResposta.Id
                     WHERE
                        PrescricaoItemResposta.AssPrescricaoMedicaId = @id
                        AND PrescricaoItemResposta.IsDeleted = 0;
                ";

                var prescricaoItemHoraQuery = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<PrescricaoItemHora>(tableAlias: "PrescricaoItemHora").GetFields()} 
                    FROM 
                        AssPrescricaoItemHora AS PrescricaoItemHora 
                    WHERE 
                        PrescricaoItemHora.AssPrescricaoItemRespostaId IN @ids AND PrescricaoItemHora.IsDeleted = 0";

                var types = new Type[]
                {
                    typeof(PrescricaoItemRespostaDto), // 0
                    typeof(PrescricaoItemDto), // 1
                    typeof(DivisaoDto), // 2
                    typeof(DivisaoDto), // 3
                    typeof(TipoPrescricaoDto), // 4
                    typeof(FormaAplicacaoDto), // 5
                    typeof(FrequenciaDto), // 6
                    typeof(MedicoDto), // 7
                    typeof(SisPessoaDto), // 8
                    typeof(PrescricaoItemDto), // 9
                    typeof(TipoPrescricaoDto), // 10
                    typeof(DivisaoDto), // 11
                    typeof(FormaAplicacaoDto), // 12
                    typeof(FrequenciaDto), // 13
                    typeof(TipoControleDto), // 14
                    typeof(UnidadeDto), // 15
                    typeof(UnidadeDto), // 16
                    typeof(VelocidadeInfusaoDto), // 17
                    typeof(ProdutoDto), // 18
                    typeof(FaturamentoItemDto), // 19
                    typeof(EstoqueDto),  // 20
                    typeof(PrescricaoStatusDto),  // 21
                    typeof(UnidadeDto),  // 22
                    typeof(UnidadeOrganizacionalDto),  // 23
                    typeof(OrganizationUnitDto),  // 24
                    typeof(UnidadeInternacaoTipoDto),  // 25
                    typeof(VelocidadeInfusaoDto)  // 26
                };
                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    var result = await sqlConnection.QueryAsync(query, types, DapperMapper, new { id }).ConfigureAwait(false);

                    var prescricaoItemHoraList = await sqlConnection.QueryAsync<PrescricaoItemHoraDto>(prescricaoItemHoraQuery, new { ids = result.Select(x => x.Id).ToList() }).ConfigureAwait(false);
                    var gridIdex = 0;

                    foreach (var item in result)
                    {
                        item.IdGridPrescricaoItemResposta = gridIdex;

                        var prescricaoItemHoras = prescricaoItemHoraList.Where(x => x.PrescricaoItemRespostaId == item.Id);
                        var horarios = string.Empty;

                        foreach (var hora in prescricaoItemHoras)
                        {
                            horarios += string.Format("{0:00}:00 ", hora.Descricao);
                        }

                        if (horarios.Length > 0)
                        {
                            horarios = horarios.Substring(0, horarios.Length - 1);
                        }

                        item.Horarios = horarios;

                        gridIdex++;
                    }

                    return new ListResultDto<PrescricaoItemRespostaDto>(result.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IEnumerable<PrescricaoItemRespostaViewModel>> ListarRespostasPorPrescricaoCompleta(long id)
        {
            try
            {
                var query = $@"
                    SELECT
                        {QueryHelper.CreateQueryFields<PrescricaoItemResposta>(tableAlias: "PrescricaoItemResposta").IgnoreField(x => x.DiaAtual).GetFields()},
                        {QueryHelper.CreateQueryFields<PrescricaoItem>(tableAlias: "DiluenteItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.DiluenteDescricao).IgnoreId().GetFields()},
                        -- Divisão
                        {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "DivisaoItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x=> x.Descricao, x=> x.DivisaoDescricao).IgnoreId().GetFields()},
                        -- FormaAplicacao
                        {QueryHelper.CreateQueryFields<FormaAplicacao>(tableAlias: "FormaAplicacaoItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.FormaAplicacaoDescricao).IgnoreId().GetFields()},
                        -- Frequencia
                        {QueryHelper.CreateQueryFields<Frequencia>(tableAlias: "FrequenciaItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.FrequenciaDescricao).IgnoreId().GetFields()},
                        -- PrescricaoItem
                        {QueryHelper.CreateQueryFields<PrescricaoItem>(tableAlias: "PrescricaoItem").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.PrescricaoItemDescricao).IgnoreId().GetFields()},
                        -- PrescricaoStatus
                        {QueryHelper.CreateQueryFields<PrescricaoStatus>(tableAlias: "PrescricaoItemStatusItemResposta")
                        .AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.PrescricaoItemStatusDescricao).IgnoreId().GetFields()},
                        {QueryHelper.CreateQueryFields<PrescricaoStatus>(tableAlias: "PrescricaoItemStatusItemResposta")
                        .AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Cor, x => x.PrescricaoItemStatusCor).IgnoreId().GetFields()},
                        -- Unidade
                        {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "UnidadeItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Sigla, x => x.UnidadeSigla).IgnoreId().GetFields()},
                        -- UnidadeOrganizacional
                        {QueryHelper.CreateQueryFields<UnidadeOrganizacional>(tableAlias: "UnidadeOrganizacionalItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.UnidadeOrganizacionalDescricao).IgnoreId().GetFields()},
                         -- VelocidadeInfusao
                        {QueryHelper.CreateQueryFields<VelocidadeInfusao>(tableAlias: "VelocidadeInfusaoItemResposta").AllowFieldAndMap<PrescricaoItemRespostaViewModel>(x => x.Descricao, x => x.VelocidadeInfusaoDescricao).IgnoreId().GetFields()}
                    FROM 
                         AssPrescricaoItemResposta AS PrescricaoItemResposta
                        LEFT JOIN AssPrescricaoItem AS DiluenteItemResposta ON PrescricaoItemResposta.DiluenteId = DiluenteItemResposta.Id
                        
                        -- Divisão
                        LEFT JOIN AssDivisao AS DivisaoItemResposta ON PrescricaoItemResposta.AssDivisaoId = DivisaoItemResposta.Id
                        
                        -- FormaAplicacao
                        LEFT JOIN AssFormaAplicacao AS FormaAplicacaoItemResposta ON FormaAplicacaoItemResposta.Id = PrescricaoItemResposta.AssFormaAplicacaoId
                        
                        --  Frequencia
                        LEFT JOIN AssFrequencia AS FrequenciaItemResposta ON PrescricaoItemResposta.AssFrequenciaId = FrequenciaItemResposta.Id

                        -- Medico
                        LEFT JOIN SisMedico AS MedicoItemResposta ON PrescricaoItemResposta.SisMedicoId = MedicoItemResposta.Id
                        LEFT JOIN SisPessoa AS SisPessoaMedicoItemResposta  ON MedicoItemResposta.SisPessoaId = SisPessoaMedicoItemResposta.Id
                        -- PrescricaoItem
                        LEFT JOIN AssPrescricaoItem AS PrescricaoItem ON PrescricaoItemResposta.AssPrescricaoItemId = PrescricaoItem.Id
                        
                        -- PrescricaoItemStatus
                        LEFT JOIN AssPrescricaoStatus AS PrescricaoItemStatusItemResposta ON  PrescricaoItemResposta.AssPrescricaoItemStatusId = PrescricaoItemStatusItemResposta.Id
                        
                        -- Unidade
                        LEFT JOIN Est_Unidade AS UnidadeItemResposta ON PrescricaoItemResposta.EstUnidadeId = UnidadeItemResposta.Id

                        -- UnidadeOrganizacional
                        LEFT JOIN SisUnidadeOrganizacional AS UnidadeOrganizacionalItemResposta ON PrescricaoItemResposta.SisUnidadeOrganizacionalId = UnidadeOrganizacionalItemResposta.Id

                        -- VelocidadeInfusaoItemResposta
                        LEFT JOIN AssVelocidadeInfusao AS VelocidadeInfusaoItemResposta ON PrescricaoItemResposta.AssVelocidadeInfusaoId = VelocidadeInfusaoItemResposta.Id
                     WHERE
                        PrescricaoItemResposta.AssPrescricaoMedicaId = @id
                        AND PrescricaoItemResposta.IsDeleted = 0;
                ";

                var prescricaoItemHoraQuery = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<PrescricaoItemHora>(tableAlias: "PrescricaoItemHora").GetFields()} 
                    FROM 
                        AssPrescricaoItemHora AS PrescricaoItemHora 
                    WHERE 
                        PrescricaoItemHora.AssPrescricaoItemRespostaId IN @ids AND PrescricaoItemHora.IsDeleted = 0";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    var result = await sqlConnection.QueryAsync<PrescricaoItemRespostaViewModel>(query, new { id }).ConfigureAwait(false);

                    var prescricaoItemHoraList = await sqlConnection.QueryAsync<PrescricaoItemHoraDto>(prescricaoItemHoraQuery, new { ids = result.Select(x => x.Id).ToList() }).ConfigureAwait(false);
                    var gridIdex = 0;

                    foreach (var item in result)
                    {
                        item.IdGridPrescricaoItemResposta = gridIdex;

                        var prescricaoItemHoras = prescricaoItemHoraList.Where(x => x.PrescricaoItemRespostaId == item.Id);
                        var horarios = string.Empty;

                        foreach (var hora in prescricaoItemHoras)
                        {
                            horarios += string.Format("{0:00}:00 ", hora.Descricao);
                        }

                        if (horarios.Length > 0)
                        {
                            horarios = horarios.Substring(0, horarios.Length - 1);
                        }

                        item.Horarios = horarios;

                        gridIdex++;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private static PrescricaoItemRespostaDto DapperMapper(object[] result)
        {
            if (result.IsNullOrEmpty() || result[0] == null)
            {
                return null;
            }

            var item = result[0] as PrescricaoItemRespostaDto;

            item.Diluente = result[1] as PrescricaoItemDto;

            item.Divisao = result[2] as DivisaoDto;

            if (item.Divisao != null)
            {
                item.Divisao.DivisaoPrincipal = result[3] as DivisaoDto;
                item.Divisao.TipoPrescricao = result[4] as TipoPrescricaoDto;
            }

            item.FormaAplicacao = result[5] as FormaAplicacaoDto;

            item.Frequencia = result[6] as FrequenciaDto;


            item.Medico = result[7] as MedicoDto;

            if (item.Medico != null)
            {
                item.Medico.SisPessoa = result[8] as SisPessoaDto;
            }

            item.PrescricaoItem = result[9] as PrescricaoItemDto;

            if (item.PrescricaoItem != null)
            {
                item.PrescricaoItem.TipoPrescricao = result[10] as TipoPrescricaoDto;
                item.PrescricaoItem.Divisao = result[11] as DivisaoDto;
                item.PrescricaoItem.FormaAplicacao = result[12] as FormaAplicacaoDto;
                item.PrescricaoItem.Frequencia = result[13] as FrequenciaDto;
                item.PrescricaoItem.TipoControle = result[14] as TipoControleDto;
                item.PrescricaoItem.Unidade = result[15] as UnidadeDto;
                item.PrescricaoItem.UnidadeRequisicao = result[16] as UnidadeDto;
                item.PrescricaoItem.VelocidadeInfusao = result[17] as VelocidadeInfusaoDto;
                item.PrescricaoItem.Produto = result[18] as ProdutoDto;
                item.PrescricaoItem.FaturamentoItem = result[19] as FaturamentoItemDto;
                item.PrescricaoItem.Estoque = result[20] as EstoqueDto;
            }

            item.PrescricaoItemStatus = result[21] as PrescricaoStatusDto;
            item.Unidade = result[22] as UnidadeDto;

            item.UnidadeOrganizacional = result[23] as UnidadeOrganizacionalDto;

            if (item.UnidadeOrganizacional != null)
            {
                item.UnidadeOrganizacional.OrganizationUnit = result[24] as OrganizationUnitDto;
                item.UnidadeOrganizacional.UnidadeInternacaoTipo = result[25] as UnidadeInternacaoTipoDto;
            }

            item.VelocidadeInfusao = result[26] as VelocidadeInfusaoDto;
            return item;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoMedicaDto>> ListarPorPaciente(ListarInput input)
        {
            var contarPrescricoesMedicas = 0;
            long pacienteId = 0;
            List<PrescricaoMedicaDto> prescricoesMedicasDto = new List<PrescricaoMedicaDto>();
            try
            {
                var test = long.TryParse(input.PrincipalId, out pacienteId);
                if (!test)
                {
                    throw new UserFriendlyException(("SelecionePaciente"));
                }

                using (var _prescricaoMedicaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                {


                    //pacienteId = string.IsNullOrWhiteSpace(input.PrincipalId) ? 0 : Convert.ToInt64(input.PrincipalId);
                    var query = _prescricaoMedicaRepository.Object.GetAll().AsNoTracking()
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.Atendimento)
                        .Include(m => m.Atendimento.Paciente).Include(m => m.Atendimento.Paciente.SisPessoa)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa).Where(m => m.Atendimento.PacienteId == pacienteId)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Observacao.Contains(input.Filtro));

                    var prescricoesMedicas =
                        await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    contarPrescricoesMedicas = await query.CountAsync().ConfigureAwait(false);
                    var resultado = PrescricaoMedicaDto.Mapear(prescricoesMedicas).ToList();
                    prescricoesMedicasDto = resultado;

                    return new PagedResultDto<PrescricaoMedicaDto>(contarPrescricoesMedicas, prescricoesMedicasDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        private async Task<IEnumerable<PrescricaoItemRespostaDto>> ListarPrescricaoItemRespostaParaCopiarDapper(long id)
        {

            var query = $@"
                SELECT
                    {QueryHelper.CreateQueryFields<PrescricaoItemResposta>().TableAlias("AssPrescricaoItemResposta").IgnoreField(x => x.DiaAtual).GetFields()},
                    {QueryHelper.CreateQueryFields<Divisao>().TableAlias("AssDivisao").GetFields()},
                    {QueryHelper.CreateQueryFields<PrescricaoItem>().TableAlias("AssPrescricaoItem").GetFields()},
                    {QueryHelper.CreateQueryFields<FormaAplicacao>().TableAlias("AssFormaAplicacao").GetFields()},
                    {QueryHelper.CreateQueryFields<Frequencia>().TableAlias("AssFrequencia").GetFields()},
                    {QueryHelper.CreateQueryFields<Medico>().TableAlias("SisMedico").IgnoreField("Item").GetFields()},
                    {QueryHelper.CreateQueryFields<SisPessoa>().TableAlias("SisPessoa").GetFields()},
                    {QueryHelper.CreateQueryFields<PrescricaoItemStatus>().TableAlias("AssPrescricaoItemStatus").GetFields()},
                    {QueryHelper.CreateQueryFields<PrescricaoMedica>().TableAlias("AssPrescricaoMedica").GetFields()},
                    {QueryHelper.CreateQueryFields<Unidade>().TableAlias("Est_Unidade").GetFields()},
                    {QueryHelper.CreateQueryFields<UnidadeOrganizacional>().TableAlias("SisUnidadeOrganizacional").IgnoreField(x => x.OrganizationUnit).GetFields()},
                    {QueryHelper.CreateQueryFields<VelocidadeInfusao>().TableAlias("AssVelocidadeInfusao").GetFields()}
                FROM AssPrescricaoItemResposta
                    LEFT JOIN AssDivisao ON AssPrescricaoItemResposta.AssDivisaoId = AssDivisao.Id
                    LEFT JOIN AssPrescricaoItem ON AssPrescricaoItemResposta.AssPrescricaoItemId = AssPrescricaoItem.Id
                    LEFT JOIN AssFormaAplicacao ON AssPrescricaoItemResposta.AssFormaAplicacaoId = AssFormaAplicacao.Id
                    LEFT JOIN AssFrequencia ON AssPrescricaoItemResposta.AssFrequenciaId = AssFrequencia.Id
                    LEFT JOIN SisMedico ON AssPrescricaoItemResposta.SisMedicoId = SisMedico.Id
                    LEFT JOIN SisPessoa ON SisMedico.SisPessoaId = SisPessoa.Id 
                    LEFT JOIN AssPrescricaoItemStatus ON AssPrescricaoItemResposta.AssPrescricaoItemStatusId = AssPrescricaoItemStatus.Id
                    LEFT JOIN AssPrescricaoMedica ON AssPrescricaoItemResposta.AssPrescricaoMedicaId = AssPrescricaoMedica.Id
                    LEFT JOIN Est_Unidade ON AssPrescricaoItemResposta.EstUnidadeId = Est_Unidade.Id
                    LEFT JOIN SisUnidadeOrganizacional ON AssPrescricaoItemResposta.SisUnidadeOrganizacionalId = SisUnidadeOrganizacional.Id 
                    LEFT JOIN AssVelocidadeInfusao ON AssPrescricaoItemResposta.AssVelocidadeInfusaoId = AssVelocidadeInfusao.Id
                WHERE
                    AssPrescricaoItemResposta.IsDeleted = @False
                    AND AssPrescricaoItemResposta.AssPrescricaoMedicaId = @prescricaoMedicaId
                    AND AssDivisao.IsCopiarPrescricao =@isCopiarPrescricao 
                    AND AssPrescricaoItemResposta.AssPrescricaoItemStatusId NOT IN @prescricaoItemStatusId 
                    AND AssPrescricaoItemResposta.IsSuspenso = @False 
                    AND AssPrescricaoItemResposta.DoseUnica = @False";
            try
            {
                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    return await connection.QueryAsync(
                        query,
                        new[]
                        {
                            typeof(PrescricaoItemRespostaDto), typeof(DivisaoDto), typeof(PrescricaoItemDto),
                            typeof(FormaAplicacaoDto), typeof(FrequenciaDto), typeof(MedicoDto),
                            typeof(SisPessoaDto), typeof(PrescricaoItemStatusDto), typeof(PrescricaoMedicaDto),
                            typeof(UnidadeDto), typeof(UnidadeOrganizacionalDto), typeof(VelocidadeInfusaoDto)
                        },
                        PrescricaoItemRespostaQueryMapping,
                        new
                        {
                            False = false,
                            prescricaoMedicaId = id,
                            isCopiarPrescricao = true,
                            prescricaoItemStatusId = new long[] {PrescricaoStatus.Cancelada, PrescricaoStatus.Suspensa}
                        }
                    ).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        private static PrescricaoItemRespostaDto PrescricaoItemRespostaQueryMapping(object[] queryResult)
        {
            if (queryResult == null || queryResult.IsNullOrEmpty())
            {
                return null;
            }

            var result = queryResult[0] as PrescricaoItemRespostaDto;

            if (result == null)
            {
                return null;
            }

            result.Divisao = queryResult[1] as DivisaoDto;
            result.PrescricaoItem = queryResult[2] as PrescricaoItemDto;
            result.FormaAplicacao = queryResult[3] as FormaAplicacaoDto;
            result.Frequencia = queryResult[4] as FrequenciaDto;
            result.Medico = queryResult[5] as MedicoDto;
            if (result.Medico != null)
            {
                result.Medico.SisPessoa = queryResult[6] as SisPessoaDto;
            }

            result.PrescricaoItemStatus = queryResult[7] as PrescricaoStatusDto;
            result.PrescricaoMedica = queryResult[8] as PrescricaoMedicaDto;
            result.Unidade = queryResult[9] as UnidadeDto;
            result.UnidadeOrganizacional = queryResult[10] as UnidadeOrganizacionalDto;
            result.VelocidadeInfusao = queryResult[11] as VelocidadeInfusaoDto;

            return result;
        }


        private async Task<IEnumerable<PrescricaoItemHoraDto>> ListarPrescricaoItemRespostasHoras(List<long> prescricaoItemRespostaIds)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var query = @"
                    SELECT 
                        AssPrescricaoItemHora.*,
                        SisHoraDia.* 
                    FROM
                        AssPrescricaoItemHora 
                        LEFT JOIN SisHoraDia ON AssPrescricaoItemHora.SisHoraDiaId = SisHoraDia.Id 
                    WHERE AssPrescricaoItemHora.AssPrescricaoItemRespostaId IN @prescricaoItemRespostaIds AND AssPrescricaoItemHora.IsDeleted = 0";

                return await connection.QueryAsync<PrescricaoItemHoraDto, HoraDiaDto, PrescricaoItemHoraDto>(query, (prescricaoItemHoraDto, horaDiaDto) =>
                {
                    if (prescricaoItemHoraDto == null)
                    {
                        return null;
                    }

                    prescricaoItemHoraDto.HoraDia = horaDiaDto;
                    return prescricaoItemHoraDto;
                }, new { prescricaoItemRespostaIds }).ConfigureAwait(false);
            }
        }


        [DisableAuditing]
        [UnitOfWork(IsDisabled = false)]
        public async Task<RetornoCopiarPrescricaoMedica> ListarRespostasParaCopiar(long id)
        {
            var retorno = new RetornoCopiarPrescricaoMedica();
            try
            {
                using (var prescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var prescricaoItemHoraRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var frequenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                using (var horaDiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<HoraDia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var prescricaoItemRespostaDepsResolver = new PrescricaoItemRespostaDepsResolver(
                       prescricaoItemRepository,
                       prescricaoMedicaRepository,
                       prescricaoItemAppService,
                       prescricaoItemRespostaRepository,
                       prescricaoItemHoraRepository,
                       unidadeOrganizacionalAppService,
                       frequenciaRepository,
                       horaDiaRepository,
                       unitOfWorkManager
                   );

                    var prescricaoItemRespostas = await this.ListarPrescricaoItemRespostaParaCopiarDapper(id).ConfigureAwait(false);
                    var prescricaoItemRespostasHoras = await this.ListarPrescricaoItemRespostasHoras(prescricaoItemRespostas.Select(x => x.Id).ToList());
                    foreach (var prescricaoItemResposta in prescricaoItemRespostas)
                    {
                        var prescricaoItemHoras = prescricaoItemRespostasHoras.Where(x => x.PrescricaoItemRespostaId == prescricaoItemResposta.Id);
                        
                        if (prescricaoItemResposta.Divisao.IsMedicamento 
                            || (!prescricaoItemResposta.Divisao.IsExameImagem && !prescricaoItemResposta.Divisao.IsExameLaboratorial)  
                            && prescricaoItemResposta.FrequenciaId.HasValue && prescricaoItemResposta.Horarios.IsNullOrEmpty())
                        {
                            //buscando os horários atuais
                            if (prescricaoItemHoras.IsNullOrEmpty())
                            {
                                prescricaoItemHoras = await PrescricaoItemRespostaAppService.SalvaHorariosPrescricaoItem(prescricaoItemResposta, prescricaoItemRespostaDepsResolver).ConfigureAwait(false);
                            }
                        }

                        if (!prescricaoItemHoras.IsNullOrEmpty())
                        {
                            prescricaoItemResposta.HorariosPrescricaoItens = prescricaoItemHoras.Select(x =>
                            {
                                x.Id = 0;
                                return x;
                            }).ToList();
                        }

                        if (prescricaoItemResposta.Horarios.IsNullOrEmpty())
                        {
                            foreach (var hora in prescricaoItemHoras.OrderBy(o => o.DataMedicamento))
                            {
                                prescricaoItemResposta.Horarios += string.Format("{0:00}:00 ", hora.Hora);
                            }

                            if (!string.IsNullOrEmpty(prescricaoItemResposta.Horarios))
                            {
                                prescricaoItemResposta.Horarios = prescricaoItemResposta.Horarios.Substring(0, prescricaoItemResposta.Horarios.Length - 1);
                            }
                        }

                        if (prescricaoItemResposta.Divisao.IsMedicamento && prescricaoItemResposta.TotalDias.HasValue  && prescricaoItemResposta.TotalDias.Value != 0 && prescricaoItemResposta.DataInicial <= DateTime.Now )
                        {
                            var diff = DateTime.Now.Subtract(prescricaoItemResposta.DataInicial.Value);
                            if (diff.Days <= prescricaoItemResposta.TotalDias.Value)
                            {
                                if (diff.Days == prescricaoItemResposta.TotalDias.Value)
                                {
                                    retorno.Mensagens.Add(string.Format("O aprazamento do item {0} termina hoje.\n", prescricaoItemResposta.PrescricaoItem.Descricao));
                                }

                                if (diff.Days == prescricaoItemResposta.TotalDias.Value - 1)
                                {
                                    retorno.Mensagens.Add(string.Format("O aprazamento do item {0} terminará amanhã.\n", prescricaoItemResposta.PrescricaoItem.Descricao));
                                }
                            }
                            else if (diff.TotalDays == prescricaoItemResposta.TotalDias.Value + 1)
                            {
                                //retornar que o medicamento acabou ontem
                                retorno.Mensagens.Add(string.Format("O aprazamento do item {0} terminou ontem.\n", prescricaoItemResposta.PrescricaoItem.Descricao));
                                continue;
                            }
                        }

                        retorno.PrescricaoItens.Add(prescricaoItemResposta);
                    }
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<DefaultReturn<PrescricaoMedicaDto>> Copiar(long id, long atendimentoId,  DateTime? dataAgrupamento = null, long modeloPrescricaoId = 0, bool dataFutura = false)
        {
            if (dataAgrupamento == null)
            {
                dataAgrupamento = DateTime.Now;
            }
            
            if (modeloPrescricaoId != 0)
            {
                using (var modeloPrescricaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloPrescricao, long>>())
                {
                    var modeloPrescricao = await modeloPrescricaoRepository.Object
                        .GetAll().AsNoTracking()
                        .Where(w => w.Id == modeloPrescricaoId)
                        .Select(x=> new {x.Id, x.PrescricaoMedicaId})
                        .FirstOrDefaultAsync().ConfigureAwait(false);

                    return await CopiarPrescricao(modeloPrescricao?.PrescricaoMedicaId ?? 0, atendimentoId, dataAgrupamento.Value, dataFutura).ConfigureAwait(false);
                }
            }

            return await CopiarPrescricao(id, atendimentoId, dataAgrupamento.Value, dataFutura).ConfigureAwait(false);

        }

        [UnitOfWork(IsDisabled = true)]
        private async Task<DefaultReturn<PrescricaoMedicaDto>> CopiarPrescricao(long id, long atendimentoId,DateTime dataAgrupamento, bool dataFutura = false)
        {
            var result = new DefaultReturn<PrescricaoMedicaDto>();
            long novoId = 0;
            var dataPrescricao = DateTime.Now;
            if (dataFutura)
            {
                dataPrescricao = DateTime.Today.AddDays(1);
            }
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica,long>>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var prescricaoItemRespostaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemRespostaAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var itens = await this.ListarRespostasParaCopiar(id).ConfigureAwait(false);
                var prescricaoItens = itens.PrescricaoItens.ToList();
                var atendimento = await atendimentoRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == atendimentoId);
                if (prescricaoItens.Any())
                {
                    long? medicoId;
                    

                    var usuario = await usuarioRepository.Object.GetAll()
                        .AsNoTracking().Select(x => new {Id = x.Id, MedicoId = x.MedicoId})
                        .FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId)
                        .ConfigureAwait(false);
                    medicoId = usuario?.MedicoId;
                    var prescricao = await prescricaoMedicaRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                    var novo = new PrescricaoMedicaDto
                    {
                        AtendimentoId = atendimentoId,
                        CreatorUserId = AbpSession.UserId,
                        Descricao = prescricao.Descricao,
                        Observacao = prescricao.Observacao,
                        UnidadeOrganizacionalId = prescricao.UnidadeOrganizacionalId,
                        DataPrescricao = dataPrescricao,
                        IsDeleted = false,
                        IsSistema = prescricao.IsSistema,
                        PrescricaoStatusId = 1,
                        MedicoId = medicoId,
                        LeitoId = atendimento?.LeitoId,
                        Id = 0
                    };
                    var prescricaoMedicaDto = await this.CriarOuEditar(novo, false).ConfigureAwait(false);

                    foreach (var dto in prescricaoItens)
                    {
                        dto.Id = 0;
                        dto.PrescricaoMedicaId = prescricaoMedicaDto.Id;
                        dto.CreationTime = DateTime.Now;
                        dto.MedicoId = medicoId;
                        dto.PrescricaoItemStatusId = PrescricaoStatus.Inicial;
                        dto.IsAcrescimo = false;
                        dto.AcrescimoUserId = null;
                        dto.DataAcrescimo = null;
                        dto.DataLiberado = null;
                        dto.LiberadoUserId = null;
                        dto.DataSuspenso = null;
                        dto.IsSuspenso = false;
                        dto.SuspensoUserId = null;
                        dto.DoseUnica = false;
                        dto.AprovadoUserId = null;
                        dto.DataAprovado = null;
                        dto.DataAgrupamento = dataAgrupamento;

                        await prescricaoItemRespostaAppService.Object.CriarOuEditar(dto, false).ConfigureAwait(false);
                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    result.ReturnObject = novo;
                    novoId = novo.Id;
                }
                else
                {
                    itens.Mensagens.Add(string.Format(
                        "Não foram encontrados itens copiáveis na prescrição.\nNenhuma alteração realizada."));
                }

                result.Errors = itens.Mensagens.Select(x => ErroDto.Criar("", x)).ToList();
            }
            
            if (novoId != 0)
            {
                await this.AtualizaArquivoPrescricaoMedica(novoId).ConfigureAwait(false);
            }
            
            return result;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task Suspender(long id, long atendimentoId)
        {
            using (var _prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
            using (var _estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var input = await _prescricaoMedicaRepository.Object.GetAsync(id).ConfigureAwait(false);
                input.PrescricaoStatusId = PrescricaoStatus.Suspensa;
                await _prescricaoMedicaRepository.Object.UpdateAsync(input).ConfigureAwait(false);

                var retorno = await _estoquePreMovimentoAppService.Object.ExcluirSolicitacoesPrescritasNaoAtendidas(id).ConfigureAwait(false);

                if (retorno.Errors.Count == 0)
                {
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }

                await this.AtualizaArquivoPrescricaoMedica(id);
            }

            //await CriarOuEditar(prescricao);
        }

        public async Task SuspenderItemResposta(long prescricaoItemRespostaId, DateTime dataAgrupamento)
        {
            //var prescricao = await Obter(id);

            //prescricao.PrescricaoStatusId = 5;

            using (var _prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            using (var _estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var entity = _prescricaoItemRespostaRepository.Object.Get(prescricaoItemRespostaId);
                entity.PrescricaoItemStatusId = PrescricaoStatus.Suspensa;
                entity.SuspensoUserId = AbpSession.UserId;
                entity.IsSuspenso = true;
                entity.DataSuspenso = DateTime.Now;
                entity.DataAgrupamento = dataAgrupamento;

                await _prescricaoItemRespostaRepository.Object.UpdateAsync(entity).ConfigureAwait(false);

                var retorno = await _estoquePreMovimentoAppService.Object.ExcluirSolicitacoesPrescritasNaoAtendidasPorItemResposta(prescricaoItemRespostaId).ConfigureAwait(false);

                if (retorno.Errors.Count == 0)
                {
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
                else
                {
                    ((IDisposable)CurrentUnitOfWork).Dispose();
                }

                var item = await _prescricaoItemRespostaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == prescricaoItemRespostaId).ConfigureAwait(false);

                if (item != null && item.PrescricaoMedicaId.HasValue)
                {
                    await this.AtualizaArquivoPrescricaoMedica(item.PrescricaoMedicaId.Value);
                }

            }

            //await CriarOuEditar(prescricao);
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<PrescricaoMedicaDto> Liberar(long id, long atendimentoId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
                using (var solicitacaoExameAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var fatItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prescricao = await this.Obter(id).ConfigureAwait(false);
                    prescricao.Atendimento = await atendimentoAppService.Object.Obter(prescricao.AtendimentoId.Value).ConfigureAwait(false);
                    var prescricoesItens = PrescricaoItemRespostaDto.Mapear(await prescricaoItemRespostaRepository.Object
                        .GetAll().Where(x => x.PrescricaoMedicaId == id && x.PrescricaoItemStatusId == PrescricaoStatus.Inicial).AsNoTracking().ToListAsync()).ToList();
                    var divisoesIds = prescricoesItens.Where(x => x.DivisaoId.HasValue).Select(x => x.DivisaoId.Value).Distinct().ToList();
                    var prescricoesItenIds = prescricoesItens.Where(x=> x.PrescricaoItemId.HasValue).Select(x => x.PrescricaoItemId.Value).ToList();
                    var divisoes = await divisaoAppService.Object.ObterIds(divisoesIds).ConfigureAwait(false);
                    var prescricaoItems = await prescricaoItemAppService.Object.ObterIds(prescricoesItenIds).ConfigureAwait(false);
                    var faturamentoItemIds = prescricaoItems.Where(x=> x.FaturamentoItemId.HasValue).Select(x => x.FaturamentoItemId.Value).ToList();
                    var faturamentoItens = await fatItemAppService.Object.ObterIds(faturamentoItemIds).ConfigureAwait(false);
                    
                    foreach (var item in prescricoesItens)
                    {
                        item.Divisao = divisoes.FirstOrDefault(x => x.Id == item.DivisaoId);
                        if (item.PrescricaoItem == null)
                        {
                            item.PrescricaoItem = prescricaoItems.FirstOrDefault(x => x.Id == item.PrescricaoItemId);
                        }

                        if (item.PrescricaoItem != null && item.PrescricaoItem.FaturamentoItemId.HasValue)
                        {
                            item.PrescricaoItem.FaturamentoItem = faturamentoItens.FirstOrDefault(x => x.Id == item.PrescricaoItem.FaturamentoItemId);
                        }
                    }
                    
                    #region RequisicaoExame

                    var examesLab = prescricoesItens
                        .Where(f => f.Divisao.IsExameLaboratorial && f.PrescricaoItem != null && f.PrescricaoItem.FaturamentoItem != null)
                        .Where(f=> (f.PrescricaoItem.FaturamentoItem.GrupoId.HasValue && f.PrescricaoItem.FaturamentoItem.Grupo.IsLaboratorio) || f.PrescricaoItem.FaturamentoItem.IsLaboratorio).ToList();
                    
                    var examesImg = prescricoesItens.Where(
                        f => f.Divisao.IsExameImagem && (f.PrescricaoItem.FaturamentoItem != null
                                                         && ((f.PrescricaoItem.FaturamentoItem.IsLaudo
                                                              && !f.PrescricaoItem.FaturamentoItem.IsLaboratorio)
                                                             || (f.PrescricaoItem.FaturamentoItem.Grupo.IsLaudo
                                                                 && !f.PrescricaoItem.FaturamentoItem.Grupo
                                                                     .IsLaboratorio)))).ToList();
                    var exames = examesLab;
                    exames.AddRange(examesImg);
                    if (exames.Any())
                    {
                        var urgente = exames.Where(m => m.IsUrgente).ToList().Any();
                        //var produtos = prescricoesItens.Where(m => m.Divisao.IsProdutoEstoque).ToList();
                        var solicitacao = new SolicitacaoExameDto
                        {
                            AtendimentoId = atendimentoId,
                            DataSolicitacao = DateTime.Now,
                            IsDeleted = false,
                            IsSistema = false,
                            CreatorUserId = AbpSession.UserId,
                            MedicoSolicitanteId = prescricao.Atendimento?.Medico?.Id,
                            Observacao = prescricao.Observacao,
                            PrescricaoId = id,
                            Prioridade = urgente ? 2 : 1,
                            UnidadeOrganizacionalId = prescricao.UnidadeOrganizacionalId
                        };
                        var solicitacaoItens = new List<SolicitacaoExameItemDto>();
                        foreach (var item in exames)
                        {
                            solicitacaoItens.Add(
                                new SolicitacaoExameItemDto
                                {
                                    CreatorUserId = AbpSession.UserId,
                                    Descricao = item.PrescricaoItem.Descricao,
                                    FaturamentoItemId = item.PrescricaoItem.FaturamentoItemId,
                                    IsDeleted = false,
                                    IsSistema = false,
                                    MaterialId = item.PrescricaoItem.FaturamentoItem.MaterialId,
                                    PrescricaoItemRespostaId = item.Id
                                });
                        }

                        var abc = new SolicitacaoExameInputDto
                        {
                            Solicitacao = solicitacao,
                            SolicitacaoExameItens = solicitacaoItens
                        };



                        await solicitacaoExameAppService.Object.RequisitarSolicitacao(abc).ConfigureAwait(false);
                    }

                    #endregion

                    var entity = await prescricaoMedicaRepository.Object.GetAsync(prescricao.Id).ConfigureAwait(false);
                    entity.PrescricaoStatusId = PrescricaoStatus.Liberada;
                    entity.LiberadoUserId = AbpSession.UserId;
                    entity.DataLiberado = DateTime.Now;

                    await prescricaoMedicaRepository.Object.UpdateAsync(entity).ConfigureAwait(false);
                    
                    var ids = prescricoesItens.Select(p => p.Id);
                    var prescricaoItemRespostas = prescricaoItemRespostaRepository.Object.GetAll().Include(x=> x.PrescricaoItem).Where(x => ids.Contains(x.Id) && x.PrescricaoItemStatusId == PrescricaoStatus.Inicial);
                    
                    foreach (var prescricaoItemResposta in prescricaoItemRespostas)
                    {
                        prescricaoItemResposta.LiberadoUserId = AbpSession.UserId;
                        prescricaoItemResposta.DataLiberado = DateTime.Now;
                        prescricaoItemResposta.PrescricaoItemStatusId = PrescricaoStatus.Liberada;
                    }

                    await GerarFaturamentoItemAtendimento(prescricaoItemRespostas.ToList(), prescricao.AtendimentoId.Value, prescricao.Atendimento?.LeitoId);
                    
                    
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
                await this.AtualizaArquivoPrescricaoMedica(id);
                return await this.Obter(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroLiberar", ex);
            }
        }

        private static async Task GerarFaturamentoItemAtendimento(IEnumerable<PrescricaoItemResposta> prescricaoItemRespostas, long atendimentoId, long? leitoId)
        {
            if (prescricaoItemRespostas.IsNullOrEmpty())
            {
                return;
            }
            
            using (var faturamentoItemAtendimentoManager = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAtendimentoManager>())
            {
                var faturamentoItemAtendimentoDto = prescricaoItemRespostas.Select(x =>
                    new FaturamentoItemAtendimentoDto
                    {
                        Data = DateTime.Now,
                        AtendimentoId = atendimentoId,
                        Entidade = FaturamentoItemAtendimentoDto.PrescricaoItemResposta,
                        EntidadeId = x.Id,
                        LeitoId = leitoId,
                        Quantidade = x.Quantidade,
                        FaturamentoItemId = x.PrescricaoItem?.FaturamentoItemId,
                        MedicoId = x.MedicoId
                    });
                await faturamentoItemAtendimentoManager.Object.AdicionaItemAsync(faturamentoItemAtendimentoDto);
            }
        }
        
        private static async Task GerarFaturamentoItemAtendimento(PrescricaoItemRespostaDto prescricaoItemResposta, long atendimentoId, long? leitoId)
        {
            if (prescricaoItemResposta == null)
            {
                return;
            }
            using (var faturamentoItemAtendimentoManager = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAtendimentoManager>())
            {
                var faturamentoItemAtendimentoDto = new FaturamentoItemAtendimentoDto
                {
                    Data = DateTime.Now,
                    AtendimentoId = atendimentoId,
                    Entidade = FaturamentoItemAtendimentoDto.PrescricaoItemResposta,
                    EntidadeId = prescricaoItemResposta.Id,
                    LeitoId = leitoId,
                    Quantidade = prescricaoItemResposta.Quantidade,
                    FaturamentoItemId = prescricaoItemResposta.PrescricaoItem.FaturamentoItemId,
                    MedicoId = prescricaoItemResposta.MedicoId
                };
                await faturamentoItemAtendimentoManager.Object.AdicionaItemAsync(faturamentoItemAtendimentoDto);
            }
        }

        public async Task LiberarItemResposta(long prescricaoItemRespostaId)
        {
            try
            {
                //using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
                using (var solicitacaoExameAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoItemRespostaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemRespostaAppService>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var fatItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    
                    var prescricaoItemResposta = await prescricaoItemRespostaAppService.Object.Obter(prescricaoItemRespostaId).ConfigureAwait(false);

                    var prescricao = await this.Obter(prescricaoItemResposta.PrescricaoMedicaId.Value).ConfigureAwait(false);
                    
                    var listTemp = new List<PrescricaoItemRespostaDto>();
                    prescricaoItemResposta.Divisao = await divisaoAppService.Object.Obter(prescricaoItemResposta.DivisaoId.Value).ConfigureAwait(false);
                    if (prescricaoItemResposta.PrescricaoItem == null)
                    {
                        prescricaoItemResposta.PrescricaoItem = await prescricaoItemAppService.Object.Obter(prescricaoItemResposta.PrescricaoItemId.Value)
                                                  .ConfigureAwait(false);
                    }

                    if (prescricaoItemResposta.PrescricaoItem != null && prescricaoItemResposta.PrescricaoItem.FaturamentoItemId.HasValue)
                    {
                        prescricaoItemResposta.PrescricaoItem.FaturamentoItem =
                            await fatItemAppService.Object.Obter(prescricaoItemResposta.PrescricaoItem.FaturamentoItemId.Value)
                                .ConfigureAwait(false);
                    }

                    listTemp.Add(prescricaoItemResposta);

                    var prescricoesItens = listTemp;

                    #region RequisicaoExame

                    var examesLab = prescricoesItens.Where(
                        f => f.Divisao.IsExameLaboratorial
                             && (f.PrescricaoItem.FaturamentoItem != null
                                 && (f.PrescricaoItem.FaturamentoItem.Grupo.IsLaboratorio
                                     || f.PrescricaoItem.FaturamentoItem.IsLaboratorio))).ToList();
                    var examesImg = prescricoesItens.Where(
                        f => f.Divisao.IsExameImagem && (f.PrescricaoItem.FaturamentoItem != null
                                                         && ((f.PrescricaoItem.FaturamentoItem.IsLaudo
                                                              && !f.PrescricaoItem.FaturamentoItem.IsLaboratorio)
                                                             || (f.PrescricaoItem.FaturamentoItem.Grupo.IsLaudo
                                                                 && !f.PrescricaoItem.FaturamentoItem.Grupo
                                                                     .IsLaboratorio)))).ToList();
                    var exames = examesLab;
                    exames.AddRange(examesImg);
                    if (exames.Any())
                    {
                        var urgente = exames.Where(m => m.IsUrgente).ToList().Any();
                        //var produtos = prescricoesItens.Where(m => m.Divisao.IsProdutoEstoque).ToList();
                        var solicitacao = new SolicitacaoExameDto
                        {
                            AtendimentoId = prescricaoItemResposta.PrescricaoMedica?.AtendimentoId,
                            DataSolicitacao = DateTime.Now,
                            IsDeleted = false,
                            IsSistema = false,
                            CreatorUserId = AbpSession.UserId,
                            MedicoSolicitanteId = prescricaoItemResposta.PrescricaoMedica?.Atendimento?.Medico?.Id,
                            Observacao = prescricaoItemResposta.PrescricaoMedica?.Observacao,
                            PrescricaoId = prescricaoItemResposta.PrescricaoMedica?.Id,
                            Prioridade = urgente ? 2 : 1,
                            UnidadeOrganizacionalId = prescricaoItemResposta.PrescricaoMedica?.UnidadeOrganizacionalId
                        };
                        var solicitacaoItens = new List<SolicitacaoExameItemDto>();
                        foreach (var item in exames)
                        {
                            solicitacaoItens.Add(
                                new SolicitacaoExameItemDto
                                {
                                    CreatorUserId = AbpSession.UserId,
                                    Descricao = item.PrescricaoItem.Descricao,
                                    FaturamentoItemId = item.PrescricaoItem.FaturamentoItemId,
                                    IsDeleted = false,
                                    IsSistema = false,
                                    MaterialId = item.PrescricaoItem.FaturamentoItem.MaterialId,
                                    PrescricaoItemRespostaId = item.Id
                                });
                        }

                        var abc = new SolicitacaoExameInputDto
                        {
                            Solicitacao = solicitacao,
                            SolicitacaoExameItens = solicitacaoItens
                        };
                        
                        await solicitacaoExameAppService.Object.RequisitarSolicitacao(abc).ConfigureAwait(false);
                    }
                    #endregion
                    
                    var entity = prescricaoItemRespostaRepository.Object.Get(prescricaoItemRespostaId);
                    entity.PrescricaoItemStatusId = PrescricaoStatus.Liberada;
                    entity.LiberadoUserId = AbpSession.UserId;
                    entity.DataLiberado = DateTime.Now;
                    
                    await GerarFaturamentoItemAtendimento(prescricaoItemResposta, prescricao.AtendimentoId.Value, prescricao.Atendimento?.LeitoId);

                    await prescricaoItemRespostaRepository.Object.UpdateAsync(entity).ConfigureAwait(false);

                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();

                    if (entity != null && entity.PrescricaoMedicaId.HasValue)
                    {
                        await this.AtualizaArquivoPrescricaoMedica(entity.PrescricaoMedicaId.Value);
                    }

                    //await CriarOuEditar(prescricao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroLiberar", ex);
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task Aprovar(long id, long atendimentoId)
        {
            try
            {
                using (var _atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var _divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
                using (var _prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var _prescricaoItemHoraRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                using (var _unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var _prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var prescricao = await this.Obter(id).ConfigureAwait(false);
                    prescricao.Atendimento = await _atendimentoAppService.Object.Obter(prescricao.AtendimentoId.Value).ConfigureAwait(false);
                    var prescricoesItens = PrescricaoItemRespostaDto.Mapear(await prescricaoItemRespostaRepository.Object
                        .GetAll().Where(x => x.PrescricaoMedicaId == id && x.PrescricaoItemStatusId == PrescricaoStatus.Liberada).AsNoTracking().ToListAsync()).ToList();
                    foreach (var item in prescricoesItens)
                    {

                        item.Divisao = await _divisaoAppService.Object.Obter(item.DivisaoId.Value).ConfigureAwait(false);
                        item.PrescricaoItem = await _prescricaoItemAppService.Object.Obter(item.PrescricaoItemId.Value).ConfigureAwait(false);
                        //item.PrescricaoItem.FaturamentoItem = await _fatItemAppService.Obter(item.PrescricaoItem.FaturamentoItemId.Value);

                        var prescricaoHorarios = _prescricaoItemHoraRepositorio.Object.GetAll().Where(w => w.PrescricaoItemRespostaId == item.Id).ToList();
                        var result = PrescricaoItemHoraDto.Mapear(prescricaoHorarios).ToList();
                        item.HorariosPrescricaoItens = result;

                        item.UnidadeOrganizacional = await _unidadeOrganizacionalAppService.Object.ObterPorId(item.UnidadeOrganizacionalId ?? 0).ConfigureAwait(false);
                    }

                    #region RequisicaoEstoque

                    //será necessário fazer uma solicitação por estoque
                    var estoques = prescricoesItens.Where(m => m.Divisao.IsProdutoEstoque).Select(s => s.UnidadeOrganizacional?.EstoqueId).Distinct().ToList();
                    await this.CriaRequisicaoPorDia(estoques, prescricoesItens, prescricao);
                    #endregion

                    var input = _prescricaoMedicaRepository.Object.Get(prescricao.Id);
                    input.PrescricaoStatusId = PrescricaoStatus.Aprovada;

                    await _prescricaoMedicaRepository.Object.UpdateAsync(input).ConfigureAwait(false);

                    var ids = prescricoesItens.Select(p => p.Id);
                    var prescricaoItemRespostas = prescricaoItemRespostaRepository.Object.GetAll().Where(x => ids.Contains(x.Id) && x.PrescricaoItemStatusId == PrescricaoStatus.Liberada);
                    foreach (var prescricaoItemResposta in prescricaoItemRespostas)
                    {
                        prescricaoItemResposta.AprovadoUserId = AbpSession.UserId;
                        prescricaoItemResposta.DataAprovado = DateTime.Now;
                        prescricaoItemResposta.PrescricaoItemStatusId = PrescricaoStatus.Aprovada;
                    }
                    
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
                await this.AtualizaArquivoPrescricaoMedica(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroAprovar", ex);
            }
        }

        public async Task AprovarItemResposta(long prescricaoItemRespostaId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoItemRespostaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemRespostaAppService>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var prescricaoItemHoraRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                {
                    var prescricaoItemResposta = await prescricaoItemRespostaAppService.Object.Obter(prescricaoItemRespostaId).ConfigureAwait(false);
                    prescricaoItemResposta.PrescricaoMedica.Atendimento = await atendimentoAppService.Object.Obter(prescricaoItemResposta.PrescricaoMedica?.AtendimentoId ?? 0).ConfigureAwait(false);
                    prescricaoItemResposta.HorariosPrescricaoItens = PrescricaoItemHoraDto.Mapear(prescricaoItemHoraRepositorio.Object.GetAll().Where(w => w.PrescricaoItemRespostaId == prescricaoItemResposta.Id).ToList()).ToList();

                    var prescricoesItens = new List<PrescricaoItemRespostaDto> { prescricaoItemResposta };

                    #region RequisicaoEstoque

                    //será necessário fazer uma solicitação por estoque
                    var estoques = prescricoesItens.Where(m => m.Divisao.IsProdutoEstoque).Select(s => s.UnidadeOrganizacional?.EstoqueId).Distinct().ToList();

                    await this.CriaRequisicaoPorDia(estoques, prescricoesItens, prescricaoItemResposta.PrescricaoMedica);

                    #endregion
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var entity = prescricaoItemRespostaRepository.Object.Get(prescricaoItemRespostaId);
                        entity.PrescricaoItemStatusId = PrescricaoStatus.Aprovada;
                        entity.AprovadoUserId = AbpSession.UserId;
                        entity.DataAprovado = DateTime.Now;

                        await prescricaoItemRespostaRepository.Object.UpdateAsync(entity).ConfigureAwait(false);

                        await unitOfWorkManager.Object.Current.SaveChangesAsync();
                        await unitOfWork.CompleteAsync();
                        unitOfWork.Dispose();

                        if (entity != null && entity.PrescricaoMedicaId.HasValue)
                        {
                            await this.AtualizaArquivoPrescricaoMedica(entity.PrescricaoMedicaId.Value);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroAprovar", ex);
            }
        }

        public async Task CriaRequisicaoPorDia(List<long?> estoques, List<PrescricaoItemRespostaDto> prescricoesItens, PrescricaoMedicaDto prescricao)
        {
            using (var formulaEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueAppService>())
            using (var formulaEstoqueKitAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueKitAppService>())
            using (var estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                foreach (var estoque in estoques)
                {
                    var est = estoque ?? 1;
                    var estoqueItens = prescricoesItens.Where(m => m.UnidadeOrganizacional != null &&  m.UnidadeOrganizacional.EstoqueId == est).ToList();
                    var precricoesHorarios = estoqueItens.Select(s => s.HorariosPrescricaoItens);
                    var horarios = precricoesHorarios.SelectMany(s => s.OrderBy(o => o.DataMedicamento).Select(s2 => s2.DataMedicamento.Date)).Distinct();

                    var estoquePreMovimentoDto = new EstoquePreMovimentoDto
                    {
                        AtendimentoId = prescricao.AtendimentoId,
                        EmpresaId = prescricao.Atendimento.EmpresaId,
                        EstTipoMovimentoId = (long)EnumTipoMovimento.Paciente_Saida,
                        EstoqueId = est,
                        Movimento = DateTime.Now,
                        Emissao = DateTime.Now,
                        HoraPrescrita = prescricao.DataPrescricao,
                        PrescricaoMedicaId = prescricao.Id,
                    };
                    var itensHorario = estoqueItens;

                    var movItens = new List<EstoquePreMovimentoItemDto>();

                    foreach (var item in itensHorario)
                    {
                        if (item.HorariosPrescricaoItens.IsNullOrEmpty())
                        {
                            continue;
                        }

                        movItens.Add(new EstoquePreMovimentoItemDto
                        {
                            ProdutoId = item.PrescricaoItem.ProdutoId.Value,
                            Quantidade = item.Quantidade.Value * item.HorariosPrescricaoItens.Count(),
                            ProdutoUnidadeId = item.UnidadeId
                        });


                        var formulasEstoque = await formulaEstoqueAppService.Object.ListarPorPrescricaoItem((long)item.PrescricaoItemId).ConfigureAwait(false);

                        foreach (var formulaEstoque in formulasEstoque.Items)
                        {
                            movItens.Add(new EstoquePreMovimentoItemDto
                            {
                                ProdutoId = formulaEstoque.ProdutoId.Value,
                                Quantidade = formulaEstoque.Quantidade * item.HorariosPrescricaoItens.Count(),
                                ProdutoUnidadeId = formulaEstoque.UnidadeId,
                            });
                        }

                        var kitItens = formulaEstoqueKitAppService.Object.ListarItensKitPorPrescricaoItem((long)item.PrescricaoItemId);

                        foreach (var kitItem in kitItens)
                        {
                            movItens.Add(
                                new EstoquePreMovimentoItemDto
                                {
                                    ProdutoId = kitItem.ProdutoId,
                                    Quantidade = kitItem.Quantidade * item.HorariosPrescricaoItens.Count(),
                                    ProdutoUnidadeId = kitItem.UnidadeId
                                });
                        }

                        estoquePreMovimentoDto.Itens = JsonConvert.SerializeObject(movItens);
                    }

                    var result = estoquePreMovimentoAppService.Object.CriarOuEditarSolicitacao(estoquePreMovimentoDto);
                }
            }
        }

        public async Task CriaRequisicaoPorHorario(List<long?> estoques, List<PrescricaoItemRespostaDto> prescricoesItens, PrescricaoMedicaDto prescricao)
        {
            using (var formulaEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueAppService>())
            using (var formulaEstoqueKitAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueKitAppService>())
            using (var estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                foreach (var estoque in estoques)
                {
                    var est = estoque ?? 1;
                    var estoqueItens = prescricoesItens.Where(m => m.UnidadeOrganizacional.EstoqueId == est).ToList();
                    var precricoesHorarios = estoqueItens.Select(s => s.HorariosPrescricaoItens);
                    var horarios = precricoesHorarios.SelectMany(s => s.OrderBy(o => o.DataMedicamento).Select(s2 => s2.DataMedicamento)).Distinct();

                    foreach (var horario in horarios)
                    {
                        var estoquePreMovimentoDto = new EstoquePreMovimentoDto
                        {
                            AtendimentoId = prescricao.AtendimentoId,
                            EmpresaId = prescricao.Atendimento.EmpresaId,
                            EstTipoMovimentoId = (long)EnumTipoMovimento.Paciente_Saida,
                            EstoqueId = est,
                            Movimento = DateTime.Now,
                            Emissao = DateTime.Now,
                            HoraPrescrita = horario,
                            PrescricaoMedicaId = prescricao.Id
                        };
                        var itensHorario = estoqueItens.Where(a => a.HorariosPrescricaoItens.Where(w => w.DataMedicamento == horario).Count() > 0);

                        var movItens = new List<EstoquePreMovimentoItemDto>();

                        foreach (var item in itensHorario)
                        {

                            movItens.Add(new EstoquePreMovimentoItemDto
                            {
                                ProdutoId = item.PrescricaoItem.ProdutoId.Value,
                                Quantidade = item.Quantidade.Value,
                                ProdutoUnidadeId = item.UnidadeId
                            });


                            var formulasEstoque = await formulaEstoqueAppService.Object.ListarPorPrescricaoItem((long)item.PrescricaoItemId).ConfigureAwait(false);

                            foreach (var formulaEstoque in formulasEstoque.Items)
                            {
                                movItens.Add(new EstoquePreMovimentoItemDto
                                {
                                    ProdutoId = formulaEstoque.ProdutoId.Value,
                                    Quantidade = formulaEstoque.Quantidade,
                                    ProdutoUnidadeId = formulaEstoque.UnidadeId
                                });
                            }

                            var kitItens = formulaEstoqueKitAppService.Object.ListarItensKitPorPrescricaoItem((long)item.PrescricaoItemId);

                            foreach (var kitItem in kitItens)
                            {
                                movItens.Add(
                                    new EstoquePreMovimentoItemDto
                                    {
                                        ProdutoId = kitItem.ProdutoId,
                                        Quantidade = kitItem.Quantidade,
                                        ProdutoUnidadeId = kitItem.UnidadeId
                                    });
                            }

                            estoquePreMovimentoDto.Itens = JsonConvert.SerializeObject(movItens);
                        }

                        var result = estoquePreMovimentoAppService.Object.CriarOuEditarSolicitacao(estoquePreMovimentoDto);
                    }
                }
            }
        }

        public async Task ReAtivar(long id)
        {
            using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
            using (var estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                //   await _prescricaoMedicaRepository.UpdateAsync(input);

                var retorno = await estoquePreMovimentoAppService.Object.ReAtivarSolicitacoDePrescricaoMedica(id).ConfigureAwait(false);
                var input = prescricaoMedicaRepository.Object.Get(id);

                if (retorno.ReturnObject != null)
                {
                    input.PrescricaoStatusId = PrescricaoStatus.Aprovada;
                }
                else
                {
                    input.PrescricaoStatusId = PrescricaoStatus.Liberada;
                }

                if (retorno.Errors.Count == 0)
                {
                    await this.AtualizaArquivoPrescricaoMedica(input.Id);

                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
                else
                {
                    ((IDisposable)CurrentUnitOfWork).Dispose();
                }
            }
        }

        public async Task ReAtivarItemResposta(long prescricaoItemRespostaId)
        {
            using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            using (var estoquePreMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                //   await _prescricaoMedicaRepository.UpdateAsync(input);

                var retorno = await estoquePreMovimentoAppService.Object.ReAtivarSolicitacoDePrescricaoItemResposta(prescricaoItemRespostaId)
                                  .ConfigureAwait(false);


                var entity = prescricaoItemRespostaRepository.Object.Get(prescricaoItemRespostaId);


                if (retorno.ReturnObject != null)
                {
                    entity.PrescricaoItemStatusId = PrescricaoStatus.Aprovada;
                }
                else
                {
                    entity.PrescricaoItemStatusId = PrescricaoStatus.Liberada;
                }


                if (retorno.Errors.Count == 0)
                {
                    await this.AtualizaArquivoPrescricaoMedica(entity.PrescricaoMedicaId.Value);

                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
                else
                {
                    ((IDisposable)CurrentUnitOfWork).Dispose();
                }
            }
        }

        public class IncluirItemPrescricaoModeloDto
        {
            public long Id { get; set; }
            public long PrescricaoCorrenteId { get; set; }
            public long? AtendimentoId { get; set; }
            public DateTime? DataFuturaPrescricao { get; set; }
            
            public DateTime DataAgrupamento { get; set; }
        }

        [UnitOfWork]
        public async Task<DefaultReturn<RetornoPrescricao>> IncluirItemPrescricaoModelo(IncluirItemPrescricaoModeloDto input)
        {
            using (var modeloPrescricaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloPrescricao, long>>())
            using (var prescricaoItemRespostaAppService =
                IocManager.Instance.ResolveAsDisposable<IPrescricaoItemRespostaAppService>())
            {
                var prescricaoMedicaDto = new PrescricaoMedicaDto();
                var modeloPrescricao = await modeloPrescricaoRepository.Object.GetAll().Where(w => w.Id == input.Id).FirstOrDefaultAsync().ConfigureAwait(false);
                
                if (input.PrescricaoCorrenteId != 0)
                {
                    var prescricaoId = modeloPrescricao != null ? modeloPrescricao.PrescricaoMedicaId : 0;

                    var itens = await this.ListarRespostasParaCopiar(prescricaoId).ConfigureAwait(false);
                    var prescricaoItens = itens.PrescricaoItens.ToList();
                    if (prescricaoItens.Any())
                    {
                        long? medicoId;
                        using (var usuarioRepository = IocManager.Instance
                            .ResolveAsDisposable<IRepository<Authorization.Users.User, long>>())
                        {
                            var usuario = usuarioRepository.Object.GetAll().Include(i => i.Medico)
                                .AsNoTracking()
                                .FirstOrDefault(w => w.Id == this.AbpSession.UserId);

                            medicoId = usuario?.MedicoId;
                        }
                        var prescricaoCorrenteDto = await this.Obter(input.PrescricaoCorrenteId).ConfigureAwait(false);
                        prescricaoMedicaDto = await this.CriarOuEditar(prescricaoCorrenteDto, false).ConfigureAwait(false);
                        
                        foreach (var item in prescricaoItens)
                        {
                            item.Id = 0;
                            item.PrescricaoMedicaId = prescricaoMedicaDto.Id;
                            item.CreationTime = DateTime.Now;
                            item.MedicoId = medicoId;
                            item.PrescricaoItemStatusId = PrescricaoStatus.Inicial;
                            item.IsAcrescimo = prescricaoMedicaDto.PrescricaoStatusId != PrescricaoStatus.Inicial;
                            item.AcrescimoUserId = null;
                            item.DataAcrescimo = null;
                            item.DataLiberado = null;
                            item.LiberadoUserId = null;
                            item.DataSuspenso = null;
                            item.IsSuspenso = false;
                            item.SuspensoUserId = null;
                            item.DoseUnica = false;
                            item.AprovadoUserId = null;
                            item.DataAprovado = null;
                            item.DataAgrupamento = input.DataAgrupamento;
                            
                            await prescricaoItemRespostaAppService.Object.CriarOuEditar(item, false).ConfigureAwait(false);
                        }

                        await this.AtualizaArquivoPrescricaoMedica(input.PrescricaoCorrenteId).ConfigureAwait(false);
                    }
                    else
                    {
                        itens.Mensagens.Add(string.Format("Não foram encontrados itens copiáveis na prescrição.\nNenhuma alteração realizada."));
                    }

                    return new DefaultReturn<RetornoPrescricao>()
                    {
                        Errors = itens.Mensagens.Select(x => ErroDto.Criar(x)).ToList(),
                        ReturnObject = new RetornoPrescricao {PrescricaoId = prescricaoMedicaDto.Id}
                    };
                }
                else
                {
                    var copia = await Copiar(0, input.AtendimentoId ?? 0, input.DataAgrupamento, input.Id).ConfigureAwait(false);

                    var retorno = new RetornoPrescricao
                    {
                        Mensagem = String.Join("\n", copia?.Errors.Select(x => x.Descricao)),
                        PrescricaoId = copia.ReturnObject.Id
                    };

                    return new DefaultReturn<RetornoPrescricao>()
                    {
                        Errors = copia?.Errors,
                        ReturnObject = retorno
                    };
                }
            }

        }



        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<RetornoCheckarMedico> ChecarMedicoPrescricao(long prescricaoId)
        {
            try
            {
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                {
                    var prescricaoMedica = await prescricaoMedicaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == prescricaoId).ConfigureAwait(false);

                    var usuario = await usuarioRepository.Object.GetAll().Include(i => i.Medico)
                            .Include(i => i.Medico.MedicoEspecialidades)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId);

                    if (usuario == null || usuario.MedicoId == null || prescricaoMedica == null)
                    {
                        return new RetornoCheckarMedico { HasError = true };
                    }

                    if (prescricaoMedica.PrescricaoStatusId != PrescricaoStatus.Inicial)
                    {
                        return new RetornoCheckarMedico { HasError = false, IsMedico = true };
                    }

                    return new RetornoCheckarMedico { HasError = false, IsMedico = prescricaoMedica.MedicoId == usuario.MedicoId };
                }
            }
            catch (Exception)
            {
                return new RetornoCheckarMedico { HasError = true };
            }

        }


        public async Task<bool> AlterarMedicoPrescricao(long prescricaoId)
        {
            try
            {
                using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                {
                    var prescricaoMedica = await prescricaoMedicaRepository.Object.GetAll().FirstOrDefaultAsync(x => x.Id == prescricaoId).ConfigureAwait(false);

                    var usuario = await usuarioRepository.Object.GetAll().Include(i => i.Medico)
                            .Include(i => i.Medico.MedicoEspecialidades)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId);

                    if (usuario != null && usuario.MedicoId != null && prescricaoMedica != null)
                    {
                        prescricaoMedica.MedicoId = usuario.MedicoId;

                        await prescricaoMedicaRepository.Object.UpdateAsync(prescricaoMedica).ConfigureAwait(false);
                        await this.AtualizaArquivoPrescricaoMedica(prescricaoMedica.Id);

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    public class RetornoCheckarMedico
    {
        public bool HasError { get; set; }

        public bool IsMedico { get; set; }
    }

    public class RetornoPrescricao
    {
        public string Mensagem { get; set; }
        public long? PrescricaoId { get; set; }
    }

    public class RetornoValidacaoDuplicidadeNaPrescricao
    {
        public bool TemMensagem { get; set; }
        public string Mensagem { get; set; }
        public string TituloMensagem { get; set; }

        public RetornoValidacaoDuplicidadeNaPrescricao()
        {
            
        }
        
        public RetornoValidacaoDuplicidadeNaPrescricao(bool temMensagem, string mensagem, string tituloMensagem)
        {
            TemMensagem = temMensagem;
            Mensagem = mensagem;
            TituloMensagem = tituloMensagem;
        }
    }

    public class PrescricaoMedicaListarInput : ListarInput
    {
        public string Filtro { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime StartDateNotNull { get; set; }

        public DateTime EndDateNotNull { get; set; }

        public long? EmpresaId { get; set; }

        public string Id { get; set; }

        public string PrincipalId { get; set; }

        public long? OperacaoId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataPrescricao desc, Id desc";
            }
        }
    }
}