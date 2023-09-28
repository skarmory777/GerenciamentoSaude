using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica
{
    public interface IProdutoAcaoTerapeuticaAppService : IApplicationService
    {
        Task<PagedResultDto<ProdutoAcaoTerapeuticaDto>> Listar(ListarProdutosAcoesTerapeuticaInput input);

        Task<ListResultDto<ProdutoAcaoTerapeuticaDto>> ListarTodos();

        Task CriarOuEditar(ProdutoAcaoTerapeuticaDto input);

        Task Excluir(ProdutoAcaoTerapeuticaDto input);

        Task<ProdutoAcaoTerapeuticaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosAcoesTerapeuticaInput input);
    }
}
