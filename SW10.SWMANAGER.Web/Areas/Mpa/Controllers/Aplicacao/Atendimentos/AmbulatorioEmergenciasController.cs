using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Favoritos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Layout;
using SW10.SWMANAGER.Web.Areas.Mpa.Startup;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.AmbulatorioEmergencias
{
    public class AmbulatorioEmergenciasController : SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index()
        {
            //var pacientes = await _pacienteAppService.ListarTodos(); //.Listar(new ListarPacientesInput());
            //  var medicos = await _medicoAppService.ListarTodos();
            // var convenios = await _convenioAppService.ListarTodos();
            //var unidadesOrganizacionais = await _organizationUnitAppService.GetOrganizationUnits();
            // var unidadesOrganizacionais = await _unidadeOrganizacionalAppService.ListarTodos();
            //  var origens = await _origemAppService.ListarTodos();


            using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
            {
                var user = await userManager.Object.GetUserByIdAsync((long)AbpSession.UserId);
                var empresas = await userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value);

                if (empresas == null || empresas.Items.Count == 0)
                {
                    empresas = await empresaAppService.Object.ListarTodos().ConfigureAwait(false);
                }

                //var userId
                //var user = Task.Run(() => _userAppService.GetUser()).Result;
                var medicoId = user.MedicoId;
                if (medicoId.HasValue)
                {
                    ViewBag.UserMedicoId = medicoId.Value;
                    ViewBag.UserMedico = await medicoAppService.Object.Obter(medicoId.Value).ConfigureAwait(false);
                }

                var viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto())
                {
                    Empresas = new SelectList(
                                            empresas.Items.Select(
                                                m => new
                                                {
                                                    Id = m.Id,
                                                    NomeFantasia = string.Format("{0}", m.NomeFantasia)
                                                }),
                                            "Id",
                                            "NomeFantasia"),
                    FiltroDataAtendimento = true
                };
                return View(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Index.cshtml",
                    viewModel);
            }
        }

        [ChildActionOnly]
        public PartialViewResult Header()
        {
            using (var _sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            using (var _multiTenancyConfig = IocManager.Instance.ResolveAsDisposable<IMultiTenancyConfig>())
            using (var _languageManager = IocManager.Instance.ResolveAsDisposable<ILanguageManager>())
            {

                var headerModel = new HeaderViewModel
                {
                    LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.Object.GetCurrentLoginInformations()),
                    Languages = _languageManager.Object.GetLanguages(),
                    CurrentLanguage = _languageManager.Object.CurrentLanguage,
                    IsMultiTenancyEnabled = _multiTenancyConfig.Object.IsEnabled,
                    IsImpersonatedLogin = AbpSession.ImpersonatorUserId.HasValue
                };

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Layout/_Header.cshtml",
                    headerModel);
            }
        }

        [ChildActionOnly]
        public PartialViewResult Favoritos(string currentPageName = "", string menuName = MpaNavigationProvider.MenuName)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            using (var favoritoAppService = IocManager.Instance.ResolveAsDisposable<IFavoritoAppService>())
            {
                var favoritosList = AsyncHelper.RunSync(() => favoritoAppService.Object.Listar(userIdentifier.UserId));
                var favoritos = favoritosList.Items;

                var menu = new UserMenu()
                {
                    Name = "Favoritos",
                    CustomData = null,
                    DisplayName = "Favoritos",
                    Items = new List<UserMenuItem>()
                };

                foreach (var fav in favoritos)
                {
                    var item = new UserMenuItem() { Name = fav.Name, Icon = fav.Icon, Url = fav.Url };

                    menu.Items.Add(item);
                }

                var favoritosModel = new FavoritosViewModel { Menu = menu, CurrentPageName = currentPageName };

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Layout/_Favoritos.cshtml",
                    favoritosModel);
            }
        }

        [HttpPost]
        public async Task<long> CriarNovoAtendimento()
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                long id = await atendimentoAppService.Object.CriarNovoAtendimento();
                return id;
            }
        }

        //NÃO ESTÃO SENDO USADOS NO MOMENTO PABLO 20/07/2017
        //public async Task<ActionResult> ModalPacientes()
        //{
        //    var model = new PacientesViewModel();
        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Pacientes/Index.cshtml", model);
        //}

        //public async Task<ActionResult> ModalPreAtendimentos()
        //{
        //    var model = new PreAtendimentosViewModel();
        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/PreAtendimentos/Index.cshtml", model);
        //}

        //public async Task<ActionResult> ModalOrcamentos()
        //{
        //    var model = new OrcamentosViewModel();
        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Orcamentos/Index.cshtml", model);
        //}

        //public async Task<ActionResult> ModalClassificacaoRiscos()
        //{
        //    var model = new ClassificacoesRiscoViewModel();
        //    //var model = new ClassificacaoRiscosViewModel();
        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/ClassificacaoRiscos/Index.cshtml", model);
        //}

        [HttpPost]
        public void AtendimentoAtual(CriarOuEditarAtendimento item)
        {
            TempData["AtendimentoAtual"] = item.Id;
        }

        [HttpPost]
        public void NovoAtendimento(long? id, long abaId)
        {
            TempData["AtendimentoAtual"] = id;
        }

        public PartialViewResult _MenuTopo()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Layout/_MenuTopo.cshtml");
        }

        public PartialViewResult _PreAtendimento()
        {
            var model = new PreAtendimentosViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/PreAmbulatorioEmergencias/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarPreAtendimento()
        {
            CriarOuEditarPreAtendimentoModalViewModel viewModel;
            viewModel = new CriarOuEditarPreAtendimentoModalViewModel(new CriarOuEditarPreAtendimento());
            //  var sexos = await _sexoAppService.ListarTodos();
            // viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/PreAmbulatorioEmergencias/_CriarOuEditarModal.cshtml", viewModel);
        }

        public PartialViewResult _ClassificacaoRisco()
        {
            var model = new ClassificacoesRiscoViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/ClassificacoesRisco/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarClassificacaoRisco()
        {
            using (var especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            {

                var especialidades = await especialidadeAppService.Object.ListarTodos().ConfigureAwait(false);

                var viewModel = new CriarOuEditarClassificacaoRiscoModalViewModel(new CriarOuEditarClassificacaoRisco())
                {
                    Especialidades = new SelectList(especialidades.Items, "Id", "Nome")
                };

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/ClassificacoesRisco/_CriarOuEditarModal.cshtml",
                    viewModel);
            }
        }

        public PartialViewResult _PesquisarPaciente()
        {
            var model = new PacientesViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Pacientes/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _IdentificacaoPaciente(long? id)
        {
            //var origens = await _origemAppService.Listar(new ListarOrigensInput());
            //var sexos = await _sexoAppService.ListarTodos();
            //var coresPele = await _corPeleAppService.ListarTodos();
            //var escolaridades = await _escolaridadeAppService.ListarTodos();
            //var religioes = await _religiaoAppService.ListarTodos();
            //var estadosCivis = await _estadoCivilAppService.ListarTodos();
            //var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();

            CriarOuEditarPacienteModalViewModel viewModel;

            if (id.HasValue)
            {
                using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
                {

                    var output = await pacienteAppService.Object.Obter2((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPacienteModalViewModel(output);
                }
                //viewModel.Origens = new SelectList(origens.Items, "Id", "Descricao", output.OrigemId);
                //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao", output.Escolaridade);
                //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao", output.CorPele);
                //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao", output.Religiao);
                //        viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao", output.EstadoCivil);
                //     viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarPacienteModalViewModel(new PacienteDto());
                //viewModel.Origens = new SelectList(origens.Items, "Id", "Descricao");
                //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao");
                //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao");
                //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao");
                //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao");
                //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Pacientes/_IdentificacaoPaciente.cshtml", viewModel);
            //return PartialView("~/Areas/Mpa/Views/Aplicacao/AmbulatorioEmergencias/AmbulatorioEmergencias/_IdentificacaoPaciente.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _Agendamento()
        {
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            using (var especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            {

                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarAtivos(0, 0).ConfigureAwait(false);
                var especialidadesAgendadas =
                    agendamentos.Select(m => m.MedicoEspecialidade.EspecialidadeId).Distinct().ToList();
                var especialidades = await especialidadeAppService.Object.Listar(especialidadesAgendadas).ConfigureAwait(false);
                var viewModel = new AgendamentoConsultasViewModel
                {
                    Especialidades = new SelectList(especialidades.Items, "Id", "Nome")
                };

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Agendamentos/Index.cshtml",
                    viewModel);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            //var pacientes = await _pacienteAppService.ListarTodos(); //.Listar(new ListarPacientesInput());
            //var medicos = await _medicoAppService.ListarTodos();
            //var empresas = await _empresaAppService.ListarTodos();
            //var convenios = await _convenioAppService.ListarTodos();
            //var unidadesOrganizacionais = await _organizationUnitAppService.GetOrganizationUnits();

            CriarOuEditarAtendimentoModalViewModel viewModel;

            if (id.HasValue)
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var output = await atendimentoAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(output);
                }
                // viewModel.Pacientes = new SelectList(pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
            }
            else
            {
                viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());
                //viewModel.Pacientes = new SelectList(pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
                //viewModel.Medicos = new SelectList(medicos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
                //viewModel.Empresas = new SelectList(empresas.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                //viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                //viewModel.UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items.Select(m => new { Id = m.Id, DisplayName = string.Format("{0}", m.DisplayName) }), "Id", "DisplayName");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/_CriarOuEditarModal.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SalvarAtendimento(AtendimentoDto ambulatorioEmergencia)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                AtendimentoDto relacao = new AtendimentoDto();
                await atendimentoAppService.Object.CriarOuEditar(ambulatorioEmergencia);
                return Content(L("Sucesso"));
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
    }
}