using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.Operacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Configuracoes
{
    public class OperacoesController : SWMANAGERControllerBase
    {
        private readonly IOperacaoAppService _operacaoAppService;
        private readonly IModuloAppService _moduloAppService;
        private readonly IBiAppService _biAppService;

        public OperacoesController(
            IOperacaoAppService operacaoAppService,
            IModuloAppService moduloAppService,
            IBiAppService biAppService

            )
        {
            _operacaoAppService = operacaoAppService;
            _moduloAppService = moduloAppService;
            _biAppService = biAppService;
        }

        public ActionResult Index()
        {
            var model = new OperacoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Operacoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Configuracoes_Operacao_Create, AppPermissions.Pages_Tenant_Configuracoes_Operacao_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long moduloId, long? id)
        {
            CriarOuEditarOperacaoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _operacaoAppService.Obter(id.Value);
                viewModel = new CriarOuEditarOperacaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarOperacaoModalViewModel(new OperacaoDto());
            }

            var modulo = await _moduloAppService.Obter(moduloId);
            viewModel.Modulo = modulo;
            viewModel.ModuloId = moduloId;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Operacoes/_CriarOuEditarModal.cshtml", viewModel);
        }

        public OperacaoDto DefinirOperacaoAtual(string name)
        {
            var operacao = Task.Run(() => _operacaoAppService.ObterPorNome(name)).Result;
            TempData["ActivePage"] = name;
            if (operacao != null)
            {
                TempData["OperacaoId"] = operacao.Id;

                var bi = Task.Run(() => _biAppService.ObterPorOperacao(operacao.Id)).Result;
                if (bi != null)
                {
                    TempData["Bi"] = bi;
                }
            }
            return operacao;
        }

    }
}