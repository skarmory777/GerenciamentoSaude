using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoEstoqueAppService : IApplicationService
    {
        Task<ProdutoEstoqueDto> CriarOuEditar(ProdutoEstoqueDto input);

        Task<ProdutoEstoqueDto> Obter(long id);

        Task<PagedResultDto<ProdutoEstoqueDto>> ListarPorProduto(ListarInput input);

        Task Excluir(ProdutoEstoqueDto input);

        ListResultDto<EstoqueDto> ListarTodosNaoRelacionadosProdutos(long produtoId, long id);

    }
}
