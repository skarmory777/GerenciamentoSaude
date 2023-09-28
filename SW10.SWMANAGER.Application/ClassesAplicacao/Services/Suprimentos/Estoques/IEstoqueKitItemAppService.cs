using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public interface IEstoqueKitItemAppService : IApplicationService
    {
        Task<PagedResultDto<EstoqueKitItemDto>> ListarItensKit(ListarEstoqueKitItemInput input);
        Task<List<EstoqueKitItemDto>> ListarPeloKitEstoqueIdEEstoqueId(long kitEstoqueId, long estoqueId);
        Task<long?> CriarOuEditar(EstoqueKitItemDto input);
        Task Excluir(EstoqueKitItemDto input);
        Task<List<EstoqueKitItemDto>> ObterItensKit(long estoqueKitItemId);
    }
}
