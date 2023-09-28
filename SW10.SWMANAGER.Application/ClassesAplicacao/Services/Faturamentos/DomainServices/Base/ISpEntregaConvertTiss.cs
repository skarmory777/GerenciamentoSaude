using System.Globalization;
using System.Text;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices
{
    public interface ISpEntregaConvertTiss
    {
        CultureInfo Culture { get; set; }
        StringBuilder SbHash {  get;  set; }
        StringBuilder SbContent { get;  set; }
        EntregaTissLoteGerado ConvertMensagemTISS(SpEntrega entrega);
    }
}