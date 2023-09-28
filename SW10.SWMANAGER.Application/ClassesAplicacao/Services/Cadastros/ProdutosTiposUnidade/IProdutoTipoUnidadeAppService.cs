using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade
{
    public interface IProdutoTipoUnidadeAppService : IApplicationService
    {
        Task<PagedResultDto<ProdutoTipoUnidadeDto>> Listar(ListarProdutosTiposUnidadeInput input);

        Task CriarOuEditar(ProdutoTipoUnidadeDto input);

        Task Excluir(ProdutoTipoUnidadeDto input);

        Task<ProdutoTipoUnidadeDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProdutosTiposUnidadeInput input);
    }
}
