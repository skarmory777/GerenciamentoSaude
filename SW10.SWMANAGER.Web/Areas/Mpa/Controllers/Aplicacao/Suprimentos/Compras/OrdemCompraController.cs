using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos.Compras
{
    public class OrdemCompraController : Controller 
    {
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession _abpSession;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly IOrdemCompraAppService _ordemCompraAppService;
        private readonly IFormaPagamentoAppService _formaPagamentoAppService;

        public OrdemCompraController(
            IUserAppService userAppService,
            IAbpSession abpSession,
            IEmpresaAppService empresaAppService,
            IOrdemCompraAppService ordemCompraAppService,
            IFormaPagamentoAppService formaPagamentoAppService
            )
        {
            _userAppService = userAppService;
            _abpSession = abpSession;
            _empresaAppService = empresaAppService;
            _ordemCompraAppService = ordemCompraAppService;
            _formaPagamentoAppService = formaPagamentoAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new OrdemCompraViewModel(new OrdemCompraDto());

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Compras/OrdemCompra/Index.cshtml", model);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra_Create, AppPermissions.Pages_Tenant_Suprimentos_OrdemCompra_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var userId = _abpSession.UserId.Value;
            var userEmpresas = await _userAppService.GetUserEmpresas(userId);

            var viewModel = new CriarOuEditarOrdemCompraViewModel(new OrdemCompraDto());

            var isEdicao = id.HasValue;

            if (isEdicao)
            {
                var output = await _ordemCompraAppService.Obter((long)id);
                viewModel = new CriarOuEditarOrdemCompraViewModel(output);

                var itensList = await _ordemCompraAppService.ListarRequisicaoItem(id.Value);
                viewModel.OrdemCompraItensJson = JsonConvert.SerializeObject(itensList.Items.ToList());
            }
            else //Novo
            {
                viewModel.OrdemCompraItensJson = JsonConvert.SerializeObject(new List<OrdemCompraDto>());
            }
            //  //viewModel.UpdateUser = user;
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Compras/OrdemCompra/_CriarOuEditarModal.cshtml", viewModel);
        }

        [UnitOfWork(false)]
        public ActionResult GerarRelatorio(long ordemCompraId)
        {
            return null;
            //using (var compraRequisicaoAppService = IocManager.Instance.ResolveAsDisposable<IOrdemCompraAppService>())
            //{
            //    return File(compraRequisicaoAppService.Object.GerarRelatorioCompraRequisicao(new CompraRequisicaoRelatorioDto()
            //    {
            //        CompraRequisicaoId = compraRequisicaoId
            //    }), "application/pdf", $"relatorio-compra-requisicao.pdf");
            //}
        }
    }
}