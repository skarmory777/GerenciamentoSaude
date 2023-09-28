using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoListaSubstituicaoAppService : IApplicationService
    {
        Task<ProdutoListaSubstituicaoDto> CriarOuEditar(ProdutoListaSubstituicaoDto input);

        Task Editar(ProdutoListaSubstituicaoDto input);

        Task Excluir(long id);

        Task<PagedResultDto<ProdutoListaSubstituicaoDto>> Listar(long Id);

        Task<ProdutoListaSubstituicaoDto> Obter(long id);

        Task<PagedResultDto<ProdutoListaSubstituicaoDto>> ListarPorProduto(ListarInput input);

    }
}
