using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.PainelSenhas;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class PainelSenhasController : Controller //SWMANAGERControllerBase
    {
        private readonly IPainelAppService _painelAppService;

        public PainelSenhasController(
           IPainelAppService painelAppService
            )
        {
            _painelAppService = painelAppService;
        }

        // GET: Mpa/Grupo
        public ActionResult Index()
        {
            var model = new PainelSenhaViewModel(new PainelDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/PainelSenhas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            PainelSenhaViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _painelAppService.Obter((long)id);
                viewModel = new PainelSenhaViewModel(output);

                List<TipoLocalChamadaIndex> tiposLocalChamadaIndex = new List<TipoLocalChamadaIndex>();

                long gridId = 0;

                foreach (var item in output.PaineisTipoLocaisChamadasDto)
                {
                    TipoLocalChamadaIndex index = new TipoLocalChamadaIndex();

                    index.Id = item.Id;
                    index.TipoLocalChamdaId = item.TipoLocalChamada.Id;
                    index.TipoLocalChamadaDescricao = item.TipoLocalChamada.Descricao;
                    index.GridId = ++gridId;
                    tiposLocalChamadaIndex.Add(index);

                }


                //var classesList = await _grupoClasseAppService.ListarPorGrupo(id.Value);
                viewModel.TipoLocalChamadas = JsonConvert.SerializeObject(tiposLocalChamadaIndex);
            }
            else
            {
                List<TipoLocalChamadaIndex> tiposLocalChamadaIndex = new List<TipoLocalChamadaIndex>();
                viewModel = new PainelSenhaViewModel(new PainelDto());
                viewModel.TipoLocalChamadas = JsonConvert.SerializeObject(tiposLocalChamadaIndex);
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/PainelSenhas/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}