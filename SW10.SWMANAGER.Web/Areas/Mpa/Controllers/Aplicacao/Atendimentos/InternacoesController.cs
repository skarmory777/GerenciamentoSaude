using Abp.Application.Navigation;
using Abp.Application.Services.Dto;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentosSalaCirurgicas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Favoritos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Visitantes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Orcamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Layout;
using SW10.SWMANAGER.Web.Areas.Mpa.Startup;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Internacoes
{
    public class InternacoesController : SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index(long? id)
        {
            // var pacientes = await _pacienteAppService.Listar (new ListarPacientesInput ());
            // var medicos = await _medicoAppService.ListarTodos ();
            // var convenios = await _convenioAppService.ListarTodos ();
            // var origens = await _origemAppService.ListarTodos ();
            using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            {
                var user = await userManager.Object.GetUserByIdAsync((long)this.AbpSession.UserId).ConfigureAwait(false);
                var empresas = await userAppService.Object.GetUserEmpresas(this.AbpSession.UserId.Value)
                                   .ConfigureAwait(false);

                if (empresas == null || empresas.Items.Count == 0)
                {

                    empresas = await empresaAppService.Object.ListarTodos().ConfigureAwait(false);
                }

                // ListarParaInternacao
                // var unidadesOrganizacionais = await _organizationUnitAppService.GetOrganizationUnits();

                var unidadesOrganizacionais =
                    await unidadeOrganizacionalAppService.Object.ListarParaInternacao().ConfigureAwait(false);

                var viewModel = new InternacoesViewModel
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
                    AgendamentoId = id
                };

                // viewModel.Pacientes = new SelectList (pacientes.Items.Select (m => new { Id = m.Id, Nome = string.Format ("{0} - {1} - {2}", m.NomeCompleto, m.Cpf, m.Nascimento) }), "Id", "Nome");
                // viewModel.Medicos = new SelectList (medicos.Items.Select (m => new { Id = m.Id, Nome = string.Format ("{0}", m.NomeCompleto) }), "Id", "Nome");

                // viewModel.Origens = new SelectList (origens.Items.Select (m => new { Id = m.Id, Descricao = string.Format ("{0}", m.Descricao) }), "Id", "Descricao");
                // viewModel.Convenios = new SelectList (convenios.Items.Select (m => new { Id = m.Id, NomeFantasia = string.Format ("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                // viewModel.UnidadesOrganizacionais = unidadesOrganizacionais.Items.ToList();

                // viewModel.MenuItemName = 

                if (empresas != null && empresas.Items.Count == 1)
                {
                    viewModel.Empresa = new EmpresaDto
                    {
                        Id = empresas.Items[0].Id,
                        NomeFantasia = empresas.Items[0].NomeFantasia
                    };
                }

                return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Index.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> MapaLeitos()
        {
            // var _userManager = this.iocResolver.Resolve<UserManager>();
            // var _userAppService = this.iocResolver.Resolve<IUserAppService>();
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            {
                var unidadesOrganizacionais =
                    await unidadeOrganizacionalAppService.Object.ListarParaInternacao().ConfigureAwait(false);

                var model = new InternacoesViewModel
                {
                    UnidadesOrganizacionais = unidadesOrganizacionais.Items.ToList()
                };

                // model.Empresas = new SelectList(empresas.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/MapaLeitos/Index.cshtml",
                    model);
            }
        }

        public async Task<PartialViewResult> PrevisaoEntradas()
        {
            using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            {
                var user = await userManager.Object.GetUserByIdAsync((long)this.AbpSession.UserId).ConfigureAwait(false);
                var empresas = await userAppService.Object.GetUserEmpresas(this.AbpSession.UserId.Value)
                                   .ConfigureAwait(false);

                if (empresas == null || empresas.Items.Count == 0)
                {


                    empresas = await empresaAppService.Object.ListarTodos().ConfigureAwait(false);
                }

                // var unidadesOrganizacionais = await _organizationUnitAppService.GetOrganizationUnits();
                // var _unidadeOrganizacionalAppService = this.iocResolver.Resolve<IUnidadeOrganizacionalAppService>();
                // var unidadesOrganizacionais = await _unidadeOrganizacionalAppService.ListarParaInternacao();
                var model = new InternacoesViewModel
                {
                    Empresas = new SelectList(
                                        empresas.Items.Select(
                                            m => new
                                            {
                                                Id = m.Id,
                                                NomeFantasia = string.Format("{0}", m.NomeFantasia)
                                            }),
                                        "Id",
                                        "NomeFantasia")
                };

                // model.UnidadesOrganizacionais = unidadesOrganizacionais.Items.ToList();
                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/PrevisaoEntradas/Index.cshtml",
                    model);
            }
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> Visitantes()
        {
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Visitantes/Index.cshtml",
                new VisitantesViewModel());
        }

        /// <summary>
        /// Chamar o ViewModel do cadastro de visitante
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CriarOuEditarVisitanteModal(long? id)
        {
            // var paises = await _paisAppService.Listar(new ListarPaisesInput());
            CriarOuEditarVisitanteModalViewModel viewModel;
            try
            {
                if (id.HasValue)
                {
                    using (var visitanteAppService = IocManager.Instance.ResolveAsDisposable<IVisitanteAppService>())
                    {
                        var output = await visitanteAppService.Object.Obter((long)id).ConfigureAwait(false);
                        viewModel = new CriarOuEditarVisitanteModalViewModel(output);
                    }

                    // viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome", output.PaisId);
                }
                else
                {
                    viewModel = new CriarOuEditarVisitanteModalViewModel(new VisitanteDto());

                    // viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome");
                }
            }
            catch (Exception ex)
            {
                return this.Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }



            // var viewModel = new CriarOuEditarVisitanteModalViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Visitantes/_CriarOuEditarModal.cshtml", viewModel);

            // return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Visitantes/Index.cshtml");
        }



        public async Task<ActionResult> CriarOuEditarVisitantePacienteModal(long? id)
        {
            // var paises = await _paisAppService.Listar(new ListarPaisesInput());
            CriarOuEditarVisitanteModalViewModel viewModel;
            try
            {
                if (id.HasValue)
                {
                    using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                    {
                        var atendimento = await atendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);

                        var output = new VisitanteDto();

                        if (atendimento != null)
                        {
                            output.AtendimentoId = atendimento.Id;
                            output.NomePacinete = atendimento.Paciente.NomeCompleto;
                            output.Atendimento = atendimento;
                        }

                        output.IsVisitante = true;
                        output.IsInternado = true;

                        viewModel = new CriarOuEditarVisitanteModalViewModel(output);
                    }

                    // viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome", output.PaisId);
                }
                else
                {
                    viewModel = new CriarOuEditarVisitanteModalViewModel(new VisitanteDto());

                    // viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome");
                }
            }
            catch (Exception ex)
            {
                return this.Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }


            // var viewModel = new CriarOuEditarVisitanteModalViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Visitantes/_CriarOuEditarModal.cshtml", viewModel);

            // return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Visitantes/Index.cshtml");
        }


        public async Task<ListResultDto<LeitoComAtendimentoDto>> ListarLeitos()
        {
            using (var leitoAppService = IocManager.Instance.ResolveAsDisposable<ILeitoAppService>())
            {
                var leitos = await leitoAppService.Object.ListarTodos().ConfigureAwait(false);
                return leitos;
            }

            // [AcceptVerbs("GET", "POST", "PUT")]
            // public JsonResult CriarProdutoRelacaoAcaoTerapeutica(ProdutoRelacaoAcaoTerapeuticaDto input)
            // {
            // try
            // {
            // var objResult = AsyncHelper.RunSync(() => _produtoRelacaoAcaoTerapeuticaAppService.CriarOuEditar(input, input.Id));
            // return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
            // }
            // catch (Exception ex)
            // {
            // return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            // }
            // }
        }

        [ChildActionOnly]
        public PartialViewResult Header()
        {
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            using (var multiTenancyConfig = IocManager.Instance.ResolveAsDisposable<IMultiTenancyConfig>())
            using (var languageManager = IocManager.Instance.ResolveAsDisposable<ILanguageManager>())
            {
                var headerModel = new HeaderViewModel
                {
                    LoginInformations = AsyncHelper.RunSync(() => sessionAppService.Object.GetCurrentLoginInformations()),
                    Languages = languageManager.Object.GetLanguages(),
                    CurrentLanguage = languageManager.Object.CurrentLanguage,
                    IsMultiTenancyEnabled = multiTenancyConfig.Object.IsEnabled,
                    IsImpersonatedLogin = this.AbpSession.ImpersonatorUserId.HasValue
                };

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Layout/_Header.cshtml",
                    headerModel);
            }
        }

        [ChildActionOnly]
        public PartialViewResult Favoritos(string currentPageName = "", string menuName = MpaNavigationProvider.MenuName)
        {
            using (var favoritoAppService = IocManager.Instance.ResolveAsDisposable<IFavoritoAppService>())
            {
                var userIdentifier = this.AbpSession.ToUserIdentifier();
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

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Layout/_Favoritos.cshtml",
                    favoritosModel);
            }
        }

        [HttpPost]
        public async Task<long> CriarNovoAtendimento()
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var id = await atendimentoAppService.Object.CriarNovoAtendimento().ConfigureAwait(false);
                return id;
            }
        }

        public async Task<ActionResult> ModalPacientes()
        {
            var model = new PacientesViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Pacientes/Index.cshtml", model);
        }

        public async Task<ActionResult> ModalPreAtendimentos()
        {
            var model = new PreAtendimentosViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/PreAtendimentos/Index.cshtml", model);
        }

        public async Task<ActionResult> ModalOrcamentos()
        {
            var model = new OrcamentosViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Orcamentos/Index.cshtml", model);
        }

        public async Task<ActionResult> ModalClassificacaoRiscos()
        {
            var model = new ClassificacoesRiscoViewModel();

            // var model = new ClassificacaoRiscosViewModel();
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/ClassificacaoRiscos/Index.cshtml",
                model);
        }

        [HttpPost]
        public void AtendimentoAtual(CriarOuEditarAtendimento item)
        {
            this.TempData["AtendimentoAtual"] = item.Id;
        }

        [HttpPost]
        public void NovoAtendimento(long? item)
        {
            this.TempData["AtendimentoAtual"] = 0;
        }

        public PartialViewResult _MenuTopo()
        {
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Layout/_MenuTopo.cshtml");
        }

        public PartialViewResult _PreAtendimento()
        {
            var model = new PreAtendimentosViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/PreInternacoes/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarPreAtendimento()
        {
            CriarOuEditarPreAtendimentoModalViewModel viewModel;
            viewModel = new CriarOuEditarPreAtendimentoModalViewModel(new CriarOuEditarPreAtendimento());

            // var sexos = await _sexoAppService.ListarTodos();
            // viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/PreInternacoes/_CriarOuEditarModal.cshtml",
                viewModel);
        }

        public PartialViewResult _ClassificacaoRisco()
        {
            var model = new ClassificacoesRiscoViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/ClassificacoesRisco/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarClassificacaoRisco()
        {
            // var especialidades = await _especialidadeAppService.ListarTodos();
            CriarOuEditarClassificacaoRiscoModalViewModel viewModel;

            viewModel = new CriarOuEditarClassificacaoRiscoModalViewModel(new CriarOuEditarClassificacaoRisco());

            // viewModel.Especialidades = new SelectList(especialidades.Items, "Id", "Nome");
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/ClassificacoesRisco/_CriarOuEditarModal.cshtml",
                viewModel);
        }

        public PartialViewResult _PesquisarPaciente()
        {
            var model = new PacientesViewModel();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Pacientes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            // var pacientes = await _pacienteAppService.Listar(new ListarPacientesInput());
            // var medicos = await _medicoAppService.ListarTodos();
            // var empresas = await _empresaAppService.ListarTodos();
            // var convenios = await _convenioAppService.ListarTodos();
            // var unidadesOrganizacionais = await _organizationUnitAppService.GetOrganizationUnits();
            CriarOuEditarAtendimentoModalViewModel viewModel;

            if (id.HasValue)
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var output = await atendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(output);
                }

                // viewModel.Pacientes = new SelectList(pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
            }
            else
            {
                viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());

                // viewModel.Pacientes = new SelectList(pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
                // viewModel.Medicos = new SelectList(medicos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
                // viewModel.Empresas = new SelectList(empresas.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                // viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                // viewModel.UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items.Select(m => new { Id = m.Id, DisplayName = string.Format("{0}", m.DisplayName) }), "Id", "DisplayName");
                viewModel.IsInternacao = true;
            }

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/_CriarOuEditarModal.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SalvarAtendimento(AtendimentoDto ambulatorioEmergencia)
        {
            // AtendimentoDto relacao = new AtendimentoDto ();
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                await atendimentoAppService.Object.CriarOuEditar(ambulatorioEmergencia).ConfigureAwait(false);
                return this.Content(this.L("Sucesso"));
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

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_Edit)]
        public async Task<PartialViewResult> CriarModalPorAgendamento(long id)
        {
            // var pacientes = await _pacienteAppService.Listar(new ListarPacientesInput());
            // var medicos = await _medicoAppService.ListarTodos();
            // var empresas = await _empresaAppService.ListarTodos();
            // var convenios = await _convenioAppService.ListarTodos();
            // var unidadesOrganizacionais = await _organizationUnitAppService.GetOrganizationUnits();
            var viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());

            // if (id.HasValue)
            // {
            // var output = await _atendimentoAppService.Obter((long)id);
            // viewModel = new CriarOuEditarAtendimentoModalViewModel(output);
            // viewModel.Pacientes = new SelectList(pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
            // }
            // else
            // {
            // viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());
            // viewModel.Pacientes = new SelectList(pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
            // viewModel.Medicos = new SelectList(medicos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }), "Id", "Nome");
            // viewModel.Empresas = new SelectList(empresas.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
            // viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
            // viewModel.UnidadesOrganizacionais = new SelectList(unidadesOrganizacionais.Items.Select(m => new { Id = m.Id, DisplayName = string.Format("{0}", m.DisplayName) }), "Id", "DisplayName");
            // viewModel.IsInternacao = true;
            // }


            viewModel.IsInternacao = true;

            using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            {
                var agendamento = await agendamentoSalaCirurgicaAppService.Object.ObterCirurgico(id).ConfigureAwait(false);
                if (agendamento != null)
                {
                    viewModel.Paciente = agendamento.Paciente;
                    viewModel.PacienteId = agendamento.PacienteId;
                    viewModel.Convenio = agendamento.Convenio;
                    viewModel.ConvenioId = agendamento.ConvenioId;
                    viewModel.Plano = agendamento.Plano;
                    viewModel.PlanoId = agendamento.PlanoId;
                }
            }

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}