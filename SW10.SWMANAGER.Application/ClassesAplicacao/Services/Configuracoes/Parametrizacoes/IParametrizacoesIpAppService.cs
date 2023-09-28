using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes
{
    public interface IParametrizacoesIpAppService : IApplicationService
    {
        Task<PagedResultDto<ParametrizacaoIpDto>> GetIps();

        Task<ParametrizacaoIpDto> CriarOuEditar(ParametrizacaoIpDto input);

        Task Excluir(long id);

    }
}
