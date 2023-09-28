using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ExamesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Laboratorio;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Diagnosticos
{
    public class ConferenciaResultadoExamesController : SWMANAGERControllerBase
    {
        private readonly IResultadoAppService _resultadoAppService;
        private readonly IRegistroArquivoAppService _registroArquivoService;
        private readonly IExameStatusAppService _exameStatusAppService;

        public ConferenciaResultadoExamesController(IResultadoAppService resultadoAppService
            , IRegistroArquivoAppService registroArquivoService
            , IExameStatusAppService exameStatusAppService)
        {
            _resultadoAppService = resultadoAppService;
            _registroArquivoService = registroArquivoService;
            _exameStatusAppService = exameStatusAppService;
        }

        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ResultadoExameViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/ConfirmacaoResultadosExames/Index.cshtml", viewModel);
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

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/ConfirmacaoResultadosExames/ListaExames.cshtml", viewModel);
        }

        public async Task<ActionResult> ModalVisualizarExame(long resultadoExameId)
        {

            // var resitroArquivo =_registroArquivoAppService.ObterPorRegistro(coletaId, (long)EnumArquivoTabela.Laboratorio);

            // var resitroArquivo = _resultadoAppService.ObterArquivoExameColeta(coletaId);

            //long registroArquivoId=0;

            //if (resitroArquivo != null)
            //{
            //    registroArquivoId = resitroArquivo.Id;
            //    coletaId = coletaId;
            //}
            ResultadoExameViewModel viewModel = new ResultadoExameViewModel
            {
                ResultadoExameId = resultadoExameId
            };

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.cshtml", viewModel);
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


    }
}