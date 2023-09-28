using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem
{
    public interface IConfiguracaoPrescricaoItemAppService : IApplicationService
    {
        Task<IList<ConfiguracaoPrescricaoItemDto>> ObterPorDivisao(long id);

        Task<IList<ConfiguracaoPrescricaoItemDto>> ObterPorPrescricaoItem(long id, long? prescricaoItemId = null);
        
        Task<IDictionary<long,IList<ConfiguracaoPrescricaoItemDto>>> ObterPorPrescricaoItemAgrupado(IEnumerable<long> ids);

        Task<IList<ConfiguracaoPrescricaoItemDto>> CriarOuEditar(IList<ConfiguracaoPrescricaoItemDto> input);

        Task Remover(ConfiguracaoPrescricaoItemDto input);


    }
}
