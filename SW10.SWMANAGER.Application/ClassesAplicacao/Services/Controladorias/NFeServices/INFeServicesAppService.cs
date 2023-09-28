using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices
{
    public interface INFeServicesAppService : IApplicationService
    {
        Task<string> DescOperacao(string tpEventoCodigo);
        Task<bool> isOnline(string CNPJ);
        Task<RetDistDFeIntOutput> NfeDistDFeInteresse(string CNPJ, string ufAutor, string ultNSU, string nSU = "0");
        Task<RetEnvEventoOutput> RecepcaoEventoManifestacaoDestinatario(int idlote, int sequenciaEvento,
            string chaveNFe, string tipoEventoManifestacaoDestinatario, string CNPJ,
            string justificativa = null);
        Task<RetDistDFeIntOutput> BuscaPorChave(string CNPJ, string ufAutor, string chNFe);
    }
}
