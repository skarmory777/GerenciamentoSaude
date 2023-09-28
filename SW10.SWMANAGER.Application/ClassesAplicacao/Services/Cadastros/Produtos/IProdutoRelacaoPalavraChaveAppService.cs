using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoRelacaoPalavraChaveAppService : IApplicationService
    {
        //Task<ProdutoRelacaoPalavraChaveDto> CriarOuEditar(ProdutoRelacaoPalavraChaveDto input, long id);
        Task<ProdutoRelacaoPalavraChaveDto> CriarOuEditar(ProdutoRelacaoPalavraChaveDto input);

        Task Editar(ProdutoRelacaoPalavraChaveDto input);

        Task Excluir(long id);

        Task<ProdutoRelacaoPalavraChaveDto> Obter(long id);

        Task<PagedResultDto<ProdutoRelacaoPalavraChaveDto>> ListarPorProduto(ListarInput Id);
    }
}
