using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.InventarioAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public interface IInventarioAppService : IApplicationService
    {
        Task<PagedResultDto<ListagemInventario>> Listar(ListarInventarioInput input);
        Task<PagedResultDto<ListagemInventarioEstoqueContagem>> ListarProdutoEstoque(ListarInventarioEstoqueInput input);
        Task<DefaultReturn<InventarioDto>> GerarInventario(long estoqueId, long? grupoId, long? classeId, long? subClasseId, long? id);
        Task<InventarioDto> Obter(long id);

        Task<PagedResultDto<InventarioEstoque>> ListarItensTodosPorInventario(InventarioEstoqueListarInput input);
        Task<PagedResultDto<InventarioEstoque>> ListarItensContadosPorInventario(InventarioEstoqueListarInput input);
        Task<PagedResultDto<InventarioEstoque>> ListarItensPendentesPorInventario(InventarioEstoqueListarInput input);

        Task<DashboardInventarioEstoque> DashboardInventarioEstoque(long id);
        Task<DefaultReturn<InventarioDto>> Atualizar(long id, List<ListagemInventarioEstoqueContagem> itens);
        Task<DefaultReturn<InventarioEstoque>> AtualizarItem(long id, ListagemInventarioEstoqueContagem item);

        Task ExcluirItem(long id, ListagemInventarioEstoqueContagem item);
        Task<DefaultReturn<InventarioDto>> FecharInventario(long id);
    }
}
