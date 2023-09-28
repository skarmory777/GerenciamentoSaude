#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaLotes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Interfaces;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helper;
using SW10.SWMANAGER.Helpers;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas
{
    public class FaturamentoEntregaLoteAppService : SWMANAGERAppServiceBase, IFaturamentoEntregaLoteAppService
    {
        #region Dependencias
        private readonly IRepository<FaturamentoEntregaLote, long> _loteRepository;
        private readonly IRepository<FaturamentoEntregaConta, long> _entregaContaRepository;
        private readonly IRepository<FaturamentoConta, long> _contaRepository;
        private readonly IRepository<FaturamentoContaItem, long> _contaItemRepository;
        private readonly IListarFaturamentoEntregaLotesExcelExporter _listarEntregaLotesExcelExporter;

        private readonly IFaturamentoGrupoAppService _faturamentoGrupoAppService;
        private readonly IFaturamentoItemAppService _faturamentoItemAppService;
        private readonly IContaAppService _fatContaAppService;
        private readonly ITISSAppService _TISSAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly ISessionAppService _sessionService;

        public FaturamentoEntregaLoteAppService(
            IRepository<FaturamentoEntregaLote, long> loteRepository,
            IRepository<FaturamentoEntregaConta, long> entregaContaRepository,
            IRepository<FaturamentoConta, long> contaRepository,
            IRepository<FaturamentoContaItem, long> contaItemRepository,
            IListarFaturamentoEntregaLotesExcelExporter listarEntregaLotesExcelExporter,
            IFaturamentoGrupoAppService faturamentoGrupoAppService,
            IFaturamentoItemAppService faturamentoItemAppService,
            IContaAppService fatContaAppService,
            ITISSAppService TISSAppService,
            IUnitOfWorkManager unitOfWorkManager,
            ISessionAppService sessionService
            )
        {
            _loteRepository = loteRepository;
            _entregaContaRepository = entregaContaRepository;
            _contaRepository = contaRepository;
            _contaItemRepository = contaItemRepository;
            _listarEntregaLotesExcelExporter = listarEntregaLotesExcelExporter;
            _faturamentoGrupoAppService = faturamentoGrupoAppService;
            _faturamentoItemAppService = faturamentoItemAppService;
            _fatContaAppService = fatContaAppService;
            _TISSAppService = TISSAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _sessionService = sessionService;
        }
        #endregion dependencias.


        public async Task<PagedResultDto<FaturamentoEntregaLoteListOutputDto>> ListarLotes(FaturamentoEntregaLoteInputDto input)
        {
            const string DefaultField = "FatEntregaLote.Id";

            const string SelectClause = @" 
                FatEntregaLote.Id,
                SisEmpresa.Id AS EmpresaId,
                SisEmpresa.NomeFantasia AS EmpresaNomeFantasia,
                SisConvenio.Id AS ConvenioId,
                SisConvenio.NomeFantasia AS ConvenioNomeFantasia,
                FatEntregaLote.CodEntregaLote,
                FatEntregaLote.NumeroProcesso,
                FatEntregaLote.DataInicial,
                FatEntregaLote.DataFinal,
                FatEntregaLote.DataEntrega,
                FatEntregaLote.IsAmbulatorio,
                FatEntregaLote.IsInternacao,
                FatEntregaLote.ValorFatura,
                COALESCE(FatEntregaContaTotal.TotalContas,0) TotalContas";

            const string FromClause = @"FatEntregaLote
                    LEFT JOIN SisEmpresa (NOLOCK) ON FatEntregaLote.SisEmpresaId = SisEmpresa.Id AND SisEmpresa.IsDeleted = @deleted
                    LEFT JOIN SisConvenio (NOLOCK) ON FatEntregaLote.SisConvenioId = SisConvenio.Id AND SisConvenio.IsDeleted = @deleted
                    LEFT JOIN (
                        SELECT  COUNT(FatEntregaConta.Id) as TotalContas, FatEntregaLoteId 
                        FROM FatEntregaConta (NOLOCK) GROUP BY FatEntregaLoteId) 
                    AS FatEntregaContaTotal ON FatEntregaContaTotal.FatEntregaLoteId = FatEntregaLote.Id ";

            const string WhereClause = " FatEntregaLote.IsDeleted = @deleted ";


            return await this.CreateDataTable<FaturamentoEntregaLoteListOutputDto, FaturamentoEntregaLoteInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .AddWhereMethod(ExecutaFiltroListarLotes)
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }


        public async Task<PagedResultDto<FaturamentoContaLoteDto>> ListarContasPorLote(FaturamentoEntregaLoteListarContasPorLoteInputDto input)
        {
            const string DefaultField = "FatEntregaConta.Id";

            const string SelectClause = @"
                FatEntregaConta.Id,
                FatEntregaConta.FatContaMedicaId AS FatContaMedicaId,
                AteAtendimento.Id as AtendimentoId,
                FatConta.Codigo,
                FatConta.DataInicio AS DataInicial,
                FatConta.DataFim AS DataFinal,
                AteAtendimento.DataRegistro,
                AteAtendimento.DataAlta,
                AteAtendimento.Matricula,
                AteAtendimento.NumeroGuia,
                SisPessoa.NomeCompleto AS PacienteNomeCompleto,
                SisPessoaMedico.NomeCompleto AS MedicoNomeCompleto,
                SisConvenio.Id AS ConvenioId,
                SisConvenio.NomeFantasia AS ConvenioNomeFantasia,
                SisPlano.Id AS PlanoId,
                SisPlano.Descricao AS PlanoDescricao,
                AteAtendimento.IsAmbulatorioEmergencia,
                FatContaStatus.Descricao AS FatContaStatusDescricao,
                FatEntregaConta.ValorConta";

            const string FromClause = @"FatEntregaConta
                INNER JOIN FatConta (NOLOCK) ON FatEntregaConta.FatContaMedicaId = FatConta.Id
                INNER JOIN AteAtendimento (NOLOCK) ON AteAtendimento.Id = FatConta.SisAtendimentoId AND AteAtendimento.IsDeleted = @isDeleted
                INNER JOIN SisPaciente (NOLOCK) ON SisPaciente.Id =FatConta.SisPacienteId AND SisPaciente.IsDeleted = @isDeleted
                INNER JOIN SisPessoa (NOLOCK) ON SisPessoa.Id = SisPaciente.SisPessoaId AND SisPessoa.IsDeleted = @isDeleted
                INNER JOIN FatContaStatus (NOLOCK) ON FatContaStatus.Id = FatConta.FatContaStatusId AND FatContaStatus.IsDeleted = @isDeleted
                LEFT JOIN SisConvenio (NOLOCK) ON SisConvenio.Id = AteAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @isDeleted
                LEFT JOIN SisPlano (NOLOCK) ON SisPlano.Id = AteAtendimento.SisPlanoId AND SisPlano.IsDeleted = @isDeleted
                LEFT JOIN SisProfissionalSaude (NOLOCK) ON SisProfissionalSaude.Id = AteAtendimento.SisMedicoId AND SisProfissionalSaude.IsDeleted = @isDeleted
                LEFT JOIN SisPessoa AS SisPessoaMedico (NOLOCK) ON SisPessoaMedico.Id = SisProfissionalSaude.SisPessoaId AND SisPessoaMedico.IsDeleted = @isDeleted
                LEFT JOIN SisGuia (NOLOCK) ON SisGuia.Id = AteAtendimento.SisGuiaId AND SisGuia.IsDeleted = @isDeleted";

            const string WhereClause = @"FatConta.IsAtivo = @isAtivo AND FatConta.IsDeleted = @isDeleted AND FatEntregaConta.FatEntregaLoteId = @LoteId";

            return await this.CreateDataTable<FaturamentoContaLoteDto, FaturamentoEntregaLoteListarContasPorLoteInputDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .EnablePagination(true)
                .AddWhereMethod((inputWhere, dapperParameters) =>
                {
                    var whereBuilder = new StringBuilder();

                    dapperParameters.Add("isDeleted", false);
                    dapperParameters.Add("isAtivo", true);
                    return whereBuilder.ToString();
                })
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }

        private static string ExecutaFiltroListarLotes(FaturamentoEntregaLoteInputDto dto, Dictionary<string, object> dapperParameters)
        {
            dapperParameters.Add("deleted", false);
            var where = new StringBuilder();

            where.WhereIf(dto.ConvenioId.HasValue, " AND SisConvenio.Id = @ConvenioId ");
            where.WhereIf(dto.EmpresaId.HasValue, " AND SisEmpresaId = @EmpresaId ");
            where.WhereIf(dto.PacienteId.HasValue, " AND SisPaciente.Id = @PacienteId ");
            where.WhereIf(dto.TipoInternacao.HasValue, " AND IsAmbulatorio = @TipoInternacao ");

            where.WhereIf(dto.StartDate.HasValue && dto.EndDate.HasValue, " AND FatEntregaLote.DataInicial BETWEEN @StartDate AND @EndDate ");

            where.WhereIf(dto.StartDateEntrega.HasValue && dto.EndDateEntrega.HasValue, " AND FatEntregaLote.DataEntrega BETWEEN @StartDateEntrega AND @EndDateEntrega ");

            return where.ToString();
        }

        public async Task<PagedResultDto<FaturamentoEntregaLoteDto>> Listar(ListarEntregasInput input)
        {
            var itemrEntregaLotes = 0;
            List<FaturamentoEntregaLote> itens;
            List<FaturamentoEntregaLoteDto> itensDtos = new List<FaturamentoEntregaLoteDto>();
            try
            {
                var query = _loteRepository
                    .GetAll()
                    .Include(c => c.Convenio)
                    .Include(c => c.Convenio.SisPessoa)
                    .Where(c => c.EmpresaId.ToString() == input.EmpresaId)
                    .Where(c => c.ConvenioId.ToString() == input.ConvenioId)
                    ;

                itemrEntregaLotes = await query.CountAsync();

                itens = await query
                    .AsNoTracking()
                       .OrderBy(input.Sorting)
                       .PageBy(input)
                    .ToListAsync();

                itensDtos = itens.MapTo<List<FaturamentoEntregaLoteDto>>();

                return new PagedResultDto<FaturamentoEntregaLoteDto>(itemrEntregaLotes, itensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<DefaultReturn<string>> CriarLote(CriaLoteInput input)
        {

            if (input.TipoInternacao.HasValue)
            {
                input.IsInternacao = input.TipoInternacao.Value == 0;
                input.IsAmbulatorio = input.TipoInternacao.Value == 1;
            }
            long loteId = 0;
            DefaultReturn<string> retorno = new DefaultReturn<string>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.ContaIds.Count() == 0)
                    {
                        return new DefaultReturn<string>();
                    }

                    if (input.LoteId != 0)
                    {
                        return new DefaultReturn<string>();
                    }

                    var novoLote = new FaturamentoEntregaLote
                    {
                        ConvenioId = input.ConvenioId,
                        CodEntregaLote = GerarSequenciaLote(input.ConvenioId),   //input.CodigoEntrega;
                        NumeroProcesso = input.NumeroProcesso,
                        IsInternacao = input.IsInternacao,
                        IsAmbulatorio = input.IsAmbulatorio,
                        EmpresaId = input.EmpresaId,
                        DataInicial = input.StartDate,
                        DataFinal = input.EndDate,
                    };

                    // Usuario que gerou o lote
                    var usuario = GetCurrentUser();
                    novoLote.UsuarioLoteId = usuario.Id;

                    var contas = await _contaRepository.GetAll().Include(x => x.ContaItens).Where(x => input.ContaIds.Contains(x.Id)).ToListAsync();
                    if (novoLote.EmpresaId == 0)
                    {
                        novoLote.EmpresaId = contas.Where(x => x.EmpresaId.HasValue).Select(x => x.EmpresaId.Value).Distinct().FirstOrDefault();
                    }

                    novoLote.Contas = contas.Select(x => new FaturamentoEntregaConta()
                    {
                        EntregaLote = novoLote,
                        ContaMedica = x,
                        ContaMedicaId = x.Id
                    }).ToList();

                    loteId = await _loteRepository.InsertAndGetIdAsync(novoLote);

                    foreach (var conta in contas)
                    {
                        conta.StatusId = FaturamentoContaStatus.Lote;

                        foreach (var i in conta.ContaItens)
                        {
                            i.StatusId = FaturamentoContaStatus.Lote;
                            //_contaItemRepository.Update(i);
                        }
                    }

                    _unitOfWorkManager.Current.SaveChanges();
                    //retorno = _TISSAppService.GerarLoteXML(loteId);

                    retorno.ReturnObject = loteId.ToString();

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    _unitOfWorkManager.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

            return retorno;
        }


        public async Task<DefaultReturn<EntregaTissLoteGerado>> GerarLote(long EntregaLoteId)
        {
            using (var geraLoteDomainService = IocManager.Instance.ResolveAsDisposable<IGeraLoteDomainService>())
            {
                var result = new DefaultReturn<EntregaTissLoteGerado>();
                var loteGerado = await geraLoteDomainService.Object.GerarLote(EntregaLoteId);
                if (!loteGerado.Gerado)
                {
                    result.Errors.Add(ErroDto.Criar("", "Não foi possivel gerar o lote"));
                    return result;
                }
                result.ReturnObject = loteGerado;
                return result;
            }
        }

        public async Task<DefaultReturn<string>> CriarOuEditar(CrudEntregaLoteContaInput input)
        {
            long loteId = 0;
            DefaultReturn<string> retorno = null;


            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.ContasIds.Count() == 0)
                        return new DefaultReturn<string>();

                    if (input.LoteId.Equals(0))
                    {
                        input.Lote = new FaturamentoEntregaLoteDto();
                        var novoLote = input.Lote.MapTo<FaturamentoEntregaLote>();
                        novoLote.ConvenioId = input.ConvenioId;
                        novoLote.CodEntregaLote = GerarSequenciaLote(input.ConvenioId);   //input.CodigoEntrega;
                        novoLote.NumeroProcesso = input.NumeroProcesso;
                        novoLote.IsInternacao = input.IsInternacao;
                        novoLote.IsAmbulatorio = input.IsAmbulatorio;
                        novoLote.EmpresaId = input.EmpresaId;

                        // Usuario que gerou o lote
                        var usuario = GetCurrentUser();
                        novoLote.UsuarioLoteId = usuario.Id;

                        loteId = await _loteRepository.InsertAndGetIdAsync(novoLote);
                        var contas = new List<FaturamentoConta>();

                        // talvez essas ids devam ser de 'EntregaConta' e nao de 'ContaMedica'
                        // fazer foreach nas 'EntergaContas' enviadas via ContasIds q deveria ser 'EntregaContasIds'
                        foreach (var contaId in input.ContasIds)
                        {
                            var entrega = _entregaContaRepository.Get(contaId);
                            var conta = _contaRepository.Get((long)entrega.ContaMedicaId);
                            conta.StatusId = 4;
                            entrega.EntregaLoteId = loteId;

                            // Atualizando Status da entrega de cada item das contas
                            var itens = _contaItemRepository.GetAll().Where(x => x.FaturamentoContaId == conta.Id).ToList();
                            foreach (var i in itens)
                            {
                                i.StatusId = 4;
                                _contaItemRepository.Update(i);
                            }

                            await _contaRepository.UpdateAsync(conta);
                            await _entregaContaRepository.UpdateAsync(entrega);
                        }
                        _unitOfWorkManager.Current.SaveChanges();
                        retorno = _TISSAppService.GerarLoteXML(loteId);

                        retorno.ReturnObject = loteId.ToString();

                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        _unitOfWorkManager.Current.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

            return retorno;
        }

        public async Task Excluir(FaturamentoEntregaLoteDto input)
        {
            try
            {
                var entregas = _entregaContaRepository.GetAll()
                    .Where(e => e.EntregaLoteId == input.Id)
                    .Include(c => c.ContaMedica);

                foreach (var entrega in entregas)
                {
                    //var contaId = entrega.ContaMedicaId;
                    //var conta = _contaRepository.Get((long)contaId);
                    entrega.ContaMedica.StatusId = 3; // Status 3 = 'Entregue'
                    _contaRepository.Update(entrega.ContaMedica);
                }


                await _loteRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoEntregaLoteDto> Obter(long id)
        {
            try
            {
                var query = await _loteRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoEntregaLoteDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarEntregasInput input)
        {
            try
            {
                var result = await Listar(input);
                var itens = result.Items;
                return _listarEntregaLotesExcelExporter.ExportToFile(itens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ListResultDto<FaturamentoEntregaLoteDto>> ListarTodos()
        {
            try
            {
                var query = _loteRepository.GetAll();

                var faturamentoEntregaLotesDto = await query.ToListAsync();

                return new ListResultDto<FaturamentoEntregaLoteDto> { Items = faturamentoEntregaLotesDto.MapTo<List<FaturamentoEntregaLoteDto>>() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoItemDto> faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _loteRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private string GerarSequenciaLote(long convenioId)
        {
            string sequencia = string.Empty;

            //var teste = ((Abp.EntityFramework.Uow.EfUnitOfWork)(((Abp.AbpServiceBase)_sessionService).UnitOfWorkManager.Current)).GetAllActiveDbContexts();

            // ((Abp.EntityFramework.Uow.EfUnitOfWork)((Abp.Domain.Uow.UnitOfWorkManager)((SW10.SWMANAGER.SWMANAGERAppServiceBase)_sessionService).UserManager._unitOfWorkManager).Current).ActiveDbContexts
            // ((Abp.EntityFramework.Uow.EfUnitOfWork)((Abp.Domain.Uow.UnitOfWorkManager)((SW10.SWMANAGER.SWMANAGERAppServiceBase)_sessionService).UserManager._unitOfWorkManager).Current).ActiveDbContexts


            // var user = ((SW10.SWMANAGER.SWMANAGERAppServiceBase)_sessionService).UserManager;
            // var aewrer = user.Users;

            //// var aaaa = ((Abp.EntityFramework.Uow.EfUnitOfWork)((SW10.SWMANAGER.SWMANAGERAppServiceBase)_sessionService).UnitOfWorkManager.Current).GetAllActiveDbContexts();

            // var contexts = ((Abp.EntityFramework.Uow.EfUnitOfWork)_unitOfWorkManager.Current).GetAllActiveDbContexts();

            //var teste = ((Abp.EntityFramework.Uow.EfUnitOfWork)(((Abp.AbpServiceBase)_sessionService).UnitOfWorkManager.Current)).GetAllActiveDbContexts();

            // DbContext context = null;

            // if(contexts.Count>0)
            // {
            //     context = contexts[0];
            // }


            // var faturamentoSequenciaLoteRepository = new SWRepository<FaturamentoSequenciaLote>(AbpSession, context);
            var faturamentoSequenciaLoteRepository = new SWRepository<FaturamentoSequenciaLote>(AbpSession, _sessionService);

            var faturamentoSequenciaLote = faturamentoSequenciaLoteRepository.GetAll()
                                               .Where(w => w.ConvenioId == convenioId)
                                               .FirstOrDefault();

            if (faturamentoSequenciaLote != null)
            {
                var sequenciaLote = ++faturamentoSequenciaLote.Sequencia;

                sequencia = sequenciaLote.ToString().PadLeft(12, '0');


                faturamentoSequenciaLoteRepository.Update(faturamentoSequenciaLote);
            }
            else
            {
                sequencia = "000000000001";

                faturamentoSequenciaLoteRepository.Insert(new FaturamentoSequenciaLote { ConvenioId = convenioId, Sequencia = 1 });
            }

            return sequencia;
        }


    }

}
