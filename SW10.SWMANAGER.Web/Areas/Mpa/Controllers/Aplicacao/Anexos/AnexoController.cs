using SW10.SWMANAGER.ClassesAplicacao.Services.Anexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Anexos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.AWS;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

#pragma warning disable CA3147 // Mark Verb Handlers With Validate Antiforgery Token
namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Anexos
{
    public class AnexoController : SWMANAGERControllerBase
    {
        private readonly IAwsS3AppService _awsS3AppService;
        private readonly IAnexoAppService _anexoAppService;

        public AnexoController(IAwsS3AppService iAwsS3AppService, IAnexoAppService anexoAppService)
        {
            _awsS3AppService = iAwsS3AppService;
            _anexoAppService = anexoAppService;
        }

        [HttpPost]
        public async Task<ActionResult> OpenModal(Guid? anexoListaId, long origemAnexoId, string origemAnexoTabela)
        {
            var anexosDto = new List<AnexoDto>();
            ViewBag.AnexoListaId = anexoListaId;
            ViewBag.OrigemAnexoId = origemAnexoId;
            ViewBag.OrigemAnexoTabela = origemAnexoTabela;

            if (anexoListaId.HasValue)
                anexosDto = await _anexoAppService.ListarAnexos(anexoListaId.Value).ConfigureAwait(false);
            else
                anexosDto = await _anexoAppService.ListarAnexosPelaOrigem(origemAnexoId, origemAnexoTabela).ConfigureAwait(false);

            return PartialView("~/Areas/Mpa/Views/Common/Modals/Anexo/_AnexoModal.cshtml", anexosDto);
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles()
        {
            HttpFileCollectionBase files = Request.Files;
            Guid? anexoListaId = null;
            var origemAnexoTabela = string.Empty;
            var origemAnexoId = string.Empty;

            if (!string.IsNullOrEmpty(Request.Params.Get("anexoListaId")))
                anexoListaId = Guid.Parse(Request.Params.Get("anexoListaId"));

            if (!string.IsNullOrEmpty(Request.Params.Get("origemAnexoTabela")))
                origemAnexoTabela = Request.Params.Get("origemAnexoTabela");

            if (Request.Params.Get("origemAnexoId") != null)
                origemAnexoId = Request.Params.Get("origemAnexoId");

            if (!anexoListaId.HasValue)
                anexoListaId = Guid.NewGuid();

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                byte[] filenameAsBytes = Encoding.ASCII.GetBytes(file.FileName);
                var filenameBase64 = Convert.ToBase64String(filenameAsBytes);
                var key = origemAnexoTabela + "/" + origemAnexoId + "/" + filenameBase64;

                var awsResponse = await _awsS3AppService.SaveObjectAsync(file, key).ConfigureAwait(false);

                if (awsResponse.HttpStatusCode.Equals(HttpStatusCode.OK))
                {
                    var novoAnexoDto = new AnexoDto()
                    {
                        AnexoListaId = anexoListaId.Value,
                        BucketName = awsResponse.BucketName,
                        FileName = file.FileName,
                        Key = key
                    };

                    await _anexoAppService.InserirAnexo(novoAnexoDto).ConfigureAwait(false);
                }
            }

            var anexoListaDto = new AnexoListaDto()
            {
                AnexoListaId = anexoListaId.Value,
                OrigemAnexoId = origemAnexoId,
                OrigemAnexoTabela = origemAnexoTabela
            };

            await _anexoAppService.CriarRelacionamento(anexoListaDto).ConfigureAwait(false);

            return Json("Anexo salvo com sucesso.", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFile(string objectKey)
        {
            var awsResponse = await _awsS3AppService.DeleteObjectAsync(objectKey).ConfigureAwait(false);

            if (awsResponse.HttpStatusCode.Equals(HttpStatusCode.OK) ||
                awsResponse.HttpStatusCode.Equals(HttpStatusCode.NoContent))
            {
                await _anexoAppService.ExcluirAnexo(objectKey).ConfigureAwait(false);
            }

            return Json("Anexo excluído com sucesso.", JsonRequestBehavior.AllowGet);
        }
    }
}
#pragma warning restore CA3147 // Mark Verb Handlers With Validate Antiforgery Token
