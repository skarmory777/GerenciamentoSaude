using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public interface IProdutoRelacaoPortariaAppService : IApplicationService
    {
        Task<ProdutoRelacaoPortariaDto> CriarOuEditar(ProdutoRelacaoPortariaDto input);

        //Task Editar(ProdutoRelacaoPortariaDto input);
        Task Editar(ProdutoRelacaoPortariaDto input);

        Task Excluir(long id);

        Task<PagedResultDto<ProdutoRelacaoPortariaDto>> Listar(ListarInput Id);

        Task<ProdutoRelacaoPortariaDto> Obter(long id);

        Task<PagedResultDto<ProdutoRelacaoPortariaDto>> ListarPorProduto(ListarInput Id);

    }
}
