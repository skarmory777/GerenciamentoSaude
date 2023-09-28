using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia
{
    public interface IProdutoSubstanciaAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<ProdutoSubstanciaDto>> Listar(ListarProdutosSubstanciaInput input);

        Task CriarOuEditar(CriarOuEditarProdutoSubstancia input);

        Task Excluir(CriarOuEditarProdutoSubstancia input);

        Task<CriarOuEditarProdutoSubstancia> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosSubstanciaInput input);
    }
}
