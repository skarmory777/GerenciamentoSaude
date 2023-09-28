using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class RateioCentroCustosController : SWMANAGERControllerBase
    {
        private readonly IRateioCentroCustoAppService _rateioCentroCustoAppService;

        public RateioCentroCustosController(IRateioCentroCustoAppService rateioCentroCustoAppService)
        {
            _rateioCentroCustoAppService = rateioCentroCustoAppService;
        }



        public ActionResult Index()
        {
            var model = new RateioCentroCustoViewModel(new RateioCentroCustoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/RateioCentroCusto/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            RateioCentroCustoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new RateioCentroCustoViewModel(new RateioCentroCustoDto());
                viewModel.CentrosCustos = JsonConvert.SerializeObject(new List<RateioCentroCustoItemDto>());
            }
            else
            {
                var rateioCentroCusto = await _rateioCentroCustoAppService.Obter((long)id);

                rateioCentroCusto.CentrosCustos = JsonConvert.SerializeObject(rateioCentroCusto.RateioCentroCustoItensDto);

                viewModel = new RateioCentroCustoViewModel(rateioCentroCusto);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/RateioCentroCusto/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}