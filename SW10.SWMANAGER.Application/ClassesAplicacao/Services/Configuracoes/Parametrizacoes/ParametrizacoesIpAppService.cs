using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes
{
    public class ParametrizacoesIpAppService : SWMANAGERAppServiceBase, IParametrizacoesIpAppService
    {
        public async Task<ParametrizacaoIpDto> CriarOuEditar(ParametrizacaoIpDto input)
        {
            using (var parametrizacaoIpRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ParametrizacaoIp, long>>())
            {
                var entity = ParametrizacaoIpDto.Mapear(input);
                input.Id = await parametrizacaoIpRepository.Object.InsertOrUpdateAndGetIdAsync(entity).ConfigureAwait(false);

                return input;
            }
        }

        public async Task Excluir(long id)
        {
            using (var parametrizacaoIpRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ParametrizacaoIp, long>>())
            {
                await parametrizacaoIpRepository.Object.DeleteAsync(id).ConfigureAwait(false);
            }
        }

        public async Task<PagedResultDto<ParametrizacaoIpDto>> GetIps()
        {
            using (var parametrizacaoIpRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ParametrizacaoIp, long>>())
            {
                var result =
                    (await parametrizacaoIpRepository.Object.GetAllListAsync().ConfigureAwait(false)).Select(x =>
                        ParametrizacaoIpDto.Mapear(x));
                
                return new PagedResultDto<ParametrizacaoIpDto>(result.Count(), result.ToList()) ;
            }
        }
    }
}
