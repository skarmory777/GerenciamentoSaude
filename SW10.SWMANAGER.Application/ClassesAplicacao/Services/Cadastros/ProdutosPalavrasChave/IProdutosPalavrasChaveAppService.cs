using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave
{
    public interface IProdutoPalavraChaveAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoPalavraChaveDto>> Listar(ListarProdutosPalavrasChaveInput input);

        Task<ListResultDto<ProdutoPalavraChaveDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarProdutoPalavraChave input);

        Task Excluir(CriarOuEditarProdutoPalavraChave input);

        Task<ProdutoPalavraChaveDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosPalavrasChaveInput input);
    }
}
