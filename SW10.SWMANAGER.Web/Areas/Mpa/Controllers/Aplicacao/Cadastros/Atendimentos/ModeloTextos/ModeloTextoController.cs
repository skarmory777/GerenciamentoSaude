using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.ModeloTextos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Atendimentos.ModeloTextos
{
    public class ModeloTextoController : SWMANAGERControllerBase
    {

        private readonly IModeloTextoAppService _modeloTextoAppService;

        public ModeloTextoController(IModeloTextoAppService modeloTextoAppService)
        {
            _modeloTextoAppService = modeloTextoAppService;
        }

        public ActionResult Index()
        {
            var model = new ModeloTextoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/ModeloTextos/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            ModeloTextoViewModel viewModel;

            if (id == null || id == 0)
            {

                viewModel = new ModeloTextoViewModel();
                //viewModel.TiposGuias = JsonConvert.SerializeObject(new List<TipoGuiaIndex>());
            }
            else
            {
                var movimentoAutomatico = await _modeloTextoAppService.Obter((long)id);
                viewModel = new ModeloTextoViewModel(movimentoAutomatico);


            }

            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/ModeloTextos/_CriarOuEditarModal.cshtml", viewModel);
        }


        public JsonResult Salvar(TextoModeloDto input)
        {
            try
            {
                //  var preMovimento = JsonConvert.DeserializeObject<EstoquePreMovimentoDto>(input, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                var result = _modeloTextoAppService.CriarOuEditar(input);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}