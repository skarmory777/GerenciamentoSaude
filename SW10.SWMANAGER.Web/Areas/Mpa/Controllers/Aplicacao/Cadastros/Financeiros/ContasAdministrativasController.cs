using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ContasAdministrativasController : SWMANAGERControllerBase
    {
        private readonly IContaAdministrativaAppService _contaAdministrativappService;

        public ContasAdministrativasController(IContaAdministrativaAppService contaAdministrativappService)
        {
            _contaAdministrativappService = contaAdministrativappService;
        }


        public ActionResult Index()
        {
            var model = new ContaAdministrativaViewModel(new ContaAdministrativaDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/ContaAdministrativa/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            ContaAdministrativaViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new ContaAdministrativaViewModel(new ContaAdministrativaDto());
                viewModel.CentrosCustos = JsonConvert.SerializeObject(new List<RateioCentroCustoItemIndex>());
                viewModel.Empresas = JsonConvert.SerializeObject(new List<EmpresaIndex>());
                viewModel.Exibir = "true";
            }
            else
            {
                var contaAdministrativa = await _contaAdministrativappService.Obter((long)id);
                viewModel = new ContaAdministrativaViewModel(contaAdministrativa);

                long i = 0;
                var lstCentroCustos = contaAdministrativa.ContaAdministrativaCustos.Select(s => new RateioCentroCustoItemIndex
                {
                    Id = s.Id,
                    CentroCustoId = s.CentroCustoId,
                    CentroCustoDescricao = string.Concat(s.CentroCusto.CodigoCentroCusto, " - ", s.CentroCusto.Descricao),
                    PercentualRateio = s.Percentual,
                    IdGrid = i++
                });
                viewModel.CentrosCustos = JsonConvert.SerializeObject(lstCentroCustos);


                i = 0;
                var lstEmpresas = contaAdministrativa.ContasAdministrativaEmpresas.Select(s => new EmpresaIndex
                {
                    Id = s.Id,
                    EmpresaId = s.EmpresaId,
                    EmpresaDescricao = string.Concat(s.Empresa.Codigo, " - ", s.Empresa.Descricao),
                    IdGrid = i++
                });


                viewModel.CentrosCustos = JsonConvert.SerializeObject(lstCentroCustos);
                viewModel.Empresas = JsonConvert.SerializeObject(lstEmpresas);

                viewModel.Exibir = contaAdministrativa.RateioCentroCustoId == null ? "true" : "false";



            }



            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/ContaAdministrativa/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}