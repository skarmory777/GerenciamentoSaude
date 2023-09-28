using Abp.Application.Services.Dto;
using Abp.Dependency;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.TabelaPrecoConvenios;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Taxas;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoTabelaPrecoConveniosController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            var model = new FaturamentoTabelaPrecoConveniosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/TabelaPrecoConvenios/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditar(long? id)
        {
            using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            {
                var viewModel = new FaturamentoConfigConvenioDto
                {
                    Convenio = await convenioAppService.Object.ObterDto((long)id).ConfigureAwait(false),
                    ConvenioId = id
                };

                ListResultDto<EmpresaDto> empresas = await userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value).ConfigureAwait(false);
                viewModel.Empresa = empresas.Items[0];

                return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/TabelaPrecoConvenios/CriarOuEditar.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            {
                var viewModel = new FaturamentoConfigConvenioDto
                {
                    Convenio = await convenioAppService.Object.ObterDto((long)id).ConfigureAwait(false),
                    ConvenioId = id
                };

                ListResultDto<EmpresaDto> empresas = await userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value).ConfigureAwait(false);
                viewModel.Empresa = empresas.Items[0];

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/TabelaPrecoConvenios/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> _Taxa(long? id)
        {
            FaturamentoTaxaViewModel viewModel;
            using (var taxaAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoTaxaAppService>())
            using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            {
                if (id.HasValue)
                {
                    var taxaDto = await taxaAppService.Object.Obter((long)id);
                    viewModel = new FaturamentoTaxaViewModel(taxaDto);
                }
                else
                {
                    viewModel = new FaturamentoTaxaViewModel(new FaturamentoTaxaDto());
                }

                viewModel.Convenio = await convenioAppService.Object.ObterDto((long)id);
                viewModel.ConvenioId = id;
                ListResultDto<EmpresaDto> empresas = await userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value);
                viewModel.Empresa = empresas.Items.FirstOrDefault();

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/TabelaPrecoConvenios/_Taxa.cshtml", viewModel);
            }
        }
    }
}