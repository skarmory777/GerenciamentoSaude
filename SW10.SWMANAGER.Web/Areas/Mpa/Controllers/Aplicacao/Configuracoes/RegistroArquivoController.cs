namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Configuracoes
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    using Abp.Auditing;
    using Abp.Extensions;
    using Abp.Web.Mvc.Authorization;

    using Newtonsoft.Json;

    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto;
    using SW10.SWMANAGER.Web.Controllers;

    public class RegistroArquivoController : SWMANAGERControllerBase
    {
        private readonly IRegistroArquivoAppService _registroArquivoAppService;

        public RegistroArquivoController(IRegistroArquivoAppService registroArquivoAppService)
        {
            _registroArquivoAppService = registroArquivoAppService;
        }

        [HttpPost]
        [ValidateInput(false)]
        public void GravarHTMLFormularioDinamico(string registroHTML)
        {
            var _registroHTML = JsonConvert.DeserializeObject<RegistroHTML>(registroHTML);
            _registroArquivoAppService.GravarHTMLFormularioDinamico(_registroHTML);
        }


        [HttpPost]
        [ValidateInput(false)]
        public void GravarImagemFormularioDinamico(string registroHTML)
        {
            var _registroHTML = JsonConvert.DeserializeObject<RegistroHTML>(registroHTML);
            _registroArquivoAppService.GravarImagemFormularioDinamico(_registroHTML);
        }

        [ValidateInput(false)]
        public ActionResult VisualizarPorId(long id)
        {
            var registroArquivo = this._registroArquivoAppService.ObterPorId(id);

            try
            {
                this.Response.Headers.Add("Content-Disposition", "inline; filename=desctino.pdf");

                if (!registroArquivo.ArquivoNome.IsNullOrEmpty())
                {
                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), ConfigurationManager.AppSettings["App.UploadFilesPath"], registroArquivo.ArquivoNome);
                    return this.File(new FileStream(path, FileMode.Open), registroArquivo.ArquivoTipo);
                }
                else
                {
                    return this.File(registroArquivo.Arquivo, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;
        }

        [ValidateInput(false)]
        public ActionResult VisualizarPorRegistroIdEOperacao(long registroId, long operacaoId)
        {
            var registroArquivo = this._registroArquivoAppService.ObterPorRegistro(registroId, RegistroArquivoDto.ObterTabelaRegistroFormularioDinamico(operacaoId));

            try
            {
                this.Response.Headers.Add("Content-Disposition", "inline; filename=desctino.pdf");

                if (!registroArquivo.ArquivoNome.IsNullOrEmpty())
                {
                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), ConfigurationManager.AppSettings["App.UploadFilesPath"], registroArquivo.ArquivoNome);
                    return this.File(new FileStream(path, FileMode.Open), registroArquivo.ArquivoTipo);
                }
                else
                {
                    return this.File(registroArquivo.Arquivo, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;
        }

        [ValidateInput(false)]
        public string ObterArquivoNomePorIdEOperacao(long registroId, long operacaoId)
        {
            var registroArquivo = this._registroArquivoAppService.ObterPorRegistro(registroId, RegistroArquivoDto.ObterTabelaRegistroFormularioDinamico(operacaoId));
            if (registroArquivo == null || registroArquivo.ArquivoNome.IsNullOrEmpty())
            {
                return null;
            }


            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "") + Path.Combine(@"/",ConfigurationManager.AppSettings["App.UploadFilesPath"], registroArquivo.ArquivoNome).Replace(@"\",@"/");
        }

        [AbpMvcAuthorize]
        [DisableAuditing]
        public ActionResult DownloadArvivoPorRegistro(long registroId, long registroTabelaId, string nomeArquivo, string tipoArquivo)
        {
            var registroArquivo = _registroArquivoAppService.ObterPorRegistro(registroId, registroTabelaId);

            if (registroArquivo != null)
            {
                string _nomeArquivo = !string.IsNullOrEmpty(nomeArquivo) ? nomeArquivo : registroArquivo.Descricao;

                return File(registroArquivo.Arquivo, tipoArquivo, _nomeArquivo);
            }
            byte[] bytes = new byte[0];
            return File(bytes, tipoArquivo, nomeArquivo);
        }

    }
}