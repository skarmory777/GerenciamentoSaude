using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;
using nfeProc = NFe.Classes.nfeProc;

namespace Sefaz
{
    public class RetornoNFeDto
    {
        public nfeProc NfeProc { get; set; }

        public resNFe ResNFe { get; set; }
    }

}
