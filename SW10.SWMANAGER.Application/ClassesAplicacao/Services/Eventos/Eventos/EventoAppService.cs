using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;

using SW10.SWMANAGER.ClassesAplicacao.Eventos.Eventos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos
{

    public class EventoAppService : SWMANAGERAppServiceBase, IEventoAppService
    {
        #region Injecao e Construtor

        private readonly IRepository<Evento, long> _eventoRepository;
        private readonly IListarEventosExcelExporter _listarEventosExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IRepository<EstoqueMovimento, long> _estoqueMovimentoRepository;
        private readonly IRepository<FaturamentoConta, long> _fatContaRepository;

        public EventoAppService(
            IRepository<Evento, long> eventoRepository,
            IListarEventosExcelExporter listarEventosExcelExporter,
            IUnitOfWorkManager unitOfWorkManager,
            IUltimoIdAppService ultimoIdAppService,
           IPacienteAppService pacienteAppService,
           IRepository<EstoqueMovimento, long> estoqueMovimentoRepository,
           IRepository<FaturamentoConta, long> fatContaRepository
            )
        {
            _eventoRepository = eventoRepository;
            _listarEventosExcelExporter = listarEventosExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoIdAppService;
            _pacienteAppService = pacienteAppService;
            _estoqueMovimentoRepository = estoqueMovimentoRepository;
            _fatContaRepository = fatContaRepository;
        }

        #endregion injecao e construtor.

        [UnitOfWork] //Atualizado (pablo 08/08/2017)
        public async Task<long> CriarOuEditar(EventoDto input)
        {
            try
            {
                var evento = input.MapTo<Evento>();

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    if (input.Id.Equals(0))
                    {
                        evento = await _eventoRepository.InsertAsync(evento);

                        // Nova conta medica
                        // Todo atendimento salvo deve gerar automaticamente uma nova conta associada a este atendimento
                        FaturamentoConta novaConta = new FaturamentoConta();

                        //  Todos os campos de FatConta abaixo, devem ser preenchidos conforme possível
                        // obtendo dados do Atendimento que gera esta nova conta
                        novaConta.AtendimentoId = evento.Id;
                        //novaConta.EmpresaId = evento.EmpresaId;
                        //novaConta.ConvenioId = evento.ConvenioId;
                        //novaConta.PlanoId = evento.PlanoId;
                        //novaConta.PacienteId = evento.PacienteId;
                        //novaConta.MedicoId = evento.MedicoId;
                        //novaConta.Matricula = evento.Matricula;
                        //novaConta.CodDependente = string.Empty;
                        //novaConta.NumeroGuia = evento.GuiaNumero;
                        //novaConta.Titular = input.Titular;
                        //novaConta.FatGuiaId = evento.FatGuiaId;
                        //novaConta.UnidadeOrganizacionalId = evento.UnidadeOrganizacionalId;
                        novaConta.StatusId = 1; // StatusId 1 = 'Inicial'
                        novaConta.DataInicio = DateTime.Now;
                        novaConta.DataFim = null;
                        novaConta.DataPagamento = null;
                        novaConta.ValidadeCarteira = null;
                        novaConta.DataAutorizacao = null;
                        novaConta.DiaSerie1 = null;
                        novaConta.DiaSerie2 = null;
                        novaConta.DiaSerie3 = null;
                        novaConta.DiaSerie4 = null;
                        novaConta.DiaSerie5 = null;
                        novaConta.DiaSerie6 = null;
                        novaConta.DiaSerie7 = null;
                        novaConta.DiaSerie8 = null;
                        novaConta.DiaSerie9 = null;
                        novaConta.DiaSerie10 = null;
                        novaConta.DataEntrFolhaSala = null;
                        novaConta.DataEntrDescCir = null;
                        novaConta.DataEntrBolAnest = null;
                        novaConta.DataEntrCDFilme = null;
                        novaConta.DataValidadeSenha = null;
                        novaConta.GuiaOperadora = null;
                        novaConta.GuiaPrincipal = null;
                        //novaConta.TipoAtendimento = evento.IsAmbulatorioEmergencia ? 1 : 2;
                        //novaConta.IsAutorizador = false;
                        //novaConta.TipoLeitoId = evento.Leito?.TipoAcomodacaoId; // tipo de leito eh tipo de acomdacao
                        //novaConta.Observacao = evento.Observacao;
                        //novaConta.SenhaAutorizacao = null;
                        //novaConta.IdentAcompanhante = null;


                        await _fatContaRepository.InsertAndGetIdAsync(novaConta);
                        // Fim - nova conta medica
                    }
                    else
                    {
                        await _eventoRepository.UpdateAsync(evento);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();

                    return evento.Id;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _eventoRepository.DeleteAsync(id);
                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<EventoDto>> ListarTodos()
        {
            var contarAtendimentos = 0;
            List<Evento> evento;
            List<EventoDto> eventosDtos = new List<EventoDto>();
            try
            {
                var query = _eventoRepository
                  .GetAll();
                //.Include(m => m.Paciente)
                //.Include(m => m.Origem);


                contarAtendimentos = await query
                    .CountAsync();

                evento = await query
                    .AsNoTracking()

                    .ToListAsync();

                eventosDtos = evento
                    .MapTo<List<EventoDto>>();

                return new PagedResultDto<EventoDto>(
                contarAtendimentos,
                eventosDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EventoDto>> ListarFiltro(ListarAtendimentosInput input)
        {
            var contarAtendimentos = 0;
            List<Evento> atendimentos;
            try
            {
                var query = _eventoRepository
                  .GetAll()
                  // .Include(m => m.Paciente)
                  //.Include(m => m.Paciente.SisPessoa)
                  //.Include(m => m.Medico)
                  //.Include(m => m.Medico.SisPessoa)
                  //.Include(m => m.AtendimentoTipo)
                  //.Include(m => m.Convenio)
                  //.Include(m => m.Convenio.SisPessoa)
                  //.Include(m => m.Empresa)
                  //.Include(m => m.Especialidade)
                  //.Include(m => m.Guia)
                  //.Include(m => m.Leito)
                  //.Include(m => m.Leito.TipoAcomodacao)
                  //.Include(m => m.MotivoAlta)
                  //.Include(m => m.Nacionalidade)
                  //.Include(m => m.Origem)
                  //.Include(m => m.Plano)
                  //.Include(m => m.ServicoMedicoPrestado)
                  //.Include(m => m.UnidadeOrganizacional)
                  //=== filtro generico pablo 
                  //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                  //  m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.UnidadeOrganizacional.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.Codigo.Contains(input.Filtro.ToUpper()) ||
                  //  m.Paciente.CodigoPaciente.ToString().ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.Convenio.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.Medico.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.Leito.TipoAcomodacao.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.Leito.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                  //  m.Empresa.NomeFantasia.ToUpper().Contains(input.Filtro.ToUpper())
                  //  )
                  //===
                  //.Where(a => a.IsAmbulatorioEmergencia == true)
                  // .WhereIf(input.FiltroData == "Alta", m => m.DataAlta >= input.StartDate && m.DataRegistro <= input.EndDate)
                  // .WhereIf((input.FiltroData == "Atendimento"), m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate)
                  // .WhereIf(input.EmpresaId > 0, m => m.EmpresaId == input.EmpresaId)
                  .WhereIf(input.UnidadeOrganizacionalId > 0, m => m.Codigo == input.NomePaciente);
                //.WhereIf(input.ConvenioId > 0, m => m.ConvenioId == input.ConvenioId)
                //.WhereIf(input.MedicoId > 0, m => m.MedicoId == input.MedicoId)
                //.WhereIf(input.PacienteId > 0, m => m.PacienteId == input.PacienteId)
                //.WhereIf(input.NacionalidadeResponsavelId > 0, m => m.NacionalidadeResponsavelId == input.NacionalidadeResponsavelId)
                //.WhereIf(input.IsAmbulatorioEmergencia.HasValue, m => m.IsAmbulatorioEmergencia == input.IsAmbulatorioEmergencia.Value)
                //.WhereIf(input.IsInternacao.HasValue, m => m.IsInternacao == input.IsInternacao.Value)
                // //.WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Paciente.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()))
                // .WhereIf(!input.NomePaciente.IsNullOrEmpty(), m => m.Paciente.NomeCompleto.ToUpper().Contains(input.NomePaciente))
                ////.WhereIf(input.FiltroData == "Internado", m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate &&  m.DataAlta == null)
                //.WhereIf(input.FiltroData == "Internado", m => m.DataAlta == null)
                //.WhereIf(input.Internados, m => m.DataAlta == null)
                //.WhereIf(input.Internados, a => DateTime.Compare((DateTime)a.DataAlta, DateTime.Now) >= 0)


                contarAtendimentos = await query
                    .CountAsync();

                atendimentos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var EvenotosDto = new List<EventoDto>();
                //atendimentos.MapTo<List<EventoDto>>();



                //EventoDto atendimentoDto;// = new EventoDto();
                //foreach (var item in atendimentos)
                //{
                //    atendimentoDto = new EventoDto();

                //    atendimentoDto.Id = item.Id;
                //    // atendimentoDto.UnidadeOrganizacional = item.UnidadeOrganizacional.MapTo<UnidadeOrganizacionalDto>();
                //    atendimentoDto.Codigo = item.Codigo;
                //    //atendimentoDto.Paciente = item.Paciente.MapTo<PacienteDto>();
                //    //atendimentoDto.DataRegistro = item.DataRegistro;
                //    //atendimentoDto.DataAlta = item.DataAlta;
                //    //atendimentoDto.Convenio = item.Convenio.MapTo<ConvenioDto>();
                //    //atendimentoDto.Medico = item.Medico.MapTo<MedicoDto>();
                //    //atendimentoDto.Leito = item.Leito.MapTo<LeitoDto>();
                //    //atendimentoDto.Empresa = item.Empresa.MapTo<EmpresaDto>();

                //    atendimentosDto.Add(atendimentoDto);

                //}



                return new PagedResultDto<EventoDto>(
                    contarAtendimentos,
                    EvenotosDto
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEventosInput input)
        {
            try
            {
                // var result = await Listar(input);
                var result = await ListarTodos();
                var atendimentos = result.Items;
                return _listarEventosExcelExporter.ExportToFile(atendimentos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<EventoDto> Obter(long id)
        {
            try
            {
                var result = await _eventoRepository
                    .GetAll()
                  //.Include(m => m.Paciente)
                  //.Include(m => m.Origem)
                  //.Include(m => m.Plano)
                  //.Include(m => m.ServicoMedicoPrestado)
                  //.Include(m => m.UnidadeOrganizacional)
                  .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var atendimento = result
                    .MapTo<EventoDto>();

                return atendimento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<EventoDto> eventosDto = new List<EventoDto>();
            try
            {
                //get com filtro
                var query = from p in _eventoRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
