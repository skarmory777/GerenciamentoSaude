// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImpressoraHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The impressora helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Microsoft.AspNet.SignalR;

    using PdfSharp;
    using PdfSharp.Drawing;

    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using SW10.SWMANAGER.PrinterHub;

    using TheArtOfDev.HtmlRenderer.Core;
    using TheArtOfDev.HtmlRenderer.PdfSharp;

    /// <summary>
    /// The impressora helper.
    /// </summary>
    public static class ImpressoraHelper
    {
        /// <summary>
        /// The merge texto.
        /// </summary>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="propriedade">
        /// The propriedade.
        /// </param>
        /// <param name="valor">
        /// The valor.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string MergeTexto(this string texto, string propriedade, string valor)
        {
            var stringBuilder = new StringBuilder(texto);
            return stringBuilder.Replace($"@{propriedade}", !string.IsNullOrEmpty(valor) ? valor : "&nbsp;").ToString();
        }

        /// <summary>
        /// The merge texto.
        /// </summary>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="propriedade">
        /// The propriedade.
        /// </param>
        /// <param name="valor">
        /// The valor.
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/>.
        /// </returns>
        public static StringBuilder MergeTexto(this StringBuilder texto, string propriedade, string valor)
        {
            return texto?.Replace($"@{propriedade}", !string.IsNullOrEmpty(valor) ? valor : "&nbsp;");
        }

        /// <summary>
        /// The gerar pdf.
        /// </summary>
        /// <param name="modelo">
        /// The modelo.
        /// </param>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] GerarPdf(this TextoModelo modelo, string texto)
        {
            using (var ms = new MemoryStream())
            {
                using (var pdf = PdfGenerator.GeneratePdf(texto, new PdfGenerateConfig { ManualPageSize = new XSize(modelo.TamanhoModelo.AlturaCm * 72, modelo.TamanhoModelo.LarguraCm * 72) }))
                {
                    pdf.Save(ms);
                    pdf.Dispose();
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// The gerar pdf.
        /// </summary>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="alturaCm">
        /// The altura cm.
        /// </param>
        /// <param name="larguraCm">
        /// The largura cm.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] GerarPdf(string texto, double alturaCm, double larguraCm)
        {
            using (var ms = new MemoryStream())
            {
                using (var pdf = PdfGenerator.GeneratePdf(texto, new PdfGenerateConfig { ManualPageSize = new XSize(alturaCm * 72, larguraCm * 72) }))
                {
                    pdf.Save(ms);
                    pdf.Dispose();
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// The gerar pdf.
        /// </summary>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] GerarPdf(string texto, CssData css = null, PdfGenerateConfig config = null)
        {
            using (var ms = new MemoryStream())
            {
                using (var pdf = PdfGenerator.GeneratePdf(texto, PageSize.A4))
                {
                    pdf.Save(ms);
                    pdf.Dispose();
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// The gerar pdf.
        /// </summary>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] GerarPdf(string texto)
        {
            using (var ms = new MemoryStream())
            {
                using (var pdf = PdfGenerator.GeneratePdf(texto, new PdfGenerateConfig()))
                {
                    pdf.AddPage();
                    pdf.Save(ms);
                    pdf.Dispose();
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// The enviar para imprimir.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="modelo">
        /// The modelo.
        /// </param>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="printerCookie">
        /// The printer name.
        /// </param>
        /// <param name="numberOfCopies">
        /// The number Of Copies.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        public static void EnviarParaImprimir(this HttpContextBase context, TextoModelo modelo, string texto, string fileName, string printerCookie, long numberOfCopies = 1)
        {
            var existPrinter = context.Request.Cookies.AllKeys.Any(x => x == printerCookie);


            if (existPrinter)
            {
                var printerHubContext = GlobalHost.ConnectionManager.GetHubContext<PrinterHub.PrinterHub>();

                var printerName = context.Server.UrlDecode(
                    context.Request.Cookies.Get(printerCookie)?.Value ?? null);
                PrinterHub.PrinterHub.DocumentToPrinter(printerHubContext, new PrintDocumentDto(fileName, modelo.GerarPdf(texto), printerName, numberOfCopies));
            }
            else
            {
                throw new Exception($"É preciso definir a impressora padrão ({printerCookie})");
            }
        }

        /// <summary>
        /// The enviar para imprimir.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="modelo">
        /// The modelo.
        /// </param>
        /// <param name="texto">
        /// The texto.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="tipoModelo">
        /// The tipo modelo.
        /// </param>
        /// <param name="numberOfCopies">
        /// The number Of Copies.
        /// </param>
        public static void EnviarParaImprimir(
            this HttpContextBase context,
            TextoModelo modelo,
            string texto,
            string fileName,
            EnumTipoModelo tipoModelo,
            long numberOfCopies = 1)
        {
            EnviarParaImprimir(context, modelo, texto, fileName, CookiePorModelo(tipoModelo), numberOfCopies);
        }

        /// <summary>
        /// The cookie por modelo.
        /// </summary>
        /// <param name="tipoModelo">
        /// The tipo modelo.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public static string CookiePorModelo(EnumTipoModelo tipoModelo)
        {
            switch (tipoModelo)
            {
                case EnumTipoModelo.EtiquetaPaciente:
                    return "impressora_etiqueta_paciente";
                case EnumTipoModelo.EtiquetaVisitante:
                    return "impressora_etiqueta_visitante";
                case EnumTipoModelo.Pulseira:
                    return "impressora_pulseira";
                case EnumTipoModelo.TerminalSenha:
                    return "impressora_terminal_de_senha";
                default:
                    return string.Empty;
            }
        }
    }
}