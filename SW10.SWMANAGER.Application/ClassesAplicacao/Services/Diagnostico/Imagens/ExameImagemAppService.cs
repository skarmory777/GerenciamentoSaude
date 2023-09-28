using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens
{
    [AbpAuthorize(AppPermissions.Pages_Tenant_Diagnosticos_Imagens)]
    public class ExameImagemAppService : SWMANAGERAppServiceBase, IExameImagemAppService
    {
        private readonly IExameListExcelExporter _exameListExcelExporter;
        private readonly IRepository<LaudoMovimento, long> _laudoMovimentoRepository;
        private readonly IRepository<LaudoMovimentoItem, long> _laudoMovimentoItemRepository;
        private readonly IRepository<LaudoMovimentoStatus, long> _laudoMovimentoStatusRepository;
        private readonly IRepository<Atendimento, long> _atendimentoRepository;
        private readonly IRepository<Paciente, long> _pacienteRepository;
        private readonly IRepository<Medico, long> _medicoRepository;
        private readonly IRepository<FaturamentoItem, long> _faturamentoItemRepository;
        private readonly IRepository<SolicitacaoExameItem, long> _solicitacaoExameItemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ExameImagemAppService(
            IExameListExcelExporter exameListExcelExporter,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<LaudoMovimento, long> laudoMovimentoRepository,
            IRepository<LaudoMovimentoItem, long> laudoMovimentoItemRepository,
            IRepository<LaudoMovimentoStatus, long> laudoMovimentoStatusRepository,
            IRepository<Atendimento, long> atendimentoRepository,
            IRepository<Paciente, long> pacienteRepository,
            IRepository<Medico, long> medicoRepository,
            IRepository<FaturamentoItem, long> faturamentoItemRepository,
            IRepository<SolicitacaoExameItem, long> solicitacaoExameItemRepository
            )
        {
            _exameListExcelExporter = exameListExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _laudoMovimentoRepository = laudoMovimentoRepository;
            _laudoMovimentoItemRepository = laudoMovimentoItemRepository;
            _laudoMovimentoStatusRepository = laudoMovimentoStatusRepository;
            _atendimentoRepository = atendimentoRepository;
            _pacienteRepository = pacienteRepository;
            _medicoRepository = medicoRepository;
            _faturamentoItemRepository = faturamentoItemRepository;
            _solicitacaoExameItemRepository = solicitacaoExameItemRepository;
        }

        public async Task<PagedResultDto<ExameListDto>> GetExames(GetExamesInput input)
        {
            var query = _laudoMovimentoRepository
                .GetAllIncluding(
                    a => a.Atendimento,
                    p => p.Atendimento.Paciente,
                    m => m.Atendimento.Medico,
                    s => s.LaudoMovimentoStatus
                 )
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    m =>
                        m.Atendimento.Paciente.NomeCompleto.Contains(input.Filter) ||
                        m.Atendimento.Medico.NomeCompleto.Contains(input.Filter)
                );

            var exameCount = await query.CountAsync();
            var exames = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var exameListDtos = exames.MapTo<List<ExameListDto>>();

            return new PagedResultDto<ExameListDto>(
                exameCount,
                exameListDtos
                );
        }

        public async Task<PagedResultDto<ExameItemListDto>> GetExameItens(GetExamesInput input)
        {
            input.Sorting = "Id";

            long laudoMovimentoId = 0;
            long.TryParse(input.Filter, out laudoMovimentoId);

            var query = _laudoMovimentoItemRepository
                .GetAllIncluding(
                    f => f.FaturamentoItem,
                    s => s.SolicitacaoExameItem,
                    l => l.LaudoMovimento
                 )
                .Where(m => m.LaudoMovimento.Id.Equals(laudoMovimentoId)
                );

            var exameItemCount = await query.CountAsync();
            var exameItens = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var exameItemListDtos = exameItens.MapTo<List<ExameItemListDto>>();

            return new PagedResultDto<ExameItemListDto>(
                exameItemCount,
                exameItemListDtos
                );
        }

        public async Task<PagedResultDto<ExameItemListDto>> GetAllExameItens(GetExamesInput input)
        {
            input.Sorting = "Id";

            long laudoMovimentoId = 0;
            long.TryParse(input.Filter, out laudoMovimentoId);

            var query = _laudoMovimentoItemRepository
                .GetAllIncluding(
                    f => f.FaturamentoItem,
                    s => s.SolicitacaoExameItem,
                    l => l.LaudoMovimento
                );

            var exameItemCount = await query.CountAsync();
            var exameItens = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var exameItemListDtos = exameItens.MapTo<List<ExameItemListDto>>();

            return new PagedResultDto<ExameItemListDto>(
                exameItemCount,
                exameItemListDtos
                );
        }


        public async Task<ListResultDto<ExameListDto>> GetAllExames()
        {
            var query = _laudoMovimentoRepository
                .GetAllIncluding(
                    a => a.Atendimento,
                    p => p.Atendimento.Paciente,
                    m => m.Atendimento.Medico,
                    s => s.LaudoMovimentoStatus
                 );


            var exameCount = await query.CountAsync();
            var exames = await query
                .ToListAsync();

            var exameListDtos = exames.MapTo<List<ExameListDto>>();

            return new ListResultDto<ExameListDto>
            {
                Items = exameListDtos
            };
        }

        public async Task<FileDto> GetExamesToExcel()
        {
            var exames = await _laudoMovimentoRepository
                .GetAllIncluding(
                    a => a.Atendimento,
                    p => p.Atendimento.Paciente,
                    m => m.Atendimento.Medico,
                    s => s.LaudoMovimentoStatus
                 ).ToListAsync();
            var exameListDtos = exames.MapTo<List<ExameListDto>>();

            return _exameListExcelExporter.ExportToFile(exameListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Create, AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Edit)]
        public async Task<GetExameForEditOutput> GetExameForEdit(NullableIdDto<long> input)
        {
            var output = new GetExameForEditOutput();

            if (!input.Id.HasValue)
            {
                output.Exame = new ExameEditDto();
            }
            else
            {
                var exame = await _laudoMovimentoRepository.GetAllIncluding(
                    a => a.Atendimento,
                    p => p.Atendimento.Paciente,
                    m => m.Atendimento.Medico,
                    s => s.LaudoMovimentoStatus,
                    c => c.Convenio,
                    l => l.Leito,
                    o => o.Atendimento.Origem
                 ).WhereIf(
                    input.Id.HasValue, m => m.Id.Equals(input.Id.Value)).FirstAsync();

                output.Exame = exame.MapTo<ExameEditDto>();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Create, AppPermissions.Pages_Tenant_Diagnosticos_Imagens_Edit)]
        public async Task<GetExameItemForEditOutput> GetExameItemForEdit(NullableIdDto<long> input)
        {
            var output = new GetExameItemForEditOutput();

            if (!input.Id.HasValue)
            {
                output.ExameItem = new ExameItemEditDto();
            }
            else
            {
                var exameItem = await _laudoMovimentoItemRepository.GetAllIncluding(
                    f => f.FaturamentoItem
                ).WhereIf(
                    input.Id.HasValue, m => m.Id.Equals(input.Id.Value)).FirstAsync();

                output.ExameItem = exameItem.MapTo<ExameItemEditDto>();
            }

            return output;
        }

        [UnitOfWork]
        public async Task CreateOrUpdateExame(CreateOrUpdateExameInput input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (input.Exame.Id.HasValue && input.Exame.Id != 0)
                    {
                        await UpdateExameAsync(input);
                    }
                    else
                    {
                        await CreateExameAsync(input);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [UnitOfWork]
        public async Task CreateOrUpdateExameItem(CreateOrUpdateExameItemInput input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (input.ExameItem.Id.HasValue)
                    {
                        await UpdateExameItemAsync(input);
                    }
                    else
                    {
                        await CreateExameItemAsync(input);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Delete)]
        public async Task DeleteExameItem(EntityDto<long> input)
        {
            //throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            await _laudoMovimentoItemRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Delete)]
        public async Task DeleteExame(EntityDto<long> input)
        {
            //throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            await _laudoMovimentoRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateExameAsync(CreateOrUpdateExameInput input)
        {
            Debug.Assert(input.Exame.Id != null, "input.Exame.Id should be set.");

            var exame = await _laudoMovimentoRepository.FirstOrDefaultAsync(input.Exame.Id.Value);

            //input.Exame.MapTo(exame);
            exame.AtendimentoId = input.Exame.AtendimentoId;
            exame.LaudoMovimentoStatusId = input.Exame.LaudoMovimentoStatusId;
            exame.ConvenioId = input.Exame.ConvenioId;
            exame.LeitoId = input.Exame.LeitoId;
            exame.IsContraste = input.Exame.IsContraste;
            exame.QtdeConstraste = input.Exame.QtdeConstraste;
            exame.Obs = input.Exame.Obs;
            var check = await _laudoMovimentoRepository.UpdateAsync(exame);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateExameItemAsync(CreateOrUpdateExameItemInput input)
        {
            Debug.Assert(input.ExameItem.Id != null, "input.ExameItem.Id should be set.");

            var exameItem = await _laudoMovimentoItemRepository.FirstOrDefaultAsync(input.ExameItem.Id.Value);

            //input.ExameItem.MapTo(exameItem);
            exameItem.FaturamentoItemId = input.ExameItem.FaturamentoItemId;
            exameItem.Parecer = input.ExameItem.Parecer;
            exameItem.Laudo = input.ExameItem.Laudo;
            exameItem.Revisao = input.ExameItem.Revisao;
            exameItem.Retificacao = input.ExameItem.Retificacao;
            exameItem.Status = input.ExameItem.Status;
            exameItem.ParecerData = input.ExameItem.ParecerData;
            exameItem.LaudoData = input.ExameItem.LaudoData;
            exameItem.RevisaoData = input.ExameItem.RevisaoData;
            exameItem.RetificacaoData = input.ExameItem.RetificacaoData;
            exameItem.TecnicoId = input.ExameItem.TecnicoId;
            exameItem.UsuarioParecerId = input.ExameItem.UsuarioParecerId;
            exameItem.UsuarioLaudoId = input.ExameItem.UsuarioLaudoId;
            exameItem.UsuarioRevisaoId = input.ExameItem.UsuarioRevisaoId;
            exameItem.UsuarioRetificacaoId = input.ExameItem.UsuarioRetificacaoId;


            var check = await _laudoMovimentoItemRepository.UpdateAsync(exameItem);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        protected virtual async Task CreateExameAsync(CreateOrUpdateExameInput input)
        {
            //var exame = input.Exame.MapTo<LaudoMovimento>();
            var exame = new LaudoMovimento();
            Random R = new Random();
            exame.Codigo = ((long)R.Next(0, 100000) * (long)R.Next(0, 100000)).ToString().PadLeft(10, '0');
            exame.LaudoMovimentoStatusId = 1;
            exame.AtendimentoId = input.Exame.AtendimentoId;
            exame.ConvenioId = input.Exame.ConvenioId;
            exame.LeitoId = input.Exame.LeitoId;
            exame.IsContraste = input.Exame.IsContraste;
            exame.QtdeConstraste = input.Exame.QtdeConstraste;
            exame.Obs = input.Exame.Obs;

            var check = await _laudoMovimentoRepository.InsertAsync(exame);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        protected virtual async Task CreateExameItemAsync(CreateOrUpdateExameItemInput input)
        {
            //var exameItem = input.ExameItem.MapTo<LaudoMovimentoItem>();
            var exameItem = new LaudoMovimentoItem();
            //     exameItem.LaudoMovimentoItemId = input.ExameItem.LaudoMovimentoId;
            exameItem.LaudoMovimentoId = input.ExameItem.LaudoMovimentoId;
            exameItem.FaturamentoItemId = input.ExameItem.FaturamentoItemId;
            exameItem.Status = 1;

            if (input.ExameItem.SolicitacaoExameItemId != null)
            {
                var solExameItemid = _solicitacaoExameItemRepository.Get((long)input.ExameItem.SolicitacaoExameItemId);
                exameItem.SolicitacaoExameItem = solExameItemid;
            }

            var check = await _laudoMovimentoItemRepository.InsertAsync(exameItem);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        // Relatorio
        public async Task<LauMovimentoReportModel> ObterReportModel(long id)
        {
            try
            {
                var c = await _laudoMovimentoRepository
                    .GetAll()
                    .Include(d => d.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(a => a.Atendimento)
                    .Include(a => a.Atendimento.Paciente)
                    .Include(a => a.Atendimento.Paciente.SisPessoa)
                    .Include(a => a.Atendimento.Guia)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var lauMovimento = new LauMovimentoReportModel();
                lauMovimento.Id = c.Id;
                //lauMovimento.Matricula = c.Matricula;
                //lauMovimento.CodDependente = c.CodDependente;
                //lauMovimento.NumeroGuia = c.NumeroGuia;
                //lauMovimento.Titular = c.Titular;
                //lauMovimento.GuiaOperadora = c.GuiaOperadora;
                //lauMovimento.GuiaPrincipal = c.GuiaPrincipal;
                //lauMovimento.Observacao = c.Observacao;
                //lauMovimento.SenhaAutorizacao = c.SenhaAutorizacao;
                //lauMovimento.IdentAcompanhante = c.IdentAcompanhante;
                //lauMovimento.PacienteId = c.Atendimento.Paciente.Id;
                //lauMovimento.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                //lauMovimento.AtendimentoId = c.Atendimento.Id;
                //lauMovimento.MedicoId = c.Medico != null ? c.Medico.Id : 0;
                //lauMovimento.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                //lauMovimento.ConvenioId = c.Convenio != null ? c.Convenio.Id : 0;
                //lauMovimento.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                //lauMovimento.PlanoId = c.Atendimento.Plano != null ? c.Atendimento.Plano.Id : 0;
                //lauMovimento.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                //lauMovimento.GuiaId = c.Atendimento.Guia != null ? c.Atendimento.Guia.Id : 0;
                //lauMovimento.GuiaNumero = c.Atendimento.GuiaNumero;
                //lauMovimento.EmpresaId = c.Empresa != null ? c.Empresa.Id : 0;
                //lauMovimento.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                //lauMovimento.AtendimentoCodigo = c.Atendimento.Codigo;
                //lauMovimento.UnidadeOrganizacionalId = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Id : 0;
                //lauMovimento.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                //lauMovimento.TipoLeitoId = c.TipoLeito != null ? c.TipoLeito.Id : 0;
                //lauMovimento.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;
                //lauMovimento.DataIncio = c.DataIncio;
                //lauMovimento.DataFim = c.DataFim;
                //lauMovimento.DataPagamento = c.DataPagamento;
                //lauMovimento.ValidadeCarteira = c.ValidadeCarteira;
                //lauMovimento.DataAutorizacao = c.DataAutorizacao;
                //lauMovimento.DiaSerie1 = c.DiaSerie1;
                //lauMovimento.DiaSerie2 = c.DiaSerie2;
                //lauMovimento.DiaSerie3 = c.DiaSerie3;
                //lauMovimento.DiaSerie4 = c.DiaSerie4;
                //lauMovimento.DiaSerie5 = c.DiaSerie5;
                //lauMovimento.DiaSerie6 = c.DiaSerie6;
                //lauMovimento.DiaSerie7 = c.DiaSerie7;
                //lauMovimento.DiaSerie8 = c.DiaSerie8;
                //lauMovimento.DiaSerie9 = c.DiaSerie9;
                //lauMovimento.DiaSerie10 = c.DiaSerie10;
                //lauMovimento.DataEntrFolhaSala = c.DataEntrFolhaSala;
                //lauMovimento.DataEntrDescCir = c.DataEntrDescCir;
                //lauMovimento.DataEntrBolAnest = c.DataEntrBolAnest;
                //lauMovimento.DataEntrCDFilme = c.DataEntrCDFilme;
                //lauMovimento.DataValidadeSenha = c.DataValidadeSenha;
                //lauMovimento.IsAutorizador = c.IsAutorizador;
                //lauMovimento.TipoAtendimento = c.TipoAtendimento;
                //lauMovimento.StatusEntrega = c.StatusEntrega;

                return lauMovimento;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }

        public async Task<PagedResultDto<LauMovimentoItemReportModel>> ListarItensReportModel(ListarLauMovimentoItensInput input)
        {
            var contarContaItens = 0;
            List<LaudoMovimentoItem> contaItens;
            List<LauMovimentoItemReportModel> contaItensDtos = new List<LauMovimentoItemReportModel>();
            try
            {
                var query = _laudoMovimentoItemRepository
                    .GetAll()
                    .Include(f => f.FaturamentoItem.LaudoGrupo)
                    .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.LaudoMovimentoId.ToString() == input.Filtro)

                    ;

                contarContaItens = await query
                    .CountAsync();

                contaItens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                foreach (var item in contaItens)
                {
                    var i = new LauMovimentoItemReportModel();

                    i.Id = item.Id;

                    //      i.Grupo = item.FaturamentoItem.Grupo != null ? item.FaturamentoItem.Grupo.Descricao : string.Empty;
                    //     i.Tipo = item.FaturamentoItem.Grupo != null ? (item.FaturamentoItem.Grupo.TipoGrupo != null ? item.FaturamentoItem.Grupo.TipoGrupo.Descricao : string.Empty) : string.Empty;

                    i.Descricao = item.Descricao;
                    i.LaudoGrupo = item.FaturamentoItem.LaudoGrupo != null ? item.FaturamentoItem.LaudoGrupo.Descricao : "";
                    //   i.FaturamentoItemId              = item.FaturamentoItemId;
                    //   i.FaturamentoItemCodigo       = item.FaturamentoItem != null ? item.FaturamentoItem.Codigo : string.Empty;
                    //   i.FaturamentoItemDescricao       = item.FaturamentoItem != null ? item.FaturamentoItem.Descricao : string.Empty;
                    //i.FaturamentoContaId             = item.FaturamentoContaId;
                    //i.Data                           = item.Data;
                    //i.Qtde                           = item.Qtde;
                    //i.UnidadeOrganizacionalId        = item.UnidadeOrganizacionalId;
                    //i.UnidadeOrganizacionalDescricao = item.UnidadeOrganizacional != null ? item.UnidadeOrganizacional.Descricao : string.Empty;
                    //i.TerceirizadoId                 = item.TerceirizadoId;
                    //i.TerceirizadoDescricao          = item.Terceirizado != null ? item.Terceirizado.Codigo : string.Empty;
                    //i.CentroCustoId                  = item.CentroCustoId;
                    //i.CentroCustoDescricao           = item.CentroCusto!= null ? item.CentroCusto.Descricao : string.Empty;
                    //i.TurnoId                        = item.TurnoId;
                    //i.TurnoDescricao                 = item.Turno != null ? item.Turno.Descricao : string.Empty;
                    //i.TipoLeitoId                    = item.TipoLeitoId;
                    //i.TipoLeitoDescricao             = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;
                    //i.ValorTemp                      = item.ValorTemp;
                    //i.MedicoId                       = item.MedicoId;
                    //i.MedicoNome                     = item.Medico != null ? item.Medico.NomeCompleto : string.Empty; ;
                    //i.IsMedCrendenciado              = item.IsMedCredenciado;
                    //i.IsGlosaMedico                  = item.IsGlosaMedico;
                    //i.MedicoEspecialidadeId          = item.MedicoEspecialidadeId;
                    //i.MedicoEspecialidadeNome        = item.MedicoEspecialidade != null ? item.MedicoEspecialidade.Especialidade.Nome : string.Empty; ;
                    //i.FaturamentoContaKitId          = item.FaturamentoContaKitId;
                    //i.IsCirurgia                     = item.IsCirurgia;
                    //i.ValorAprovado                  = item.ValorAprovado;
                    //i.ValorTaxas                     = item.ValorTaxas;
                    //i.IsValorItemManual              = item.IsValorItemManual;
                    //i.ValorItem                      = item.ValorItem;
                    //i.HMCH                           = item.HMCH;
                    //i.ValorFilme                     = item.ValorFilme;
                    //i.ValorFilmeAprovado             = item.ValorFilmeAprovado;
                    //i.ValorCOCH                      = item.ValorCOCH;
                    //i.ValorCOCHAprovado              = item.ValorCOCHAprovado;
                    //i.Percentual                     = item.Percentual;
                    //i.IsInstrCredenciado             = item.IsInstrCredenciado;
                    //i.ValorTotalRecuperado           = item.ValorTotalRecuperado;
                    //i.ValorTotalRecebido             = item.ValorTotalRecebido;
                    //i.MetragemFilme                  = item.MetragemFilme;
                    //i.MetragemFilmeAprovada          = item.MetragemFilmeAprovada;
                    //i.COCH                           = item.COCH;
                    //i.COCHAprovado                   = item.COCHAprovado;
                    //i.StatusEntrega                  = item.StatusEntrega;
                    //i.IsRecuperaMedico               = item.IsRecuperaMedico;
                    //i.IsAux1Credenciado              = item.IsAux1Credenciado;
                    //i.IsRecebeAuxiliar1              = item.IsRecebeAuxiliar1;
                    //i.IsGlosaAuxiliar1               = item.IsGlosaAuxiliar1;
                    //i.IsRecuperaAuxiliar1            = item.IsRecuperaAuxiliar1;
                    //i.IsAux2Credenciado              = item.IsAux2Credenciado;
                    //i.IsRecebeAuxiliar2              = item.IsRecebeAuxiliar2;
                    //i.IsGlosaAuxiliar2               = item.IsGlosaAuxiliar2;
                    //i.IsRecuperaAuxiliar2            = item.IsRecuperaAuxiliar2;
                    //i.IsAux3Credenciado              = item.IsAux3Credenciado;
                    //i.IsRecebeAuxiliar3              = item.IsRecebeAuxiliar3;
                    //i.IsGlosaAuxiliar3               = item.IsGlosaAuxiliar3;
                    //i.IsRecuperaAuxiliar3            = item.IsRecuperaAuxiliar3;
                    //i.IsRecebeInstrumentador         = item.IsRecebeInstrumentador;
                    //i.IsGlosaInstrumentador          = item.IsGlosaInstrumentador;
                    //i.IsRecuperaInstrumentador       = item.IsRecuperaInstrumentador;
                    //i.Observacao                     = item.Observacao;
                    //i.QtdeRecuperada                 = item.QtdeRecuperada;
                    //i.QtdeAprovada                   = item.QtdeAprovada;
                    //i.QtdeRecebida                   = item.QtdeRecebida;
                    //i.ValorMoedaAprovado             = item.ValorMoedaAprovado;
                    //i.SisMoedaId                     = item.SisMoedaId;
                    //i.SisMoedaNome                   = item.SisMoeda != null ? item.SisMoeda.Descricao : string.Empty;
                    //i.DataAutorizacao                = item.DataAutorizacao;
                    //i.SenhaAutorizacao               = item.SenhaAutorizacao;
                    //i.NomeAutorizacao                = item.NomeAutorizacao;
                    //i.ObsAutorizacao                 = item.ObsAutorizacao;
                    //i.HoraIncio                      = item.HoraIncio;
                    //i.HoraFim                        = item.HoraFim;
                    //i.ViaAcesso                      = item.ViaAcesso;
                    //i.Tecnica                        = item.Tecnica;
                    //i.ClinicaId                      = item.ClinicaId;
                    //i.FornecedorId                   = item.FornecedorId;
                    //i.FornecedorNome                 = item.Fornecedor != null ? item.Fornecedor.Descricao : string.Empty;
                    //i.NumeroNF                       = item.NumeroNF;
                    //i.IsImportaEstoque               = item.IsImportaEstoque;

                    contaItensDtos.Add(i);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
            return new PagedResultDto<LauMovimentoItemReportModel>(
                contarContaItens,
                contaItensDtos
                );
        }

    }

    public class LauMovimentoReportModel
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public long? AtendimentoId { get; set; }
        public long? LaudoMovimentoStatusId { get; set; }
        public long? ConvenioId { get; set; }
        public long? LeitoId { get; set; }
        //   public ICollection<LaudoMovimentoItem> LaudoMovimentoItens { get; set; }
        public bool IsContraste { get; set; }
        public string QtdeConstraste { get; set; }
        public string Obs { get; set; }
    }

    public class LauMovimentoItemReportModel
    {
        public long Id { get; set; }

        public string Descricao { get; set; }
        public long LaudoMovimentoId { get; set; }
        public long FaturamentoItemId { get; set; }
        public long? SolicitacaoExameItemId { get; set; }
        public string LaudoGrupo { get; set; }

        //public LaudoMovimento LaudoMovimento { get; set; }
        //public FaturamentoItem FaturamentoItem { get; set; }
        //public SolicitacaoExameItem SolicitacaoExameItem { get; set; }

        public long? TecnicoId { get; set; }
    }
}
