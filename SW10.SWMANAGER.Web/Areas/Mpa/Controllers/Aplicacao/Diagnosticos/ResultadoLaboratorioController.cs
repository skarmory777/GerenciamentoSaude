using Abp.Dependency;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Input;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Laboratorio;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Diagnosticos
{
    public class ResultadoLaboratorioController : SWMANAGERControllerBase
    {
        private readonly IResultadoAppService _resultadoAppService;
        private readonly IFormataItemAppService _formataItemAppService;
        private readonly IRegistroArquivoAppService _registroArquivoAppService;
        private readonly IUserAppService _userAppService;

        public ResultadoLaboratorioController(
            IResultadoAppService resultadoAppService,
            IFormataItemAppService formataItemAppService,
            IRegistroArquivoAppService registroArquivoAppService,
            IUserAppService userAppService)
        {
            _resultadoAppService = resultadoAppService;
            _formataItemAppService = formataItemAppService;
            _registroArquivoAppService = registroArquivoAppService;
            _userAppService = userAppService;
        }



        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new LaudoMovimentoViewModel(new LaudoMovimentoDto());
            //Obter empresas habilitadas para o usuário
            var empresas = Task.Run(() => _userAppService.GetUserEmpresas(AbpSession.UserId.Value)).Result;
            if (empresas.Items.Count == 1)
            {
                viewModel.EmpresaId = empresas.Items[0].Id;
                viewModel.NomeEmpresa = empresas.Items[0].NomeFantasia;
            }
            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarResultadoExame(long? id)
        {
            var viewModel = new ResultadoExameViewModel();
            if (id != null)
            {
                var resultado = await _resultadoAppService.Obter((long)id);

                viewModel.ColetaId = resultado.Id;
                viewModel.Paciente = resultado.Atendimento.Paciente.NomeCompleto;
                viewModel.Codigo = resultado.Nic.ToString();
                viewModel.DataColeta = string.Format("{0:dd/MM/yyyy hh:mm}", resultado.DataColeta);
                viewModel.IsRN = resultado.IsRn;
                viewModel.IsUrgente = resultado.IsUrgente;
                viewModel.Leito = resultado.Atendimento?.Leito?.Descricao;
                viewModel.Medico = resultado.Atendimento?.Medico?.NomeCompleto;
                viewModel.Tecnico = resultado.Tecnico?.Descricao;

                var itensExame = await this._resultadoAppService.ListarItensFormatacaoPorColeta(new LaudoResultadoInput { ColetaId = id }).ConfigureAwait(false);

                viewModel.ItensJson = JsonConvert.SerializeObject(itensExame);
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/ResultadoExames.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarLaudoResultadoExame(long? id)
        {
            var viewModel = new ResultadoExameViewModel();
            if (id != null)
            {
                var resultadoExame = await _resultadoAppService.ObterResultadoExame((long)id);

                viewModel.ColetaId = resultadoExame.Resultado.Id;
                viewModel.Paciente = resultadoExame.Resultado.Atendimento.Paciente.NomeCompleto;
                viewModel.Codigo = resultadoExame.Resultado.Nic.ToString();
                viewModel.DataColeta = String.Format("{0:dd/MM/yyyy hh:mm}", resultadoExame.Resultado.DataColeta);
                viewModel.ExameId = resultadoExame.FaturamentoItemId;
                viewModel.Exame = resultadoExame.FaturamentoItem.Descricao;
                viewModel.ResultadoExameId = resultadoExame.Id;

                var itensExame = await _resultadoAppService.ListarItensFormatacaoPorExame(new LaudoResultadoInput { ExameId = (long)viewModel.ExameId, ResultadoExameId = resultadoExame.Id });

                viewModel.ItensJson = JsonConvert.SerializeObject(itensExame);

            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/LaudoResultadoExames.cshtml", viewModel);
        }

        public async Task<string> CalcularFormula(string input, long itemResultadoId)
        {
            await _formataItemAppService.CalcularFormula(input, itemResultadoId);

            return "";
        }


        public async Task<ActionResult> ModalVisualizarExame(long registroArquivoId)
        {
            var viewModel = new ResultadoExameViewModel
            {
                RegistroArquivoId = registroArquivoId
                // ,
                // FichaPdf = await GerarFichaInternacao(atendimentoId) as FileContentResult
            };

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.cshtml", viewModel);
        }

        public async Task<ActionResult> VisualizarExame(long registroArquivoId)
        {
            var registroArquivo = _registroArquivoAppService.ObterPorId(registroArquivoId);

            try
            {
                Response.Headers.Add("Content-Disposition", "inline; filename=ficha_amb.pdf");
                return File(registroArquivo.Arquivo, "application/pdf");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;
        }

        [AllowAnonymous]
        public async Task<string> RetornaFormatadoColeta(long coletaId, long exameId, int tenantId)
        {
            this.CurrentUnitOfWork.SetTenantId(tenantId);

            using (var resultadoLaudoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoLaudoAppService>())
            {
                var result = await resultadoLaudoAppService.Object.FormatacaoColetaExame(coletaId, exameId).ConfigureAwait(false);

                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(result);
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                this.Response.ContentType = "text/html; charset=ISO-8859-1";
                this.Response.Charset = "ISO-8859-1";
                this.Response.ContentEncoding = iso;
                this.Response.BinaryWrite(iso.GetPreamble());
                this.Response.Write(result);
                return null;
            }
        }

        public async Task<ActionResult> ModalVisualizarExamePorRegistroColeta(long coletaId)
        {
            var viewModel = new ResultadoExameViewModel
            {
                ColetaId = coletaId
            };

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.cshtml", viewModel);
        }



        public async Task<ActionResult> VisualizarExamePorColeta(long coletaId)
        {
            try
            {
                var registroArquivo = _resultadoAppService.ObterArquivoExameColeta(coletaId);
                if (registroArquivo == null)
                {
                    return null;
                }
                Response.Headers.Add("Content-Disposition", "inline; filename=desctino.pdf");
                return File(registroArquivo.Arquivo, "application/pdf");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;

        }


        public async Task<ActionResult> VisualizarPorId(long id)
        {
            var registroArquivo = _registroArquivoAppService.ObterPorId(id);

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