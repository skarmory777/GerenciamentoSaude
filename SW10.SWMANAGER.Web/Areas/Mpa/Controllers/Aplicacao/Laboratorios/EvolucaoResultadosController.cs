using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ExamesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Laboratorios;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Laboratorios
{
    public class EvolucaoResultadosController : Controller
    {
        private readonly IResultadoAppService _resultadoAppService;
        private readonly IRegistroArquivoAppService _registroArquivoService;
        private readonly IExameStatusAppService _exameStatusAppService;
        private readonly IEvolucaoResultadosAppService _evolucaoResultadosAppService;

        public EvolucaoResultadosController(IResultadoAppService resultadoAppService
            , IRegistroArquivoAppService registroArquivoService
            , IExameStatusAppService exameStatusAppService, IEvolucaoResultadosAppService evolucaoResultadosAppService)
        {
            _resultadoAppService = resultadoAppService;
            _registroArquivoService = registroArquivoService;
            _exameStatusAppService = exameStatusAppService;
            _evolucaoResultadosAppService = evolucaoResultadosAppService;
        }
        // GET: Mpa/EvolucaoResultados
        public ActionResult Index()
        {
            var viewModel = new EvolucaoResultadoExameViewModel(new ResultadoLaudoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Laboratorios/EvolucaoResultados/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> ExibirExames(long coletaId)
        {
            var viewModel = new ResultadoExameViewModel();

            var coleta = await _resultadoAppService.Obter(coletaId);

            if (coleta != null)
            {
                viewModel.Paciente = coleta.Atendimento.Paciente.NomeCompleto;
                viewModel.DataColeta = string.Format("{0:dd/MM/yyyy}", coleta.DataColeta);
                viewModel.Codigo = coleta.Nic.ToString();
                viewModel.PacienteId = coleta.Atendimento.PacienteId;
            }

            viewModel.ColetaId = coletaId;
            viewModel.ExamesStatus = _exameStatusAppService.ObterTodos();

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/EvolucaoResultadosComparativos/ListaExames.cshtml", viewModel);
        }

        public async Task<ActionResult> ModalVisualizarExame(long resultadoExameId)
        {
            ResultadoExameViewModel viewModel = new ResultadoExameViewModel
            {
                ResultadoExameId = resultadoExameId
            };

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/EvolucaoResultadosComparativos/_ModalVisualizacaoResultado.cshtml", viewModel);
        }

        public async Task<ActionResult> VisualizarPorExame(long resultadoExameId)
        {
            var registroArquivo = _registroArquivoService.ObterPorRegistro(resultadoExameId, (long)EnumArquivoTabela.LaboratorioExame);

            try
            {
                Response.Headers.Add("Content-Disposition", "inline; filename=desctino.pdf");
                return File(registroArquivo.Arquivo, "application/pdf");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;

        }

        public async Task<PartialViewResult> ModalVisualizarComparativoResultado(long? id)
        {
            EvolucaoResultadoExameViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new EvolucaoResultadoExameViewModel(new ResultadoLaudoDto());
            }
            else
            {
                //var collection = await _evolucaoResultadosAppService.Obter((long)id);

                viewModel = new EvolucaoResultadoExameViewModel(new ResultadoLaudoDto());
                viewModel.Id = (long)id;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/EvolucaoResultados/ListaExames.cshtml", viewModel);
        }
    }
}