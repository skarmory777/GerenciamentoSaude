using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SW10.SWMANAGER.Authorization.Users;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes
{
    public class ParametrizacoesAppService : SWMANAGERAppServiceBase, IParametrizacoesAppService
    {
        public async Task<ParametrizacoesDto> GetParametrizacoes()
        {
            using (var parametrizacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Parametrizacao, long>>())
            {
                var entity = await parametrizacaoRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false) ?? new Parametrizacao();
                return ParametrizacoesDto.Mapear(entity);
            }
        }

        public ParametrizacoesDto GetParametrizacoesSync()
        {
            using (var parametrizacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Parametrizacao, long>>())
            {
                var entity =  parametrizacaoRepository.Object.GetAll().AsNoTracking().FirstOrDefault() ?? new Parametrizacao();
                return ParametrizacoesDto.Mapear(entity);
            }
        }

        public async Task<ParametrizacoesDto> CriarOuEditar(ParametrizacoesDto input)
        {
            using (var parametrizacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Parametrizacao, long>>())
            {
                var entity = ParametrizacoesDto.Mapear(input);
                input.Id = await parametrizacaoRepository.Object.InsertOrUpdateAndGetIdAsync(entity).ConfigureAwait(false);

                return input;
            }
        }

        

        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                // get host IP addresses
                var hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
