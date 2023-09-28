using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Entradas;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class EntradasController : Controller // Web.Controllers.SWMANAGERControllerBase
    {
        private readonly IEstoqueAppService _estoqueAppService;
        private readonly IEntradaAppService _entradaAppService;
        private readonly ICfopAppService _cfopAppService;
        private readonly IEntradaItemAppService _entradaItemAppService;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly IFornecedorAppService _fornecedorAppService;
        //  private readonly ITipoDocumentoAppService _tipoDocumentoAppService;
        private readonly ICentroCustoAppService _centroCustoAppService;
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession AbpSession;

        public EntradasController(
            IEstoqueAppService estoqueAppService,
            ICfopAppService cfopAppService,
            IEntradaAppService entradaAppService,
            IEntradaItemAppService entradaItemAppService,
            IEmpresaAppService empresaAppService,
            IFornecedorAppService fornecedorAppService,
            //  ITipoDocumentoAppService tipoDocumentoAppService,
            ICentroCustoAppService centroCustoAppService,
            IUserAppService userAppService,
            IAbpSession abpSession
            )
        {
            _estoqueAppService = estoqueAppService;
            _cfopAppService = cfopAppService;
            _centroCustoAppService = centroCustoAppService;
            // _tipoDocumentoAppService = tipoDocumentoAppService;
            _fornecedorAppService = fornecedorAppService;
            _empresaAppService = empresaAppService;
            _entradaAppService = entradaAppService;
            _entradaItemAppService = entradaItemAppService;
            _userAppService = userAppService;
            AbpSession = abpSession;
        }

        public ActionResult Index()
        {
            var model = new EntradasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Entradas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var estoques = await _estoqueAppService.ListarTodos();
            var empresas = await _empresaAppService.ListarTodos();
            //  var fornecedor = await _fornecedorAppService.ListarTodos();
            var centroCusto = await _centroCustoAppService.ListarTodos();
            //  var tipoDocumento = await _tipoDocumentoAppService.ListarTodos();
            var cfop = await _cfopAppService.ListarTodos();
            //Instanciar user
            //var userId = AbpSession.UserId.Value;
            //var users = await _userAppService.GetAllUsers();
            //var userTmp = users.Items.Where(m => m.Id == AbpSession.UserId.Value).FirstOrDefault();
            //var user = new UserEditDto {
            //    EmailAddress =userTmp.EmailAddress,
            //    Id=userTmp.Id
            //};

            var userId = AbpSession.UserId.Value;
            var userEmpresas = await _userAppService.GetUserEmpresas(userId);

            CriarOuEditarEntradaModalViewModel viewModel;

            if (id.HasValue) //edição
            {
                var output = await _entradaAppService.Obter((long)id);
                viewModel = new CriarOuEditarEntradaModalViewModel(output);
                //
                viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia", output.EmpresaId);  //SelectList(empresas.Items, "Id", "NomeFantasia", output.EmpresaId);
                                                                                                                  //viewModel.Fornecedores = new SelectList(fornecedor.Items, "Id", "Id", output.FornecedorId);
                                                                                                                  //   viewModel.TiposDocumento = new SelectList(tipoDocumento.Items, "Id", "Descricao", output.TipoDocumentoId);
                viewModel.CentrosCustos = new SelectList(centroCusto.Items, "Id", "Descricao", output.CentroCustoId);
                viewModel.Cfops = new SelectList(cfop.Items, "Id", "Numero", output.CfopId);
                viewModel.Estoques = new SelectList(estoques.Items, "Id", "Descricao", output.EstoqueId);
            }
            else //Novo
            {
                viewModel = new CriarOuEditarEntradaModalViewModel(new CriarOuEditarEntrada());
                //
                viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia");
                // viewModel.Fornecedores = new SelectList(fornecedor.Items, "Id", "Id");
                // viewModel.TiposDocumento = new SelectList(tipoDocumento.Items, "Id", "Descricao");
                viewModel.CentrosCustos = new SelectList(centroCusto.Items, "Id", "Descricao");
                viewModel.Cfops = new SelectList(cfop.Items, "Id", "Numero");
                viewModel.Estoques = new SelectList(estoques.Items, "Id", "Descricao");
            }
            //viewModel.UpdateUser = user;
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Entradas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _entradaAppService.ListarAutoComplete(term);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult EditarEntradaItem(CriarOuEditarEntradaItem input)
        {
            try
            {
                //_entradaItemAppService.Editar(input);
                AsyncHelper.RunSync(() => _entradaItemAppService.Editar(input));
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarEntradaItem(CriarOuEditarEntradaItem input)
        {
            try
            {
                var objItem = AsyncHelper.RunSync(() => _entradaItemAppService.CriarOuEditar(input, input.EntradaId));
                return Json(new { Result = "OK", Record = objItem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult ExcluirEntradaItem(long id)
        {
            try
            {
                //input. EntradaListaSubstituicaoDto
                AsyncHelper.RunSync(() => _entradaItemAppService.Excluir(id));
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}