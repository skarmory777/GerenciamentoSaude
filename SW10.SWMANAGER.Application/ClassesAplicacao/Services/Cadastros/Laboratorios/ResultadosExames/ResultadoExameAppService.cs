using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Dependency;
using RestSharp;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Impressora;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboatorios.ResultadosExames
{
    public class ResultadoExameAppService : SWMANAGERAppServiceBase, IResultadoExameAppService
    {
        private readonly IListarResultadoExamesExcelExporter _listarResultadoExamesExcelExporter;

        private readonly IRepository<ResultadoExame, long> _resultadoExameRepositorio;

        private readonly IRepository<ExameStatus, long> _exameStatusRepositorio;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        // private readonly IExameAppService _exameAppService;
        private readonly IFaturamentoContaItemAppService _fatContaItemAppService;

        private readonly IFormataAppService _formataAppService;

        private readonly IMaterialAppService _materialAppService;

        private readonly ITabelaAppService _tabelaAppService;

        private readonly IKitExameAppService _kitExameAppService;

        private readonly IRepository<FaturamentoItem, long> _faturamentoItemRepository;

        private readonly IRepository<RegistroArquivo, long> _registroArquivoRepository;

        public ResultadoExameAppService(
            IRepository<ResultadoExame, long> resultadoExameRepositorio,
            IRepository<ExameStatus, long> exameStatusRepositorio,
            IListarResultadoExamesExcelExporter listarResultadoExamesExcelExporter,
            IUnitOfWorkManager unitOfWorkManager,

            // IExameAppService exameAppService,
            IFaturamentoContaItemAppService fatContaItemAppService,
            IFormataAppService formataAppService,
            IMaterialAppService materialAppService,
            ITabelaAppService tabelaAppService,
            IKitExameAppService kitExameAppService,
            IRepository<FaturamentoItem, long> faturamentoItemRepository,
            IRepository<RegistroArquivo, long> registroArquivoRepository)
        {
            this._resultadoExameRepositorio = resultadoExameRepositorio;
            this._exameStatusRepositorio = exameStatusRepositorio;
            this._listarResultadoExamesExcelExporter = listarResultadoExamesExcelExporter;
            this._unitOfWorkManager = unitOfWorkManager;

            // _exameAppService = exameAppService;
            this._fatContaItemAppService = fatContaItemAppService;
            this._formataAppService = formataAppService;
            this._materialAppService = materialAppService;
            this._tabelaAppService = tabelaAppService;
            this._kitExameAppService = kitExameAppService;
            this._faturamentoItemRepository = faturamentoItemRepository;
            this._registroArquivoRepository = registroArquivoRepository;
        }

        [UnitOfWork]
        [AbpAuthorize(
            AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create,
            AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(ResultadoExameDto input)
        {
            try
            {
                using (var ocorrenciaRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
                {
                    var resultadoExame = new ResultadoExame
                    {
                        // input.MapTo<ResultadoExame>();
                        Codigo = input.Codigo,
                        CreationTime = input.CreationTime,
                        CreatorUserId = input.CreatorUserId,
                        DataAlteracao = input.DataAlteracao,
                        DataAlteradoExame = input.DataAlteradoExame,
                        DataConferidoExame = input.DataConferidoExame,
                        DataDigitadoExame = input.DataDigitadoExame,
                        DataEnvioEmail = input.DataEnvioEmail,
                        DataExclusao = input.DataExclusao,
                        DataImporta = input.DataImporta,
                        DataImpressoExame = input.DataImpressoExame,
                        DataImpSolicita = input.DataImpSolicita,
                        DataInclusao = input.DataInclusao,
                        DataPendenteExame = input.DataPendenteExame,
                        DataUsuarioCienteExame = input.DataUsuarioCienteExame,
                        DeleterUserId = input.DeleterUserId,
                        DeletionTime = input.DeletionTime,
                        Descricao = input.Descricao,
                        FaturamentoItem = null,
                        FaturamentoItemId = input.FaturamentoItemId,
                        FaturamentoContaItem = null,
                        FaturamentoContaItemId = input.FaturamentoContaItemId,
                        FormataId = input.FormataId,
                        Id = input.Id,
                        ImpResultado = input.ImpResultado,
                        IsCienteExame = input.IsCienteExame,
                        IsDeleted = input.IsDeleted,
                        IsImprime = input.IsImprime,
                        IsSergioFranco = input.IsSergioFranco,
                        IsSigiloso = input.IsSigiloso,
                        IsSistema = input.IsSistema,
                        KitExameId = input.KitExameId,
                        LastModificationTime = input.LastModificationTime,
                        LastModifierUserId = input.LastModifierUserId,
                        MaqImpSolicita = input.MaqImpSolicita,
                        MaterialId = input.MaterialId,
                        Mneumonico = input.Mneumonico,
                        MotivoPendenteExame = input.MotivoPendenteExame,
                        Observacao = input.Observacao,
                        Quantidade = input.Quantidade,
                        ResultadoId = input.ResultadoId,
                        Seq = input.Seq,
                        TabelaId = input.TabelaId,
                        UsuarioAlteradoExameId = input.UsuarioAlteradoExameId,
                        UsuarioCienteExameId = input.UsuarioCienteExameId,
                        UsuarioConferidoExameId = input.UsuarioConferidoExameId,
                        UsuarioDigitadoExameId = input.UsuarioDigitadoExameId,
                        UsuarioImpressoExameId = input.UsuarioImpressoExameId,
                        UsuarioImpSolicitaId = input.UsuarioImpSolicitaId,
                        UsuarioIncluidoExameId = input.UsuarioIncluidoExameId,
                        UsuarioPendenteExameId = input.UsuarioPendenteExameId,
                        VolumeMaterial = input.VolumeMaterial,
                        ExameStatusId = input.ExameStatusId,
                        SolicitacaoExameId = input.SolicitacaoExameId
                    };

                    if (input.Id.Equals(0))
                    {
                        // using (var unitOfWork = _unitOfWorkManager.Begin())
                        // {
                        resultadoExame.ExameStatusId = (long)EnumStatusExame.Inicial;
                        resultadoExame.Id = await this._resultadoExameRepositorio.InsertAndGetIdAsync(resultadoExame)
                            .ConfigureAwait(false);

                        // unitOfWork.Complete();
                        // unitOfWork.Dispose();
                        this._unitOfWorkManager.Current.SaveChanges();

                        var exame = _resultadoExameRepositorio.GetAll()
                            .Include(x => x.FaturamentoItem)
                            .AsNoTracking().FirstOrDefault(x => x.Id == resultadoExame.Id);

                        input.Id = resultadoExame.Id;

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            ResultadoExameDto.OcorrenciaCriado(exame, GetCurrentUser().FullName, DateTime.Now),
                            TipoOcorrencia.ResultadoExame,
                            null,
                            typeof(ResultadoExame).FullName, input.Id, typeof(Resultado).FullName, input.ResultadoId));

                        // }
                    }
                    else
                    {
                        var ori = await this._resultadoExameRepositorio.GetAsync(input.Id).ConfigureAwait(false);
                        ori.Codigo = input.Codigo;
                        ori.CreationTime = input.CreationTime;
                        ori.CreatorUserId = input.CreatorUserId;
                        ori.DataAlteracao = input.DataAlteracao;
                        ori.DataAlteradoExame = input.DataAlteradoExame;
                        ori.DataConferidoExame = input.DataConferidoExame;
                        ori.DataDigitadoExame = input.DataDigitadoExame;
                        ori.DataEnvioEmail = input.DataEnvioEmail;
                        ori.DataExclusao = input.DataExclusao;
                        ori.DataImporta = input.DataImporta;
                        ori.DataImpressoExame = input.DataImpressoExame;
                        ori.DataImpSolicita = input.DataImpSolicita;
                        ori.DataInclusao = input.DataInclusao;
                        ori.DataPendenteExame = input.DataPendenteExame;
                        ori.DataUsuarioCienteExame = input.DataUsuarioCienteExame;
                        ori.DeleterUserId = input.DeleterUserId;
                        ori.DeletionTime = input.DeletionTime;
                        ori.Descricao = input.Descricao;

                        // ori.FaturamentoItem = null;
                        ori.FaturamentoItemId = input.FaturamentoItemId;

                        // ori.FaturamentoContaItem = null;
                        ori.FaturamentoContaItemId = input.FaturamentoContaItemId;
                        ori.FormataId = input.FormataId;
                        ori.ImpResultado = input.ImpResultado;
                        ori.IsCienteExame = input.IsCienteExame;
                        ori.IsDeleted = input.IsDeleted;
                        ori.IsImprime = input.IsImprime;
                        ori.IsSergioFranco = input.IsSergioFranco;
                        ori.IsSigiloso = input.IsSigiloso;
                        ori.IsSistema = input.IsSistema;
                        ori.KitExameId = input.KitExameId;
                        ori.LastModificationTime = input.LastModificationTime;
                        ori.LastModifierUserId = input.LastModifierUserId;
                        ori.MaqImpSolicita = input.MaqImpSolicita;
                        ori.MaterialId = input.MaterialId;
                        ori.Mneumonico = input.Mneumonico;
                        ori.MotivoPendenteExame = input.MotivoPendenteExame;
                        ori.Observacao = input.Observacao;
                        ori.Quantidade = input.Quantidade;
                        ori.ResultadoId = input.ResultadoId;
                        ori.Seq = input.Seq;
                        ori.TabelaId = input.TabelaId;
                        ori.UsuarioAlteradoExameId = input.UsuarioAlteradoExameId;
                        ori.UsuarioCienteExameId = input.UsuarioCienteExameId;
                        ori.UsuarioConferidoExameId = input.UsuarioConferidoExameId;
                        ori.UsuarioDigitadoExameId = input.UsuarioDigitadoExameId;
                        ori.UsuarioImpressoExameId = input.UsuarioImpressoExameId;
                        ori.UsuarioImpSolicitaId = input.UsuarioImpSolicitaId;
                        ori.UsuarioIncluidoExameId = input.UsuarioIncluidoExameId;
                        ori.UsuarioPendenteExameId = input.UsuarioPendenteExameId;
                        ori.VolumeMaterial = input.VolumeMaterial;

                        if (ori.ExameStatusId != 4)
                        {
                            await this._resultadoExameRepositorio.UpdateAsync(ori).ConfigureAwait(false);
                            this._unitOfWorkManager.Current.SaveChanges();

                            var exame = _resultadoExameRepositorio.GetAll()
                                .Include(x => x.FaturamentoItem)
                                .AsNoTracking().FirstOrDefault(x => x.Id == resultadoExame.Id);
                            await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                                ResultadoExameDto.OcorrenciaCriado(exame, GetCurrentUser().FullName, DateTime.Now),
                                TipoOcorrencia.ResultadoExame,
                                null,
                                typeof(ResultadoExame).FullName, input.Id, typeof(Resultado).FullName,
                                input.ResultadoId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ResultadoExameDto input)
        {
            try
            {
                await this._resultadoExameRepositorio.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }
        }

        public async Task<ListResultDto<ResultadoExame>> ListarTodos()
        {
            try
            {
                var query = await this._resultadoExameRepositorio.GetAllListAsync().ConfigureAwait(false);

                var resultadoExamesDto = query.MapTo<List<ResultadoExame>>();

                return new ListResultDto<ResultadoExame> { Items = resultadoExamesDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ResultadoExameIndexDto>> ListarIndex(ListarInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<ResultadoExame> resultadoExames = new List<ResultadoExame>();
            List<ResultadoExameIndexDto> resultadoExamesDtos = new List<ResultadoExameIndexDto>();
            try
            {
                long id = 0;
                long.TryParse(input.Id, out id);
                var query = this._resultadoExameRepositorio.GetAll().Include(m => m.UsuarioAlteradoExame)
                    .Include(m => m.UsuarioCienteExame).Include(m => m.UsuarioConferidoExame)
                    .Include(m => m.UsuarioDigitadoExame).Include(m => m.UsuarioImpressoExame)
                    .Include(m => m.UsuarioImpSolicita).Include(m => m.UsuarioIncluidoExame)
                    .Include(m => m.UsuarioPendenteExame).Include(m => m.Resultado)
                    .Include(m => m.Resultado.Atendimento).Include(m => m.Resultado.Atendimento.Empresa)
                    .Where(m => m.ResultadoId == id).WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                // .Select(m => new ResultadoExameIndexDto
                // {
                // Id = m.Id,
                // Mneumonico = m.Mneumonico,
                // DataColeta = m.Resultado.CreationTime,
                // NumeroExame = m.Exame.Codigo,
                // NomeExame = m.Exame.Descricao,
                // //UsuarioIncluidoId = m.UsuarioIncluidoExame.FullName,
                // DataIncluido = m.DataInclusao,
                // //UsuarioDigitadoId = m.UsuarioDigitadoExame.FullName,
                // DataDigitado = m.DataDigitadoExame,
                // //UsuarioConferidoId = m.UsuarioConferidoExame.FullName,
                // DataConferido = m.DataConferidoExame,
                // DataPendente = m.DataPendenteExame,
                // //UsuarioImpressoId = m.UsuarioImpressoExame.FullName,
                // DataImpresso = m.DataImpressoExame,
                // DataEnvioEmail = m.DataEnvioEmail
                // });
                contarTiposTabelaDominio = await query.CountAsync().ConfigureAwait(false);

                resultadoExames = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                    .ConfigureAwait(false);

                foreach (var item in resultadoExames)
                {
                    var resultado = new ResultadoExameIndexDto
                    {
                        Id = item.Id,
                        AtendimentoId = item.Resultado.AtendimentoId,
                        EmpresaId = item.Resultado.Atendimento?.EmpresaId,
                        Empresa = item.Resultado.Atendimento?.Empresa?.NomeFantasia,
                        DataColeta = item.Resultado.DataColeta,
                        DataConferido = item.DataConferidoExame,
                        DataDigitado = item.DataDigitadoExame,
                        DataEnvioEmail = item.DataEnvioEmail,
                        DataImpresso = item.DataImpressoExame,
                        DataIncluido = item.DataInclusao,
                        DataPendente = item.DataPendenteExame,
                        Mneumonico = item.Mneumonico,
                        NomeExame = item.FaturamentoItem?.Descricao,
                        NumeroExame = item.FaturamentoItem?.Codigo,
                        UsuarioConferidoId = item.UsuarioConferidoExame?.FullName,
                        UsuarioDigitadoId = item.UsuarioDigitadoExame?.FullName,
                        UsuarioImpressoId = item.UsuarioImpressoExame?.FullName,
                        UsuarioIncluidoId = item.UsuarioIncluidoExame?.FullName,
                        ExameStatusId = item.ExameStatusId,
                        ExameStatus = item.ExameStatus?.Descricao,
                        ExameStatusCor = item.ExameStatus?.Cor
                    };

                    resultadoExamesDtos.Add(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<ResultadoExameIndexDto>(contarTiposTabelaDominio, resultadoExamesDtos);
        }

        public async Task<PagedResultDto<ResultadoExameIndexCrudDto>> Listar(ListarInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<ResultadoExame> resultadoExames = new List<ResultadoExame>();
            List<ResultadoExameIndexCrudDto> resultadoExamesDtos = new List<ResultadoExameIndexCrudDto>();
            try
            {
                long id = 0;
                long.TryParse(input.Id, out id);
                var query = this._resultadoExameRepositorio.GetAll().Include(m => m.UsuarioAlteradoExame)
                    .Include(m => m.UsuarioCienteExame).Include(m => m.UsuarioConferidoExame)
                    .Include(m => m.UsuarioDigitadoExame).Include(m => m.UsuarioImpressoExame)
                    .Include(m => m.UsuarioImpSolicita).Include(m => m.UsuarioIncluidoExame)
                    .Include(m => m.UsuarioPendenteExame).Include(m => m.Resultado)
                    .Include(m => m.Resultado.Atendimento).Include(m => m.Resultado.Atendimento.Empresa)
                    .Where(m => m.ResultadoId == id).WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                contarTiposTabelaDominio = await query.CountAsync().ConfigureAwait(false);

                resultadoExames = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                    .ConfigureAwait(false);

                foreach (var item in resultadoExames)
                {
                    var resultado = new ResultadoExameIndexCrudDto
                    {
                        Id = item.Id,
                        DataConferido = item.DataConferidoExame,
                        DataDigitado = item.DataDigitadoExame,
                        DataPendente = item.DataPendenteExame,
                        Exame = item.FaturamentoItem?.Descricao,
                        UsuarioConferidoId = item.UsuarioConferidoExameId,
                        UsuarioDigitadoId = item.UsuarioDigitadoExameId,
                        UsuarioPendenteId = item.UsuarioPendenteExameId,
                        FaturamentoContaItemId = item.FaturamentoContaItemId,
                        FaturamentoContaItem = item.FaturamentoContaItem?.Descricao,
                        IsSigiloso = item.IsSigiloso,
                        MaterialId = item.MaterialId,
                        Material = item.Material?.Descricao,
                        MotivoPendenteExame = item.MotivoPendenteExame,
                        ObservacaoExame = item.Observacao,
                        Quantidade = item.Quantidade,
                        AtendimentoId = item.Resultado.AtendimentoId,
                        EmpresaId = item.Resultado.Atendimento?.EmpresaId,
                        Empresa = item.Resultado.Atendimento?.Empresa?.NomeFantasia,
                        ExameStatusId = item.ExameStatusId,
                        ExameStatus = item.ExameStatus?.Descricao,
                        ExameStatusCor = item.ExameStatus?.Cor
                    };

                    resultadoExamesDtos.Add(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<ResultadoExameIndexCrudDto>(contarTiposTabelaDominio, resultadoExamesDtos);
        }

        public async Task<ListResultDto<ResultadoExameIndexCrudDto>> ListarPorResultado(long id)
        {
            var resultadoExames = new List<ResultadoExame>();
            var resultadoExamesDtos = new List<ResultadoExameIndexCrudDto>();
            try
            {
                var query = this._resultadoExameRepositorio.GetAll().Include(m => m.UsuarioAlteradoExame)
                    .Include(m => m.UsuarioCienteExame).Include(m => m.UsuarioConferidoExame)
                    .Include(m => m.UsuarioDigitadoExame).Include(m => m.UsuarioImpressoExame)
                    .Include(m => m.UsuarioImpSolicita).Include(m => m.UsuarioIncluidoExame)
                    .Include(m => m.UsuarioPendenteExame).Include(m => m.Resultado)
                    .Include(m => m.Resultado.Atendimento).Include(m => m.Resultado.Atendimento.Empresa)
                    .Include(m => m.FaturamentoItem).Include(m => m.Material).Include(m => m.ExameStatus)
                    .Where(m => m.ResultadoId == id);

                resultadoExames = await query.ToListAsync().ConfigureAwait(false);

                long idGrid = 0;
                foreach (var item in resultadoExames)
                {
                    var resultado = new ResultadoExameIndexCrudDto
                    {
                        Id = item.Id,
                        DataConferido = item.DataConferidoExame,
                        DataDigitado = item.DataDigitadoExame,
                        DataPendente = item.DataPendenteExame,
                        FaturamentoItemId = item.FaturamentoItemId,
                        Exame = item.FaturamentoItem?.Descricao,
                        UsuarioConferidoId = item.UsuarioConferidoExameId,
                        UsuarioDigitadoId = item.UsuarioDigitadoExameId,
                        UsuarioPendenteId = item.UsuarioPendenteExameId,
                        FaturamentoContaItemId = item.FaturamentoContaItemId,
                        FaturamentoContaItem = item.FaturamentoContaItem?.Descricao,
                        IsSigiloso = item.IsSigiloso,
                        MaterialId = item.MaterialId,
                        Material = item.Material?.Descricao,
                        MotivoPendenteExame = item.MotivoPendenteExame,
                        ObservacaoExame = item.Observacao,
                        Quantidade = item.Quantidade,
                        CreatorUserId = item.CreatorUserId,
                        CreationTime = item.CreationTime,
                        IsSistema = item.IsSistema,
                        LastModificationTime = item.LastModificationTime,
                        LastModifierUserId = item.LastModifierUserId,
                        IdGridResultadoExame = ++idGrid,
                        Cor = item.ExameStatus.Cor,
                        ExameStatusId = item.ExameStatusId,
                        AtendimentoId = item.Resultado.AtendimentoId,
                        EmpresaId = item.Resultado.Atendimento?.EmpresaId,
                        Empresa = item.Resultado.Atendimento?.Empresa?.NomeFantasia
                    };
                    resultado.ExameStatusId = item.ExameStatusId;
                    resultado.ExameStatus = item.ExameStatus?.Descricao;
                    resultado.ExameStatusCor = item.ExameStatus?.Cor;

                    resultadoExamesDtos.Add(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }

            return new ListResultDto<ResultadoExameIndexCrudDto> { Items = resultadoExamesDtos };
        }

        public async Task<ResultadoExameDto> Obter(long id)
        {
            try
            {
                using (var userRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
                {
                    var query = this._resultadoExameRepositorio.GetAll()
                        .AsNoTracking()
                        .Include(x => x.Formata)
                        .Include(x => x.Material)
                        .Include(x => x.FaturamentoItem)
                        .Include(x => x.FaturamentoItem.Equipamento)
                        .Include(x => x.FaturamentoItem.Setor)
                        .Include(x => x.Tabela)
                        .Include(x => x.KitExame)
                        .Include(x => x.ExameStatus)
                        .Where(m => m.Id == id);

                    var exame = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                    var exameDto = exame != null ? ResultadoExameDto.Mapear(exame) : new ResultadoExameDto();
                    if (exameDto == null)
                    {
                        return exameDto;
                    }

                    if (exameDto.UsuarioColetaBaixaId.HasValue)
                    {
                        exameDto.UsuarioColetaBaixa =
                            userRepository.Object.FirstOrDefault(exameDto.UsuarioColetaBaixaId.Value);
                    }

                    if (exame.UsuarioConferidoExameId.HasValue)
                    {
                        exameDto.UsuarioConferidoExame =
                            userRepository.Object.FirstOrDefault(exameDto.UsuarioConferidoExameId.Value);
                    }

                    if (exameDto.UsuarioPendenteExameId.HasValue)
                    {
                        exameDto.UsuarioPendenteExame =
                            userRepository.Object.FirstOrDefault(exameDto.UsuarioPendenteExameId.Value);
                    }
                    
                    if (exameDto.PendenciaUserId.HasValue)
                    {
                        exameDto.UsuarioPendenteExame =
                            userRepository.Object.FirstOrDefault(exameDto.PendenciaUserId.Value);
                    }

                    if (exameDto.UsuarioImpressoExameId.HasValue)
                    {
                        exameDto.UsuarioImpressoExame =
                            userRepository.Object.FirstOrDefault(exameDto.UsuarioImpressoExameId.Value);
                    }

                    if (exameDto.UsuarioDigitadoExameId.HasValue)
                    {
                        exameDto.UsuarioDigitadoExame =
                            userRepository.Object.FirstOrDefault(exameDto.UsuarioDigitadoExameId.Value);
                    }

                    if (exameDto.CreatorUserId.HasValue)
                    {
                        exameDto.CreatorUser = userRepository.Object.FirstOrDefault(exameDto.CreatorUserId.Value);
                    }

                    return exameDto;
                }

                // var resultadoExameDto = new ResultadoExameDto
                // {
                //     // resultadoExame.MapTo<ResultadoExameDto>();
                //     Codigo = input.Codigo,
                //     CreationTime = input.CreationTime,
                //     CreatorUserId = input.CreatorUserId,
                //     DataAlteracao = input.DataAlteracao,
                //     DataAlteradoExame = input.DataAlteradoExame,
                //     DataConferidoExame = input.DataConferidoExame,
                //     DataDigitadoExame = input.DataDigitadoExame,
                //     DataEnvioEmail = input.DataEnvioEmail,
                //     DataExclusao = input.DataExclusao,
                //     DataImporta = input.DataImporta,
                //     DataImpressoExame = input.DataImpressoExame,
                //     DataImpSolicita = input.DataImpSolicita,
                //     DataInclusao = input.DataInclusao,
                //     DataPendenteExame = input.DataPendenteExame,
                //     DataUsuarioCienteExame = input.DataUsuarioCienteExame,
                //     DeleterUserId = input.DeleterUserId,
                //     DeletionTime = input.DeletionTime,
                //     Descricao = input.Descricao,
                //     ExameStatusId = input.ExameStatusId,
                //     FaturamentoItemId = input.FaturamentoItemId,
                //     FaturamentoContaItemId = input.FaturamentoContaItemId,
                //     FormataId = input.FormataId,
                //     Id = input.Id,
                //     ImpResultado = input.ImpResultado,
                //     IsCienteExame = input.IsCienteExame,
                //     IsDeleted = input.IsDeleted,
                //     IsImprime = input.IsImprime,
                //     IsSergioFranco = input.IsSergioFranco,
                //     IsSigiloso = input.IsSigiloso,
                //     IsSistema = input.IsSistema,
                //     KitExameId = input.KitExameId,
                //     LastModificationTime = input.LastModificationTime,
                //     LastModifierUserId = input.LastModifierUserId,
                //     MaqImpSolicita = input.MaqImpSolicita,
                //     MaterialId = input.MaterialId,
                //     Mneumonico = input.Mneumonico,
                //     MotivoPendenteExame = input.MotivoPendenteExame,
                //     Observacao = input.Observacao,
                //     Quantidade = input.Quantidade,
                //     ResultadoId = input.ResultadoId,
                //     Seq = input.Seq,
                //     TabelaId = input.TabelaId,
                //     UsuarioAlteradoExameId = input.UsuarioAlteradoExameId,
                //     UsuarioCienteExameId = input.UsuarioCienteExameId,
                //     UsuarioConferidoExameId = input.UsuarioConferidoExameId,
                //     UsuarioDigitadoExameId = input.UsuarioDigitadoExameId,
                //     UsuarioImpressoExameId = input.UsuarioImpressoExameId,
                //     UsuarioImpSolicitaId = input.UsuarioImpSolicitaId,
                //     UsuarioIncluidoExameId = input.UsuarioIncluidoExameId,
                //     UsuarioPendenteExameId = input.UsuarioPendenteExameId,
                //     VolumeMaterial = input.VolumeMaterial
                // };
                //
                // if (resultadoExameDto.FaturamentoItemId.HasValue)
                // {
                //     resultadoExameDto.FaturamentoItem = this._faturamentoItemRepository.GetAll()
                //         .FirstOrDefault(w => w.Id == resultadoExameDto.FaturamentoItemId).MapTo<FaturamentoItemDto>();
                //
                //     // .Obter(resultadoExameDto.FaturamentoItemId.Value);
                // }
                //
                // // if (resultadoExameDto.FaturamentoContaItemId.HasValue)
                // // {
                // // resultadoExameDto.FaturamentoContaItem = await _fatContaItemAppService.Obter(resultadoExameDto.FaturamentoContaItemId.Value);
                // // }
                // if (resultadoExameDto.FormataId.HasValue)
                // {
                //     resultadoExameDto.Formata = await this._formataAppService.Obter(resultadoExameDto.FormataId.Value)
                //                                     .ConfigureAwait(false);
                // }
                //
                // if (resultadoExameDto.MaterialId.HasValue)
                // {
                //     resultadoExameDto.Material = await this._materialAppService
                //                                      .Obter(resultadoExameDto.MaterialId.Value).ConfigureAwait(false);
                // }
                //
                // if (resultadoExameDto.TabelaId.HasValue)
                // {
                //     resultadoExameDto.Tabela = await this._tabelaAppService.Obter(resultadoExameDto.TabelaId.Value)
                //                                    .ConfigureAwait(false);
                // }
                //
                // if (resultadoExameDto.KitExameId.HasValue)
                // {
                //     resultadoExameDto.KitExame = await this._kitExameAppService
                //                                      .Obter(resultadoExameDto.KitExameId.Value).ConfigureAwait(false);
                // }
                //
                // return resultadoExameDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ExameStatusDto>> Legenda()
        {
            try
            {
                var query = await this._exameStatusRepositorio.GetAllListAsync().ConfigureAwait(false);

                var exameStatusDto = new List<ExameStatusDto>();
                foreach (var item in query)
                {
                    exameStatusDto.Add(ExameStatusDto.Mapear(item));
                }

                return new ListResultDto<ExameStatusDto> { Items = exameStatusDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await this._resultadoExameRepositorio.GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m => m.Descricao.Contains(input))
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao }).ToListAsync()
                    .ConfigureAwait(false);

                var ResultadoExames = new ListResultDto<GenericoIdNome> { Items = query };

                List<ResultadoExameDto> ResultadoExamesList = new List<ResultadoExameDto>();

                return ResultadoExames;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarInput input)
        {
            try
            {
                var result = await this.ListarIndex(input).ConfigureAwait(false);
                var ResultadoExames = result.Items;
                return this._listarResultadoExamesExcelExporter.ExportToFile(ResultadoExames.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<ResultadoExameDto> pacientesDtos = new List<ResultadoExameDto>();
            try
            {
                // get com filtro
                var query = from p in this._resultadoExameRepositorio.GetAll().WhereIf(
                        !dropdownInput.search.IsNullOrEmpty(),
                        m => m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())

                        // ||
                        // m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                    )
                    orderby p.Descricao ascending
                    select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };

                // paginação 
                var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                var total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ResultadoExameIndexCrudDto>> ListarJson(List<ResultadoExameIndexCrudDto> list)
        {
            try
            {
                var result = new PagedResultDto<ResultadoExameIndexCrudDto>(list.Count(), list);
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ResultadoExameIndexCrudDto>> ListarNaoConferidos(
            ListarResultadoExamesInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<ResultadoExame> resultadoExames = new List<ResultadoExame>();
            List<ResultadoExameIndexCrudDto> resultadoExamesDtos = new List<ResultadoExameIndexCrudDto>();
            try
            {
                var query = this._resultadoExameRepositorio.GetAll().Include(m => m.Resultado)
                    .Include(m => m.FaturamentoItem).Include(m => m.ExameStatus).Where(
                        m => m.ResultadoId == input.ColetaId

                        // && m.UsuarioConferidoExameId == null
                    ).WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                contarTiposTabelaDominio = await query.CountAsync().ConfigureAwait(false);

                resultadoExames = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                    .ConfigureAwait(false);

                foreach (var item in resultadoExames)
                {
                    var resultado = new ResultadoExameIndexCrudDto
                    {
                        Id = item.Id,
                        DataConferido = item.DataConferidoExame,
                        DataDigitado = item.DataDigitadoExame,
                        DataPendente = item.DataPendenteExame,
                        FaturamentoItemId = item.FaturamentoItemId,
                        Exame = item.FaturamentoItem?.Descricao,
                        UsuarioConferidoId = item.UsuarioConferidoExameId,
                        UsuarioDigitadoId = item.UsuarioDigitadoExameId,
                        UsuarioPendenteId = item.UsuarioPendenteExameId,
                        FaturamentoContaItemId = item.FaturamentoContaItemId,
                        FaturamentoContaItem = item.FaturamentoContaItem?.Descricao,
                        IsSigiloso = item.IsSigiloso,
                        MaterialId = item.MaterialId,
                        Material = item.Material?.Descricao,
                        MotivoPendenteExame = item.MotivoPendenteExame,
                        ObservacaoExame = item.Observacao,
                        Quantidade = item.Quantidade,
                        Codigo = item.Resultado.Numero,
                        ExameStatusId = item.ExameStatusId,
                        Cor = item.ExameStatus.Cor
                    };
                    resultadoExamesDtos.Add(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<ResultadoExameIndexCrudDto>(contarTiposTabelaDominio, resultadoExamesDtos);
        }

        public async Task<DefaultReturn<ResultadoExameIndexCrudDto>> RegistrarConferenciaExames(long[] examesIds)
        {
            var retornoPadrao = new DefaultReturn<ResultadoExameIndexCrudDto>
            {
                ReturnObject = new ResultadoExameIndexCrudDto(),
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };

            using (var unitOfWork = this._unitOfWorkManager.Begin())
            {
                foreach (var item in examesIds)
                {
                    var resultadoExame = this._resultadoExameRepositorio.GetAll().FirstOrDefault(w => w.Id == item);

                    if (resultadoExame != null)
                    {
                        resultadoExame.ExameStatusId = (long)EnumStatusExame.Conferido;
                        resultadoExame.DataConferidoExame = DateTime.Now;
                        resultadoExame.UsuarioConferidoExameId = this.AbpSession.UserId;
                    }
                }

                unitOfWork.Complete();
                this._unitOfWorkManager.Current.SaveChanges();
                unitOfWork.Dispose();
            }

            return retornoPadrao;
        }


        public async Task<IEnumerable<ResultadoExameDto>> ObterResultadoExames(long resultadoId,
            List<long> resultadoExameIds = null)
        {
            using (var resultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            {
                return ResultadoExameDto.Mapear(await resultadoExameRepository.Object.GetAll()
                    .AsNoTracking()
                    .Include(x => x.FaturamentoItem)
                    .Include(x => x.Material)
                    .Include(x => x.ExameStatus)
                    .Include(x => x.FaturamentoItem.Equipamento)
                    .Include(x => x.FaturamentoItem.Setor)
                    .Where(x => x.ResultadoId == resultadoId)
                    .WhereIf(!resultadoExameIds.IsNullOrEmpty(), x => resultadoExameIds.Contains(x.Id))
                    .ToListAsync());
            }
        }

        public async Task<IEnumerable<ResultadoExameDto>> ObterResultadoExamesPorSolicitacaoExames(long resultadoId,
            List<long> solicitacaoExameIds = null)
        {
            using (var resultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            {
                return ResultadoExameDto.Mapear(await resultadoExameRepository.Object.GetAll()
                    .AsNoTracking()
                    .Include(x => x.FaturamentoItem)
                    .Include(x => x.Material)
                    .Include(x => x.ExameStatus)
                    .Include(x => x.FaturamentoItem.Equipamento)
                    .Include(x => x.FaturamentoItem.Setor)
                    .Where(x => x.ResultadoId == resultadoId)
                    .WhereIf(!solicitacaoExameIds.IsNullOrEmpty(),
                        x => x.SolicitacaoExameId.HasValue && solicitacaoExameIds.Contains(x.SolicitacaoExameId.Value))
                    .ToListAsync());
            }
        }

        public async Task<DefaultReturn<object>> RegistrarBaixa(long resultadoId, long? tecnicoId,
            List<LabResultadoExameBaixaInputDto> resultadoExamesBaixa)
        {
            using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            using (var tecnicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Tecnico, long>>())
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var resultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            {
                var exameIds = resultadoExamesBaixa.Select(x => x.ResultadoExameId);
                var result = new DefaultReturn<object>();
                var exames = await resultadoExameRepository.Object.GetAll()
                    .Include(x => x.FaturamentoItem)
                    .Include(x => x.Material)
                    .Include(x => x.ExameStatus)
                    .Where(x => x.ResultadoId == resultadoId)
                    .Where(x => exameIds.Contains(x.Id)).ToListAsync();
                var ExamestatusCondition = new List<long> { ExameStatusDto.Inicial, ExameStatusDto.EmColeta };
                if (exames.Any(x => x.ExameStatusId.HasValue && !ExamestatusCondition.Contains(x.ExameStatusId.Value)))
                {
                    result.Errors ??= new List<ErroDto>();

                    result.Errors.AddRange(exames
                        .Where(x => x.ExameStatusId.HasValue && !ExamestatusCondition.Contains(x.ExameStatusId.Value))
                        .Select(x =>
                            ErroDto.Criar(string.Empty,
                                $@"Exame: <b>{x.FaturamentoItem.Descricao}</b> não pode ser dado baixa.
                            <br/>O Exame se encontra no status <b> {x.ExameStatus.Descricao}</b> é necessário que ele esteja em <b>Inicial</b> ou <b> Em Coleta</b>.")));
                    if (!result.Errors.IsNullOrEmpty())
                    {
                        return result;
                    }
                }

                var currentUser = await GetCurrentUserAsync();
                var currentDate = DateTime.Now;
                var tecnico = tecnicoRepository.Object.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == tecnicoId);
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var exame in exames.Where(x =>
                        x.ExameStatusId.HasValue && ExamestatusCondition.Contains(x.ExameStatusId.Value)))
                    {
                        exame.DataColetaBaixa = DateTime.Now;
                        exame.UsuarioColetaBaixaId = currentUser.Id;
                        exame.TecnicoColetaId = tecnicoId;
                        exame.MaterialDescricaoLocal = resultadoExamesBaixa
                            .FirstOrDefault(x => x.ResultadoExameId == exame.Id)?.MaterialDescricaoLocal;
                        exame.Observacao = resultadoExamesBaixa.FirstOrDefault(x => x.ResultadoExameId == exame.Id)
                            ?.Observacao;
                        exame.ExameStatusId = ExameStatusDto.Coletado;

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(currentDate,
                            ResultadoExameDto.OcorrenciaDarBaixaExame(exame, currentUser.FullName, tecnico?.Descricao,
                                currentDate),
                            TipoOcorrencia.ResultadoExame,
                            null,
                            typeof(ResultadoExame).FullName, exame.Id, typeof(Resultado).FullName, exame.ResultadoId));
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                await resultadoAppService.Object.AtualizaStatus(resultadoId);
                return result;
            }
        }

        public async Task AdicionarPendencias(long resultadoId,
            List<LabResultadoExameBaixaInputDto> resultadoExamesPendencias)
        {
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var resultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            {
                var exameIds = resultadoExamesPendencias.Select(x => x.ResultadoExameId);
                var exames = await resultadoExameRepository.Object.GetAll()
                    .Include(x => x.FaturamentoItem)
                    .Include(x => x.Material)
                    .Include(x => x.ExameStatus)
                    .Where(x => x.ResultadoId == resultadoId)
                    .Where(x => exameIds.Contains(x.Id)).ToListAsync();

                var currentUser = await GetCurrentUserAsync();
                var currentDate = DateTime.Now;
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var exame in exames)
                    {
                        exame.PendenciaDateTime = currentDate;
                        exame.PendenciaUserId = currentUser.Id;
                        exame.MotivoPendencia = resultadoExamesPendencias
                            .FirstOrDefault(x => x.ResultadoExameId == exame.Id)?.Observacao;
                        exame.IsPendencia = true;
                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(currentDate,
                            ResultadoExameDto.OcorrenciaCriarPendencia(exame, currentUser.FullName, currentDate),
                            TipoOcorrencia.ResultadoExame,
                            null,
                            typeof(ResultadoExame).FullName, exame.Id, typeof(Resultado).FullName, exame.ResultadoId));
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
        }

        public async Task<DefaultReturn<object>> VoltarStatusAnterior(long resultadoId, List<long> resultadoExameIds)
        {
            using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var resultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var exameStatusRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ExameStatus, long>>())
            {
                var result = new DefaultReturn<object>();
                var exames = await resultadoExameRepository.Object.GetAll()
                    .Include(x => x.FaturamentoItem)
                    .Include(x => x.Material)
                    .Include(x => x.ExameStatus)
                    .Where(x => x.ResultadoId == resultadoId)
                    .Where(x => resultadoExameIds.Contains(x.Id)).ToListAsync();

                var exameStatus = await exameStatusRepository.Object.GetAll().AsNoTracking().ToListAsync();
                var currentUser = await GetCurrentUserAsync();
                //TODO: Ver quais status não podem voltar;
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var exame in exames)
                    {
                        var statusAnterior = exame.ExameStatusId;

                        switch (exame.ExameStatusId)
                        {
                            case ExameStatusDto.Inicial:
                            {
                                continue;
                                break;
                            }
                            case ExameStatusDto.EmColeta:
                            {
                                exame.ExameStatusId = ExameStatusDto.Inicial;
                                break;
                            }
                            case ExameStatusDto.Coletado:
                            {
                                exame.ExameStatusId = ExameStatusDto.EmColeta;
                                exame.DataColetaBaixa = null;
                                exame.TecnicoColetaId = null;
                                break;
                            }
                            //TODO: Fazer os outros status;
                        }

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            ResultadoExameDto.OcorrenciaVoltarStatus(exame,
                                exameStatus.FirstOrDefault(x => x.Id == statusAnterior)?.Descricao,
                                exameStatus.FirstOrDefault(x => x.Id == exame.ExameStatusId)?.Descricao,
                                currentUser.FullName, DateTime.Now),
                            TipoOcorrencia.ResultadoExame,
                            null,
                            typeof(ResultadoExame).FullName, exame.Id, typeof(Resultado).FullName, exame.ResultadoId));
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
                
                await resultadoAppService.Object.AtualizaStatus(resultadoId);

                return result;
            }
        }

        public async Task<DefaultReturn<object>> AdicionarExameColeta(long resultadoId, long solicitacaoExameId,
            List<long> solicitacaoExameIds)
        {
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var resultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Resultado, long>>())
            using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            using (var resultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var solicitacaoExameAppService =
                IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
            using (var faturamentoItemRepositorio =
                IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            using (var faturamentoContaItemAppService =
                IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var exameStatusRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ExameStatus, long>>())
            {
                var result = new DefaultReturn<object>();
                result.Errors ??= new List<ErroDto>();

                var solicitacao = await solicitacaoExameAppService.Object.Obter(solicitacaoExameId);
                var resultadoExames = await resultadoExameRepository.Object.GetAll().Include(x => x.FaturamentoItem)
                    .AsNoTracking()
                    .Where(x => x.Id == resultadoId).ToListAsync();
                var resultado = await resultadoRepository.Object.GetAll().FirstOrDefaultAsync(x => x.Id == resultadoId);
                if (solicitacao == null)
                {
                    result.Errors.Add(ErroDto.Criar("",
                        "Não foi possível adicionar os exames na coleta pois a solicitação não foi encontrada."));
                    return result;
                }

                if (resultadoExames.Any(x =>
                    x.SolicitacaoExameId.HasValue && solicitacaoExameIds.Contains(x.SolicitacaoExameId.Value)))
                {
                    result.Errors.AddRange(resultadoExames.Where(x =>
                            x.SolicitacaoExameId.HasValue && solicitacaoExameIds.Contains(x.SolicitacaoExameId.Value))
                        .Select(x => ErroDto.Criar("",
                            $"Exame: <b>{x.FaturamentoItem.Descricao}</b> não foi possível ser adicionado na coleta pois ele já se encontra nela.")));
                    return result;
                }

                var solicitacaoIQ = solicitacao.SolicitacaoItens.Where(x => solicitacaoExameIds.Contains(x.Id));
                var faturamentoItemIds = solicitacaoIQ.Select(x => x.FaturamentoItemId);
                var faturamentoItems = await faturamentoItemRepositorio.Object.GetAll()
                    .Where(x => faturamentoItemIds.Contains(x.Id)).ToListAsync();
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var solicitacaoItem in solicitacaoIQ.Select(x =>
                        ResultadoExameIndexCrudDto.Mapear(x, solicitacao.AtendimentoId)))
                    {
                        try
                        {
                            var faturamentoItem =
                                faturamentoItems.FirstOrDefault(x => x.Id == solicitacaoItem.FaturamentoItemId);
                            var exame = new ResultadoExameDto
                            {
                                Codigo = solicitacaoItem.Codigo,
                                CreationTime = solicitacaoItem.CreationTime,
                                CreatorUserId = solicitacaoItem.CreatorUserId,
                                FaturamentoItemId = solicitacaoItem.FaturamentoItemId,
                                FaturamentoContaItemId = solicitacaoItem.FaturamentoContaItemId,
                                Id = solicitacaoItem.Id,
                                IsDeleted = solicitacaoItem.IsDeleted,
                                IsSigiloso = solicitacaoItem.IsSigiloso,
                                IsSistema = solicitacaoItem.IsSistema,
                                LastModificationTime = solicitacaoItem.LastModificationTime,
                                LastModifierUserId = solicitacaoItem.LastModifierUserId,
                                MaterialId = solicitacaoItem.MaterialId,
                                MotivoPendenteExame = solicitacaoItem.MotivoPendenteExame,
                                Observacao = solicitacaoItem.ObservacaoExame,
                                Quantidade = solicitacaoItem.Quantidade,
                                UsuarioConferidoExameId = solicitacaoItem.UsuarioConferidoId,
                                UsuarioDigitadoExameId = solicitacaoItem.UsuarioDigitadoId,
                                UsuarioPendenteExameId = solicitacaoItem.UsuarioPendenteId,
                                ResultadoId = resultado.Id,
                                FormataId = faturamentoItem?.FormataId,
                                ExameStatusId = (long)EnumStatusExame.Inicial,
                                SolicitacaoExameId = solicitacaoItem.SolicitacaoItemId
                            };
                            await this.CriarOuEditar(exame);
                            // CarregarFaturamentoContaItem(ResultadoExameDto.Mapear(exame));
                            var _exame = ResultadoExameDto.Mapear(exame);
                            _exame.FaturamentoItem = faturamentoItem;
                            ResultadoAppService.CarregarFaturamentoContaItem(_exame, resultado);

                            // salvar os itens de fatconta
                            var faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto
                            {
                                AtendimentoId = resultado.AtendimentoId ?? 0,
                                Data = resultado.DataColeta,
                                ItensFaturamento = new List<FaturamentoContaItemDto>()
                            };

                            var item = exame.FaturamentoContaItem ??
                                       new FaturamentoContaItemDto(); // FaturamentoContaItemDto.MapearFromCore(_exame.FaturamentoContaItem);
                            item.Id = exame.FaturamentoItemId.Value;
                            faturamentoContaItemInsertDto.ItensFaturamento.Add(item);

                            await faturamentoContaItemAppService.Object
                                .InserirItensContaFaturamento(faturamentoContaItemInsertDto).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            result.Errors.Add(ErroDto.Criar("",
                                $"Não foi possível adicionar o exame {solicitacaoItem.FaturamentoContaItem}."));
                            Logger.Error(
                                $" ResultadoExameAppService[AdicionarExameColeta]: Não foi possível adicionar o exame {solicitacaoItem.FaturamentoContaItem}.",
                                e);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                return result;
            }
        }

        public async Task AtualizarObservacao(long resultadoExameId, string observacao)
        {
            using (var resultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var exame = await resultadoExameRepository.Object.FirstOrDefaultAsync(resultadoExameId);
                exame.Observacao = observacao;
                
                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();
                
            }
        }

        public async Task ResolverPendencias(long resultadoId, List<long> resultadoExameIds)
        {
            if (resultadoExameIds.IsNullOrEmpty())
            {
                return;
            }
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            using (var resultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var currentDate = DateTime.Now;
                var currentUser = await GetCurrentUserAsync();
                foreach (var resultadoExame in resultadoExameRepository.Object.GetAll().Include(x=> x.FaturamentoItem).Where(x=> resultadoExameIds.Contains(x.Id) && x.IsPendencia))
                {
                    resultadoExame.IsPendencia = false;
                    await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(currentDate,
                        ResultadoExameDto.OcorrenciaResolverPendencia(resultadoExame, currentUser.FullName, currentDate),
                        TipoOcorrencia.ResultadoExame,
                        null,
                        typeof(ResultadoExame).FullName, resultadoExame.Id, typeof(Resultado).FullName,
                        resultadoExame.ResultadoId));
                    
                }
                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();
            }
        }


        public async Task ImprimirEtiquetas(LabImprimirEtiquetaInputDto input)
        {
            using (var exameStatusRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ExameStatus, long>>())
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            using (var resultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            using (var impressoraArquivosAppService =
                IocManager.Instance.ResolveAsDisposable<IImpressoraArquivosAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                var exameStatus =
                    ExameStatusDto.Mapear(await exameStatusRepository.Object.GetAll().AsNoTracking().ToListAsync());
                var currentUser = await GetCurrentUserAsync();
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var labEtiqueta in input.Etiquetas)
                    {
                        var file = this.CreateJasperReport("/Laboratorio/Etiquetas")
                            .SetMethod(Method.POST)
                            .AddParameter("SetorId", labEtiqueta.SetorId.ToString())
                            .AddParameter("ResultadoId", input.ResultadoId.ToString())
                            .AddParameter("ResultadoExameIds",
                                !labEtiqueta.ResultadoExameIds.IsNullOrEmpty()
                                    ? string.Join(",", labEtiqueta.ResultadoExameIds)
                                    : "")
                            .AddParameter("Qtd", labEtiqueta.Qtd.ToString())
                            .AddParameter("Modelo", labEtiqueta.Modelo)
                            .AddParameter("Dominio", this.GetConnectionStringName())
                            .GenerateReport();
                        if (file.Length == 0)
                        {
                            continue;
                        }

                        impressoraArquivosAppService.Object.EnviarParaImprimir(input.Impressora, file,
                            $"Lab_Etiqueta_{Guid.NewGuid().ToString()}_{labEtiqueta.Modelo}_{labEtiqueta.Qtd}.pdf");
                        var currentDate = DateTime.Now;
                        if (labEtiqueta.ResultadoExameIds.IsNullOrEmpty())
                        {
                            continue;
                        }

                        var iqResultadoExame = resultadoExameRepository.Object.GetAll()
                            .Include(x => x.FaturamentoItem)
                            .Where(x => labEtiqueta.ResultadoExameIds.Contains(x.Id));

                        foreach (var resultadoExame in iqResultadoExame)
                        {
                            if (resultadoExame.ExameStatusId == ExameStatusDto.Inicial)
                            {
                                var statusAnterior =
                                    exameStatus.FirstOrDefault(x => x.Id == resultadoExame.ExameStatusId);
                                resultadoExame.ExameStatusId = ExameStatusDto.EmColeta;
                                var statusAtual = exameStatus.FirstOrDefault(x => x.Id == resultadoExame.ExameStatusId);
                                await resultadoExameRepository.Object.UpdateAsync(resultadoExame);
                                await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(currentDate,
                                    ResultadoExameDto.OcorrenciaVoltarStatus(resultadoExame, statusAnterior?.Descricao,
                                        statusAtual?.Descricao, currentUser.FullName, currentDate),
                                    TipoOcorrencia.ResultadoExame,
                                    null,
                                    typeof(ResultadoExame).FullName, resultadoExame.Id, typeof(Resultado).FullName,
                                    resultadoExame.ResultadoId));
                            }

                            await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(currentDate,
                                ResultadoExameDto.OcorrenciaColetaImpressa(resultadoExame, currentUser.FullName,
                                    currentDate),
                                TipoOcorrencia.ResultadoExame,
                                null,
                                typeof(ResultadoExame).FullName, resultadoExame.Id, typeof(Resultado).FullName,
                                resultadoExame.ResultadoId));
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
        }
    }
}