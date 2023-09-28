using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoUnidadeTipoAppService : IApplicationService
    {
        Task<ProdutoUnidadeDto> CriarOuEditar(ProdutoUnidadeDto input, long id);

        Task Editar(ProdutoUnidadeDto input);

        Task Excluir(ProdutoUnidadeDto input);

        Task<PagedResultDto<ProdutoUnidadeDto>> Listar(long Id);

        Task<ProdutoUnidadeDto> Obter(long id);

        Task<ProdutoUnidadeDto> ObterPorUnidadeProduto(long unidadeId, long produtoid);
    }
}
