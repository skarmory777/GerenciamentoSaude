#region Usings

using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasMedicas;
using SW10.SWMANAGER.Web.Controllers;

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class EntregaContasController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            var model = new ContasMedicasViewModel();
            using (var faturamentoContaStatusAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaStatusAppService>())
            {
                model.ListaStatus = faturamentoContaStatusAppService.Object.ListarTodos();
                return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/EntregaContas/Index.cshtml", model);
            }
        }

        public async Task<ActionResult> ConferenciaModal(long? id)
        {
            CriarOuEditarContaMedicaModalViewModel viewModel;

            if (id.HasValue)
            {
                using (var _contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var _atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var _configConvenioAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoConfigConvenioAppService>())
                {

                    var output = await _contaMedicaAppService.Object.ObterViewModel((long)id);
                    viewModel = new CriarOuEditarContaMedicaModalViewModel(output);
                    viewModel.Atendimento = await _atendimentoAppService.Object.Obter((long)viewModel.AtendimentoId);// ta demorando este

                    ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();
                    configConvenioInput.Filtro = output.ConvenioId.ToString();
                    var configsConvenio = await _configConvenioAppService.Object.ListarPorConvenio(configConvenioInput);

                    // Filtrar por empresa
                    var configsPorEmpresa = configsConvenio.Items
                        .Where(c => c.EmpresaId == output.EmpresaId);

                    viewModel.configsPorEmpresa = configsPorEmpresa.ToArray();

                    // Filtrar por plano
                    var configsPorPlano = configsPorEmpresa
                        .Where(x => x.PlanoId != null)
                        .Where(c => c.PlanoId == output.PlanoId);

                    viewModel.configsPorPlano = configsPorPlano.ToArray();
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaMedicaModalViewModel(new FaturamentoContaDto());
                viewModel.Atendimento = new ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto.AtendimentoDto();
                viewModel.EmpresaId = 0; // CORRIGIR PARA EMPRESA LOGADA
                viewModel.ConvenioId = 0; // CORRIGIR ?
                viewModel.PlanoId = 0; // CORRIGIR ?
            }

            viewModel.Conferencia = true;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/EntregaContas/ConferenciaModal.cshtml", viewModel);
        }



    }


}