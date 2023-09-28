using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.AtendimentosLeitosMov.Altas;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class AltaController : SWMANAGERControllerBase
    {

        // GET: Mpa/Alta
        public ActionResult Index()
        {
            var viewModel = new AltaModalViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/Index.cshtml", viewModel);
        }

        //public async Task<PartialViewResult> AltaModal(dynamic data)
        //{

        // //   CriarOuEditarAltaViewModel viewmodel = new CriarOuEditarAltaViewModel(data);
        // //   viewmodel.AltaMedicaViewModel = new CriarOuEditarAltaMedicaViewModel(new ClassesAplicacao.Services.Atendimentos.AltasMedicas.Dto.AltaMedicaDto());

        //    // var altamedica = await _altamedicaappservice.listar(alta);
        //    //viewmodel.altamedica = altamedica.items.tolist();
        ////    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.cshtml", viewmodel);
        //}
    }
}