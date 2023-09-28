using System.Globalization;
using System.Text;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices
{
    public abstract class BaseSpEntregaConvertTiss : ISpEntregaConvertTiss
    {
        public CultureInfo Culture { get; set; }
        public StringBuilder SbHash { get; set; }
        public StringBuilder SbContent { get; set; }
        public BaseSpEntregaConvertTiss()
        {
            Culture = CultureInfo.GetCultureInfo("en-US");
            SbHash = new StringBuilder();
            SbContent = new StringBuilder();
        }

        public abstract EntregaTissLoteGerado ConvertMensagemTISS(SpEntrega entrega);
    }
}