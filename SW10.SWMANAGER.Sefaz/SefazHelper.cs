using Sefaz.Dto;

namespace Sefaz
{
    public static partial class SefazHelper
    {
        public static SefazConnection ConexaoSefaz( string cnpj)
        {
            return new SefazConnection(cnpj);
        }

        public static SefazConnection ConexaoSefaz(SefazConfig config)
        {
            return new SefazConnection(config);
        }
    }
}
