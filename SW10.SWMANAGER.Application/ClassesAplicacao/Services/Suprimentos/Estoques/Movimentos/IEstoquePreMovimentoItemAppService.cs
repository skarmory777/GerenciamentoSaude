using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.EstoquePreMovimentoAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoquePreMovimentoItemAppService : IApplicationService
    {
        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditar(EstoquePreMovimentoItemDto input);

        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditarSaida(EstoquePreMovimentoItemDto input);

        Task Editar(EstoquePreMovimentoItemDto input);

        Task ExcluirTodosItensKitEstoque(long id, long estoqueKitId);

        Task Excluir(long id);

        Task<PagedResultDto<EstoquePreMovimentoItemDto>> Listar(long Id);

        Task<EstoquePreMovimentoItemDto> Obter(long id);

        Task<PagedResultDto<EstoquePreMovimentoItemDto>> ListarPorMovimentacaoLoteValidade(ListarEstoquePreMovimentoInput input);

        Task<PagedResultDto<EstoquePreMovimentoItemDto>> ListarPorMovimentacaoLoteValidadeProduto(ListarEstoquePreMovimentoInput input);

        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditarTransferencia(EstoquePreMovimentoItemDto input, long transferenciaProdutoId, long transferenciaProdutoItemId);

        Task<EstoqueTransferenciaProdutoItemDto> ObterTransferenciaItem(long id);

        Task ExcluirItemTransferencia(long id);

        Task<List<string>> ObterNumerosSerieProduto(long estoqueId, long produtoId);

        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditarDevolucao(EstoquePreMovimentoItemDto input);

        Task<List<EstoquePreMovimentoItemDto>> ObterItensPorPreMovimento(long preMovimentoId);

        List<EstoquePreMovimentoItemSolicitacaoDto> ObterItensSolicitacaoPorPreMovimento(long preMovimentoId);

        PagedResultDto<LoteValidadeGridDto> ListarLoteValidadeJson(ListarItensJsonInput input);

        PagedResultDto<NumeroSerieGridDto> ListarNumeroSerieJson(ListarItensJsonInput input);

        PagedResultDto<LoteValidadeGridDto> ListarLoteValidadeJsonLista(ListarItensJsonInput input); //List<LoteValidadeGridDto> lista);
        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarSaidaPorCodigoBarra(string codigoBarra, long? estoqueId, long? preMovimentoId, decimal? quantidade);
        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarDevolucaoPorCodigoBarra(string codigoBarra, long? estoqueId, long? preMovimentoId, decimal? quantidade);
        Task ExcluirPorPreMovimento(long id);

        Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarKitEstoqueItem(EstoquePreMovimentoKitEstoqueItemDto input);
    }
}
