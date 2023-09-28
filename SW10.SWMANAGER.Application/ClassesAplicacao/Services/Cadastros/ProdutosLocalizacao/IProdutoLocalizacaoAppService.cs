using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao
{
    public interface IProdutoLocalizacaoAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoLocalizacaoDto>> Listar(ListarProdutosLocalizacaoInput input);

        Task CriarOuEditar(ProdutoLocalizacaoDto input);

        Task Excluir(ProdutoLocalizacaoDto input);

        Task<ProdutoLocalizacaoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosLocalizacaoInput input);

        Task<ListResultDto<ProdutoLocalizacaoDto>> ListarTodos();

    }
}
