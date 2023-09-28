using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class DivisoesController : SWMANAGERControllerBase
    {
        private readonly IDivisaoAppService _divisaoAppService;
        private readonly IPrescricaoItemRespostaAppService _prescricaoItemRespostaAppService;
        private readonly ITipoPrescricaoAppService _tipoPrescricaoAppService;

        public DivisoesController(
            IDivisaoAppService divisaoAppService,
            IPrescricaoItemRespostaAppService prescricaoItemRespostaAppService
            )
        {
            _divisaoAppService = divisaoAppService;
            _prescricaoItemRespostaAppService = prescricaoItemRespostaAppService;
        }

        // GET: Mpa/Divisao
        public ActionResult Index()
        {
            var model = new DivisaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/Index.cshtml", model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_DivisaoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_DivisaoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            //var tiposRespostas = await _prescricaoItemRespostaAppService.ListarTodos();
            CriarOuEditarDivisaoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _divisaoAppService.Obter((long)id); //_Divisoeservice.GetDivisoes(new GetDivisoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarDivisaoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarDivisaoViewModel(new DivisaoDto());
            }
            //viewModel.TiposRespostasDisponiveis = tiposRespostas.Items.ToList();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _CriarOuEditarSubDivisaoModal(long divisaoPrincipalId, long? id)
        {
            var tiposRespostas = await _prescricaoItemRespostaAppService.ListarTodos();
            CriarOuEditarDivisaoViewModel viewModel;
            var output = new DivisaoDto();
            var divisaoPrincipal = await _divisaoAppService.Obter(divisaoPrincipalId);
            output.DivisaoPrincipal = divisaoPrincipal;
            output.DivisaoPrincipalId = divisaoPrincipalId;
            if (id.HasValue)
            {
                output = await _divisaoAppService.Obter((long)id); //_Divisoeservice.GetDivisoes(new GetDivisoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarDivisaoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarDivisaoViewModel(output);
            }
            ///viewModel.TiposRespostasDisponiveis = tiposRespostas.Items.ToList();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_CriarOuEditarSubDivisaoModal.cshtml", viewModel);
        }

        public PartialViewResult _MontarTiposRespostas(CriarOuEditarDivisaoViewModel model)
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/Index.cshtml", model);
        }

        public PartialViewResult _SelecionarSubDivisaoModal(DivisaoViewModel model)
        {
            ViewBag.DivisaoPrincipalId = model.Filtro;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_SelecionarSubDivisaoModal.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarTipoPrescricaoModal(long divisaoId, long? id)
        {
            var output = new TipoPrescricaoDto();
            if (id.HasValue)
            {
                output = await _tipoPrescricaoAppService.Obter(id.Value);
            }
            var viewModel = new CriarOuEditarTipoPrescricaoViewModel(output);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_CriarOuEditarTipoPrescricaoModal.cshtml", viewModel);
        }

        public ActionResult _SelecionarTiposPrescricaoModal(long divisaoId)
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_SelecionarTiposPrescricaoModal.cshtml");
        }

    }

}