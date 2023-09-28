using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos
{

    public class EventoGrupoAppService : SWMANAGERAppServiceBase, IEventoGrupoAppService
    {
        #region Injecao e Construtor

        private readonly IRepository<Atendimento, long> _atendimentoRepository;
        private readonly IListarAtendimentosExcelExporter _listarAtendimentosExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IRepository<EstoqueMovimento, long> _estoqueMovimentoRepository;
        private readonly IRepository<FaturamentoConta, long> _fatContaRepository;

        public EventoGrupoAppService(
            IRepository<Atendimento, long> atendimentoRepository,
            IListarAtendimentosExcelExporter listarAtendimentosExcelExporter,
            IUnitOfWorkManager unitOfWorkManager,
            IUltimoIdAppService ultimoIdAppService,
           IPacienteAppService pacienteAppService,
           IRepository<EstoqueMovimento, long> estoqueMovimentoRepository,
           IRepository<FaturamentoConta, long> fatContaRepository
            )
        {
            _atendimentoRepository = atendimentoRepository;
            _listarAtendimentosExcelExporter = listarAtendimentosExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoIdAppService;
            _pacienteAppService = pacienteAppService;
            _estoqueMovimentoRepository = estoqueMovimentoRepository;
            _fatContaRepository = fatContaRepository;
        }

        #endregion injecao e construtor.

        [UnitOfWork] //Atualizado (pablo 08/08/2017)
        public async Task<long> CriarOuEditar(AtendimentoDto input)
        {
            try
            {
                var atendimento = input.MapTo<Atendimento>();

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    //=========CRIAR MÉTODO PARA TRATAMENTO DE PRE-ATENDIMENTO===============
                    if (input.NomePreAtendimento != null)
                    {
                        PacienteDto paciente = new PacienteDto();
                        //pesquisa paciente
                        paciente = await _pacienteAppService.ObterPorCpf(input.CpfPreAtendimento);

                        if (paciente == null)
                        {
                            //CriarOuEditarPaciente paciente2 = new CriarOuEditarPaciente();
                            paciente = new PacienteDto();
                            paciente.NomeCompleto = input.NomePreAtendimento;
                            paciente.Telefone1 = input.TelefonePreAtendimento;
                            paciente.Nascimento = input.DataNascPreAtendimento;
                            paciente.Cpf = input.CpfPreAtendimento;
                            paciente.Rg = input.IdentPreAtendimento;
                            //salva paciente
                            atendimento.PacienteId = await _pacienteAppService.CriarOuEditarOriginal(paciente);
                        }
                        else
                        {
                            atendimento.PacienteId = paciente.Id;
                        }
                        atendimento.IsPreatendimento = true;
                        atendimento.AtendimentoTipoId = input.AtendimentoTipoId;
                        atendimento.DataRegistro = input.DataRegistro;
                        atendimento.DataPreatendimento = input.DataRegistro;
                        atendimento.Observacao = input.Observacao;

                    }  //=======FIM TRATAMENTO P/ PRE-ATENDIMENTO===============

                    //========Cria método depois geração do codigo caso ñ seja passado============
                    if (atendimento.Codigo.IsNullOrWhiteSpace())
                    {
                        // Buscando 'UltimoId'
                        var codigos = await _ultimoIdAppService.ListarTodos();
                        var codigosList = codigos.Items.ToList();

                        var ultimo = codigosList.FirstOrDefault(c => c.NomeTabela == "Atendimento");

                        // Atribuindo 'Codigo' de 'Atendimento'
                        atendimento.Codigo = ultimo.Codigo;

                        // Incrementando 'UltimoId'
                        var codigoNumero = Convert.ToInt64(ultimo.Codigo);
                        codigoNumero++;
                        ultimo.Codigo = codigoNumero.ToString();
                        await _ultimoIdAppService.CriarOuEditar(ultimo);

                        // Precisa reverter este incremento caso ocorra erro na insercao do Atendimento?
                    }
                    //========fim geração do codigo caso ñ seja passado============


                    if (input.Id.Equals(0))
                    {
                        atendimento = await _atendimentoRepository.InsertAsync(atendimento);

                        // Nova conta medica
                        // Todo atendimento salvo deve gerar automaticamente uma nova conta associada a este atendimento
                        FaturamentoConta novaConta = new FaturamentoConta();

                        // Todos os campos de FatConta abaixo, devem ser preenchidos conforme possível
                        // obtendo dados do Atendimento que gera esta nova conta
                        novaConta.AtendimentoId = atendimento.Id;
                        novaConta.EmpresaId = atendimento.EmpresaId;
                        novaConta.ConvenioId = atendimento.ConvenioId;
                        novaConta.PlanoId = atendimento.PlanoId;
                        novaConta.PacienteId = atendimento.PacienteId;
                        novaConta.MedicoId = atendimento.MedicoId;
                        novaConta.Matricula = atendimento.Matricula;
                        novaConta.CodDependente = string.Empty;
                        novaConta.NumeroGuia = atendimento.GuiaNumero;
                        novaConta.Titular = input.Titular;
                        novaConta.FatGuiaId = atendimento.FatGuiaId;
                        novaConta.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
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
                        novaConta.TipoAtendimento = atendimento.IsAmbulatorioEmergencia ? 1 : 2;
                        novaConta.IsAutorizador = false;
                        novaConta.TipoAcomodacaoId = atendimento.Leito?.TipoAcomodacaoId; // tipo de leito eh tipo de acomdacao
                        novaConta.Observacao = atendimento.Observacao;
                        novaConta.SenhaAutorizacao = null;
                        novaConta.IdentAcompanhante = null;

                        if (atendimento.IsInternacao)
                        {
                            novaConta.DataInicio = atendimento.DataRegistro;
                        }

                        await _fatContaRepository.InsertAndGetIdAsync(novaConta);
                        // Fim - nova conta medica
                    }
                    else
                    {
                        await _atendimentoRepository.UpdateAsync(atendimento);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();

                    return atendimento.Id;
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
                    await _atendimentoRepository.DeleteAsync(id);
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

        public async Task<PagedResultDto<AtendimentoDto>> ListarTodos()
        {
            var contarAtendimentos = 0;
            List<Atendimento> atendimentos;
            List<AtendimentoDto> atendimentosDtos = new List<AtendimentoDto>();
            try
            {
                var query = _atendimentoRepository
                  .GetAll()
                  .Include(m => m.Paciente)
                  .Include(m => m.Paciente.SisPessoa)
                  .Include(m => m.Medico)
                  .Include(m => m.Medico.SisPessoa)
                  .Include(m => m.AtendimentoTipo)
                  .Include(m => m.Convenio)
                  .Include(m => m.Convenio.SisPessoa)
                  .Include(m => m.Empresa)
                  .Include(m => m.Especialidade)
                  .Include(m => m.Guia)
                  .Include(m => m.Leito)
                  .Include(m => m.MotivoAlta)
                  .Include(m => m.Nacionalidade)
                  .Include(m => m.Origem)
                  .Include(m => m.Plano)
                  .Include(m => m.ServicoMedicoPrestado)
                  .Include(m => m.UnidadeOrganizacional);


                contarAtendimentos = await query
                    .CountAsync();

                atendimentos = await query
                    .AsNoTracking()

                    .ToListAsync();

                atendimentosDtos = atendimentos
                    .MapTo<List<AtendimentoDto>>();

                return new PagedResultDto<AtendimentoDto>(
                contarAtendimentos,
                atendimentosDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<PagedResultDto<AtendimentoDto>> ListarFiltro(ListarAtendimentosInput input)
        {
            var contarAtendimentos = 0;
            List<Atendimento> atendimentos;
            try
            {
                //var query = _atendimentoRepository
                //    .GetAll()
                //    .Where(a => a.IsAmbulatorioEmergencia == true)
                //    .Where(m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate)
                //    .WhereIf(input.EmpresaId > 0, m => m.EmpresaId == input.EmpresaId)
                //    .WhereIf(input.UnidadeOrganizacionalId > 0, m => m.UnidadeOrganizacionalId == input.UnidadeOrganizacionalId)
                //    .WhereIf(input.ConvenioId > 0, m => m.ConvenioId == input.ConvenioId)
                //    .WhereIf(input.MedicoId > 0, m => m.MedicoId == input.MedicoId)
                //    .WhereIf(input.PacienteId > 0, m => m.PacienteId == input.PacienteId
                //    );

                //input.endDate2 = input.endDate2.AddDays(1);

                var query = _atendimentoRepository
                  .GetAll()
                  .Include(m => m.Paciente)
                  .Include(m => m.Paciente.SisPessoa)
                  .Include(m => m.Medico)
                  .Include(m => m.Medico.SisPessoa)
                  .Include(m => m.AtendimentoTipo)
                  .Include(m => m.Convenio)
                  .Include(m => m.Convenio.SisPessoa)
                  .Include(m => m.Empresa)
                  .Include(m => m.Especialidade)
                  .Include(m => m.Guia)
                  .Include(m => m.Leito)
                  .Include(m => m.Leito.TipoAcomodacao)
                  .Include(m => m.MotivoAlta)
                  .Include(m => m.Nacionalidade)
                  .Include(m => m.Origem)
                  .Include(m => m.Plano)
                  .Include(m => m.ServicoMedicoPrestado)
                  .Include(m => m.UnidadeOrganizacional)
                  //=== filtro generico pablo 
                  .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.Paciente.NomeCompleto.Contains(input.Filtro) ||
                    m.UnidadeOrganizacional.Descricao.Contains(input.Filtro) ||
                    m.Codigo.Contains(input.Filtro) ||
                    m.Paciente.CodigoPaciente.ToString().Contains(input.Filtro) ||
                    m.Convenio.NomeFantasia.Contains(input.Filtro) ||
                    m.Medico.NomeCompleto.Contains(input.Filtro) ||
                    m.Leito.TipoAcomodacao.Descricao.Contains(input.Filtro) ||
                    m.Leito.Descricao.Contains(input.Filtro) ||
                    m.Empresa.NomeFantasia.Contains(input.Filtro)
                    )
                  //===
                  .Where(a => a.IsAmbulatorioEmergencia == true)
                  .WhereIf(input.FiltroData == "Alta", m => m.DataAlta >= input.StartDate && m.DataRegistro <= input.EndDate)
                  .WhereIf((input.FiltroData == "Atendimento"), m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate)
                  .WhereIf(input.EmpresaId > 0, m => m.EmpresaId == input.EmpresaId)
                  .WhereIf(input.UnidadeOrganizacionalId > 0, m => m.UnidadeOrganizacionalId == input.UnidadeOrganizacionalId)
                  .WhereIf(input.ConvenioId > 0, m => m.ConvenioId == input.ConvenioId)
                  .WhereIf(input.MedicoId > 0, m => m.MedicoId == input.MedicoId)
                  .WhereIf(input.PacienteId > 0, m => m.PacienteId == input.PacienteId)
                  .WhereIf(input.NacionalidadeResponsavelId > 0, m => m.NacionalidadeResponsavelId == input.NacionalidadeResponsavelId)
                  .WhereIf(input.IsAmbulatorioEmergencia.HasValue, m => m.IsAmbulatorioEmergencia == input.IsAmbulatorioEmergencia.Value)
                  .WhereIf(input.IsInternacao.HasValue, m => m.IsInternacao == input.IsInternacao.Value)
                   //.WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Paciente.NomeCompleto.Contains(input.Filtro))
                   .WhereIf(!input.NomePaciente.IsNullOrEmpty(), m => m.Paciente.NomeCompleto.Contains(input.NomePaciente))
                  //.WhereIf(input.FiltroData == "Internado", m => m.DataRegistro >= input.StartDate && m.DataRegistro <= input.EndDate &&  m.DataAlta == null)
                  .WhereIf(input.FiltroData == "Internado", m => m.DataAlta == null)
                //.WhereIf(input.Internados, m => m.DataAlta == null)
                //.WhereIf(input.Internados, a => DateTime.Compare((DateTime)a.DataAlta, DateTime.Now) >= 0)
                ;

                contarAtendimentos = await query
                    .CountAsync();

                atendimentos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var atendimentosDto = new List<AtendimentoDto>();
                //atendimentos.MapTo<List<AtendimentoDto>>();



                AtendimentoDto atendimentoDto;// = new AtendimentoDto();
                foreach (var item in atendimentos)
                {
                    atendimentoDto = new AtendimentoDto();

                    atendimentoDto.Id = item.Id;
                    atendimentoDto.UnidadeOrganizacional = item.UnidadeOrganizacional.MapTo<UnidadeOrganizacionalDto>();
                    atendimentoDto.Codigo = item.Codigo;
                    atendimentoDto.Paciente = item.Paciente.MapTo<PacienteDto>();
                    atendimentoDto.DataRegistro = item.DataRegistro;
                    atendimentoDto.DataAlta = item.DataAlta;
                    atendimentoDto.Convenio = item.Convenio.MapTo<ConvenioDto>();
                    atendimentoDto.Medico = item.Medico.MapTo<MedicoDto>();
                    atendimentoDto.Leito = item.Leito.MapTo<LeitoDto>();
                    atendimentoDto.Empresa = item.Empresa.MapTo<EmpresaDto>();

                    atendimentosDto.Add(atendimentoDto);

                }



                return new PagedResultDto<AtendimentoDto>(
                    contarAtendimentos,
                    atendimentosDto
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarAtendimentosInput input)
        {
            try
            {
                //var result = await Listar(input);
                var result = await ListarTodos();
                var atendimentos = result.Items;
                return _listarAtendimentosExcelExporter.ExportToFile(atendimentos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<AtendimentoDto> Obter(long id)
        {
            try
            {
                var result = await _atendimentoRepository
                    .GetAll()
                  .Include(m => m.Paciente)
                  .Include(m => m.Paciente.SisPessoa)
                  .Include(m => m.Medico)
                  .Include(m => m.Medico.SisPessoa)
                  .Include(m => m.AtendimentoTipo)
                  .Include(m => m.Convenio)
                  .Include(m => m.Convenio.SisPessoa)
                  .Include(m => m.Empresa)
                  .Include(m => m.Especialidade)
                  .Include(m => m.Guia) // modelo antigo
                  .Include(m => m.FatGuia) // novo modelo FatGuia
                  .Include(m => m.Leito)
                  .Include(m => m.Leito.TipoAcomodacao)
                  .Include(m => m.MotivoAlta)
                  .Include(m => m.Nacionalidade)
                  .Include(m => m.Origem)
                  .Include(m => m.Plano)
                  .Include(m => m.ServicoMedicoPrestado)
                  .Include(m => m.UnidadeOrganizacional)
                  .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var atendimento = result
                    .MapTo<AtendimentoDto>();

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
            List<AtendimentoDto> atendimentosDto = new List<AtendimentoDto>();
            try
            {
                //get com filtro
                var query = from p in _atendimentoRepository
                            .GetAll()
                            .Include(m => m.Paciente)
                            .Include(m => m.Paciente.SisPessoa)
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Paciente.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Paciente.NomeCompleto ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Paciente.NomeCompleto) };
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
