using Abp.Application.Navigation;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.ControleProducoes;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ControleProducoesController : SWMANAGERControllerBase
    {
        private readonly IControleProducaoAppService _controleProducaoAppService;
        private readonly IConsultorTabelaAppService _consultorTabelaAppService;
        private readonly IUserAppService _userAppService;
        private readonly IUserNavigationManager _userNavigationManager;

        public ControleProducoesController(
            IControleProducaoAppService controleProducaoAppService,
            IConsultorTabelaAppService consultorTabelaAppService,
            IUserAppService userAppService,
            IUserNavigationManager userNavigationManager
            )
        {
            _controleProducaoAppService = controleProducaoAppService;
            _consultorTabelaAppService = consultorTabelaAppService;
            _userAppService = userAppService;
            _userNavigationManager = userNavigationManager;
        }

        public ActionResult Index()
        {
            var model = new CriarOuEditarControleProducaoModalViewModel(new CriarOuEditarControleProducao());
            return View("~/Areas/Mpa/Views/Aplicacao/Desenvolvimento/ControleProducoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Create, AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var tabelas = await _consultorTabelaAppService.Listar(new ListarConsultorTabelasInput());
            var usuarios = await _userAppService.GetUsers(new GetUsersInput());

            CriarOuEditarControleProducaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _controleProducaoAppService.Obter((long)id);
                viewModel = new CriarOuEditarControleProducaoModalViewModel(output);
                viewModel.TabelasSistema = new SelectList(tabelas.Items, "Id", "Nome");
                viewModel.Usuarios = new SelectList(usuarios.Items, "Id", "Name");
            }
            else
            {
                viewModel = new CriarOuEditarControleProducaoModalViewModel(new CriarOuEditarControleProducao());
                viewModel.TabelasSistema = new SelectList(tabelas.Items, "Id", "Nome");
                viewModel.Usuarios = new SelectList(usuarios.Items, "Id", "Name");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Desenvolvimento/ControleProducoes/_CriarOuEditarModal.cshtml", viewModel);
        }

        //public SelectList ObterItensMenu ()
        //{
        //    var fullMenuList = new List<GenericoIdNome> ();
        //    var menu = AsyncHelper.RunSync (() => _userNavigationManager.GetMenuAsync (MpaNavigationProvider.MenuName, AbpSession.ToUserIdentifier ()));
        //    foreach (var menuRaiz in menu.Items)
        //    {
        //        var count = -1;
        //        var fullMenu = new GenericoIdNome ();
        //        string strMenu = menuRaiz.DisplayName;
        //        if (menuRaiz.Items.Count () > 0)
        //        {
        //            foreach (var MenuNivel1 in menuRaiz.Items)
        //            {
        //                strMenu += string.Format ("/{0}", MenuNivel1.DisplayName);
        //                if (MenuNivel1.Items.Count () > 0)
        //                {
        //                    foreach (var MenuNivel2 in menuRaiz.Items)
        //                    {
        //                        strMenu += string.Format ("/{0}", MenuNivel2.DisplayName);
        //                        if (MenuNivel2.Items.Count () > 0)
        //                        {

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        fullMenu.Id = count++;
        //        fullMenu.Nome = strMenu;
        //    }
        //    SelectList cboMenu;
        //    cboMenu = new SelectList (fullMenuList, "Id", "Nome");
        //    return cboMenu;
        //}

    }
}