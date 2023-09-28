using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria
{
    public interface IProdutoPortariaAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoPortariaDto>> Listar(ListarProdutosPortariaInput input);

        Task<ListResultDto<ProdutoPortariaDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarProdutoPortaria input);

        Task Excluir(CriarOuEditarProdutoPortaria input);

        Task<CriarOuEditarProdutoPortaria> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosPortariaInput input);
    }
}
