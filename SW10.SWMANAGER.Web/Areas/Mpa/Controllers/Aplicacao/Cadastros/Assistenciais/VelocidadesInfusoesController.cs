using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class VelocidadesInfusoesController : SWMANAGERControllerBase
    {
        private readonly IVelocidadeInfusaoAppService _velocidadeInfusaoAppService;

        public VelocidadesInfusoesController(
            IVelocidadeInfusaoAppService velocidadeInfusaoAppService
            )
        {
            _velocidadeInfusaoAppService = velocidadeInfusaoAppService;
        }

        public ActionResult Index()
        {
            var model = new VelocidadeInfusaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/VelocidadesInfusoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_VelocidadeInfusao_Edit)]
        public async Task<PartialViewResult> CriarOuEditar(long? id)
        {
            using (var formaApplicacao = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
            {
                CriarOuEditarVelocidadeInfusaoViewModel viewModel;
                if (id.HasValue)
                {
                    var output = await _velocidadeInfusaoAppService.Obter(id.Value);
                    viewModel = new CriarOuEditarVelocidadeInfusaoViewModel(output);
                    viewModel.ListFormaAplicacao = FormaAplicacaoDto.Mapear(formaApplicacao.Object.GetAllList()).ToList();
                }
                else
                {
                    viewModel = new CriarOuEditarVelocidadeInfusaoViewModel(new VelocidadeInfusaoDto());
                    viewModel.ListFormaAplicacao = FormaAplicacaoDto.Mapear(formaApplicacao.Object.GetAllList()).ToList();
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/VelocidadesInfusoes/_CriarOuEditarModal.cshtml", viewModel);
            }
        }


    }

}