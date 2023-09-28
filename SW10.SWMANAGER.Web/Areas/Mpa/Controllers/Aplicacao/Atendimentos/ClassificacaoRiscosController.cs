using Abp.Application.Navigation;
using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.CoresPele;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Escolaridades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.EstadosCivis;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.ClassificacoesRisco
{
    public class ClassificacaoRiscosController : SWMANAGERControllerBase
    {
        //private readonly IClassificacaoRiscoAppService _classificacaoRiscoAppService;
        //private readonly IPacienteAppService _pacienteAppService;
        //private readonly IProfissaoAppService _profissaoAppService;
        //private readonly INaturalidadeAppService _naturalidadeAppService;
        //private readonly IOrigemAppService _origemAppService;
        //private readonly IPlanoAppService _planoAppService;
        //private readonly IPaisAppService _paisAppService;
        //private readonly IEstadoAppService _estadoAppService;
        //private readonly ICidadeAppService _cidadeAppService;
        //private readonly IConvenioAppService _convenioAppService;
        //private readonly ISexoAppService _sexoAppService;
        //private readonly IEscolaridadeAppService _escolaridadeAppService;
        //private readonly ICorPeleAppService _corPeleAppService;
        //private readonly IReligiaoAppService _religiaoAppService;
        //private readonly IEstadoCivilAppService _estadoCivilAppService;
        //private readonly ITipoTelefoneAppService _tipoTelefoneAppService;
        //private readonly IPacientePesoAppService _pacientePesoAppService;
        //private readonly IAgendamentoConsultaMedicoDisponibilidadeAppService _agendamentoConsultaMedicoDisponibilidadeAppService;
        //private readonly IEspecialidadeAppService _especialidadeAppService;
        //private readonly IAgendamentoConsultaAppService _agendamentoConsultaAppService;
        //private readonly IPreAtendimentoAppService _preAtendimentoAppService;

        //public ClassificacaoRiscosController(
        //    IUserNavigationManager userNavigationManager,
        //    IClassificacaoRiscoAppService classificacaoRiscoAppService,
        //    IPacienteAppService pacienteAppService,
        //    IProfissaoAppService profissaoAppService,
        //    INaturalidadeAppService naturalidadeAppService,
        //    IOrigemAppService origemAppService,
        //    IPaisAppService paisAppService,
        //    IEstadoAppService estadoAppService,
        //    ICidadeAppService cidadeAppService,
        //    IPlanoAppService planoAppService,
        //    IConvenioAppService convenioAppService,
        //    ISexoAppService sexoAppService,
        //    IEscolaridadeAppService escolaridadeAppService,
        //    ICorPeleAppService corPeleAppService,
        //    IReligiaoAppService religiaoAppService,
        //    IEstadoCivilAppService estadoCivilAppService,
        //    ITipoTelefoneAppService tipoTelefoneAppService,
        //    IPacientePesoAppService pacientePesoAppService,
        //    IAgendamentoConsultaMedicoDisponibilidadeAppService agendamentoConsultaMedicoDisponibilidadeAppService,
        //    IEspecialidadeAppService especialidadeAppService,
        //    IAgendamentoConsultaAppService agendamentoConsultaAppService,
        //    IPreAtendimentoAppService preAtendimentoAppService

        //    )
        //{
        //    _classificacaoRiscoAppService = classificacaoRiscoAppService;
        //    _pacienteAppService = pacienteAppService;
        //    _profissaoAppService = profissaoAppService;
        //    _naturalidadeAppService = naturalidadeAppService;
        //    _origemAppService = origemAppService;
        //    _planoAppService = planoAppService;
        //    _paisAppService = paisAppService;
        //    _estadoAppService = estadoAppService;
        //    _cidadeAppService = cidadeAppService;
        //    _convenioAppService = convenioAppService;
        //    _sexoAppService = sexoAppService;
        //    _escolaridadeAppService = escolaridadeAppService;
        //    _corPeleAppService = corPeleAppService;
        //    _religiaoAppService = religiaoAppService;
        //    _estadoCivilAppService = estadoCivilAppService;
        //    _tipoTelefoneAppService = tipoTelefoneAppService;
        //    _pacientePesoAppService = pacientePesoAppService;
        //    _agendamentoConsultaMedicoDisponibilidadeAppService = agendamentoConsultaMedicoDisponibilidadeAppService;
        //    _especialidadeAppService = especialidadeAppService;
        //    _agendamentoConsultaAppService = agendamentoConsultaAppService;
        //    _preAtendimentoAppService = preAtendimentoAppService;
        //}

        public ActionResult Index()
        {
            TempData["ClassificacaoRisco"] = new ClassificacaoRiscoDto();
            TempData["ClassificacaoRiscoId"] = 0;
            var model = new ClassificacoesRiscoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/ClassificacaoRiscos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos, AppPermissions.Pages_Tenant_Atendimento_ClassificacaoRiscos_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            using (var especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            using (var classificacaoRiscoAppService = IocManager.Instance.ResolveAsDisposable<IClassificacaoRiscoAppService>())
            {
                var especialidades = await especialidadeAppService.Object.ListarTodos().ConfigureAwait(false);

                CriarOuEditarClassificacaoRiscoModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await classificacaoRiscoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarClassificacaoRiscoModalViewModel(output);
                    viewModel.Especialidades = new SelectList(especialidades.Items, "Id", "Nome", output.Especialidade);
                }
                else
                {
                    viewModel = new CriarOuEditarClassificacaoRiscoModalViewModel(new CriarOuEditarClassificacaoRisco());
                    viewModel.Especialidades = new SelectList(especialidades.Items, "Id", "Nome");
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/ClassificacaoRiscos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _CriarOuEditarClassificacaoRisco()
        {
            using (var especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            using (var sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            {
                var sexos = await sexoAppService.Object.ListarTodos().ConfigureAwait(false);
                var especialidades = await especialidadeAppService.Object.ListarTodos().ConfigureAwait(false);
                CriarOuEditarClassificacaoRiscoModalViewModel viewModel;
                viewModel = new CriarOuEditarClassificacaoRiscoModalViewModel(new CriarOuEditarClassificacaoRisco());
                viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                viewModel.Especialidades = new SelectList(especialidades.Items, "Id", "Nome");
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/ClassificacaoRiscos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public PartialViewResult _PesquisarPreAtendimento()
        {
            var model = new PreAtendimentosViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/ClassificacaoRiscos/_PesquisarPreAtendimentos.cshtml", model);
        }

        public async Task<PartialViewResult> _PreAtendimento(long? id)
        {
            using (var sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            using (var preAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IPreAtendimentoAppService>())
            {
                var sexos = await sexoAppService.Object.ListarTodos().ConfigureAwait(false);

                CriarOuEditarPreAtendimentoModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await preAtendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreAtendimentoModalViewModel(output);
                    viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                }
                else
                {
                    viewModel = new CriarOuEditarPreAtendimentoModalViewModel(new CriarOuEditarPreAtendimento());
                    viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/ClassificacaoRiscos/_CriarOuEditarPreAtendimento.cshtml", viewModel);
            }
        }

        public PartialViewResult _PesquisarPaciente()
        {
            var model = new PacientesViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Pacientes/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _IdentificacaoPaciente(long? id)
        {
            using (var origemAppService = IocManager.Instance.ResolveAsDisposable<IOrigemAppService>())
            using (var sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            using (var corPeleAppService = IocManager.Instance.ResolveAsDisposable<ICorPeleAppService>())
            using (var escolaridadeAppService = IocManager.Instance.ResolveAsDisposable<IEscolaridadeAppService>())
            using (var religiaoAppService = IocManager.Instance.ResolveAsDisposable<IReligiaoAppService>())
            using (var estadoCivilAppService = IocManager.Instance.ResolveAsDisposable<IEstadoCivilAppService>())
            using (var tipoTelefoneAppService = IocManager.Instance.ResolveAsDisposable<ITipoTelefoneAppService>())
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var sexos = await sexoAppService.Object.ListarTodos().ConfigureAwait(false);
                var coresPele = await corPeleAppService.Object.ListarTodos().ConfigureAwait(false);
                var escolaridades = await escolaridadeAppService.Object.ListarTodos().ConfigureAwait(false);
                var religioes = await religiaoAppService.Object.ListarTodos().ConfigureAwait(false);
                var estadosCivis = await estadoCivilAppService.Object.ListarTodos().ConfigureAwait(false);
                var tiposTelefone = await tipoTelefoneAppService.Object.ListarTodos().ConfigureAwait(false);

                CriarOuEditarPacienteModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await pacienteAppService.Object.Obter2((long)id).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPacienteModalViewModel(output);
                    //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                    //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao", output.Escolaridade);
                    //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao", output.CorPele);
                    //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao", output.Religiao);
                    //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao", output.EstadoCivil);
                    //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
                }
                else
                {
                    viewModel = new CriarOuEditarPacienteModalViewModel(new PacienteDto());
                    //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                    //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao");
                    //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao");
                    //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao");
                    //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao");
                    //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Pacientes/_IdentificacaoPaciente.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<long> SalvarPreAtendimento(CriarOuEditarPreAtendimento preAtendimento)
        {
            using (var preAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IPreAtendimentoAppService>())
            {
                var preAtendimentoInserido = await preAtendimentoAppService.Object.CriarGetId(preAtendimento).ConfigureAwait(false);
                return preAtendimentoInserido;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SalvarClassificacaoRisco(CriarOuEditarClassificacaoRisco classificacaoRisco)
        {
            using (var classificacaoRiscoAppService = IocManager.Instance.ResolveAsDisposable<IClassificacaoRiscoAppService>())
            {
                ClassificacaoRiscoDto relacao = new ClassificacaoRiscoDto();
                await classificacaoRiscoAppService.Object.CriarOuEditar(classificacaoRisco).ConfigureAwait(false);
                return Content(L("Sucesso"));
            }
        }
    }
}