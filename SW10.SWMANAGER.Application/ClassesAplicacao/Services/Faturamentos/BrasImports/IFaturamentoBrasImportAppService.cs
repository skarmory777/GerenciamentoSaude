using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports
{
    public interface IFaturamentoBrasImportAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoBrasImportDto>> Listar(ListarBrasImportsInput input);

        Task CriarOuEditar(FaturamentoBrasImportDto input);

        Task Excluir(FaturamentoBrasImportDto input);

        Task<FaturamentoBrasImportDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarBrasImportsInput input);
    }
}
