using SW10.SWMANAGER.Helper;
using System.Text;
using System.Web.Mvc;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices
{
    public class EntregaTissLoteGerado
    {

        public EntregaTissLoteGerado()
        {

        }

        public EntregaTissLoteGerado(SpEntrega entrega, StringBuilder sbContent)
        {
            this.NomeLote = FormataNome(entrega);
            this.Conteudo = sbContent;
        }

        private string FormataNome(SpEntrega entrega) => @$"{entrega.Lote.SequencialTransacao.PadLeft(20, '0')}_{entrega.EpilogoHash}.xml";

        public string NomeLote { get; set; }

        public StringBuilder Conteudo { get; set; }

        public bool Gerado { get { return Conteudo.Length != 0; } }

        public FileContentResult GerarArquivo()
        {
            return new FileContentResult(Conteudo.GenerateByteFromString(), "application/octet-stream")
            {
                FileDownloadName = NomeLote
            };
        }
    }
}