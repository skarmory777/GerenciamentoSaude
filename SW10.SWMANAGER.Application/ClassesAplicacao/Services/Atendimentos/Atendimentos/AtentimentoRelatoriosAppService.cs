using RestSharp;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos
{
    public class AtentimentoRelatoriosAppService : SWMANAGERAppServiceBase, IAtentimentoRelatoriosAppService
    {
        public byte[] RetornaArquivoMapaDiaSintatico(long? UnidadeOrganizacionalId, long? StatusId)
        {
            return this.CreateJasperReport("/assistencial/MapaDiaSintatico")
                    .SetMethod(Method.POST)
                    .AddParameter("UnidadeId", UnidadeOrganizacionalId.HasValue ? UnidadeOrganizacionalId.ToString() : "0")
                    .AddParameter("StatusId", StatusId.HasValue ? StatusId.ToString() : "0")
                    .AddParameter("UsuarioImpressao", this.GetCurrentUser().FullName)
                    .AddParameter("Dominio", this.GetConnectionStringName())
                    .GenerateReport();
        }
    }
}
