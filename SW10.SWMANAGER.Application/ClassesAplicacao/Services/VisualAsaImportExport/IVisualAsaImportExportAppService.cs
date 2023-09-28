using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs
{
    public interface IVisualAsaImportExportLogAppService : IApplicationService
    {
        Task CriarOuEditar(VisualAsaImportExportLogDto input);

        Task<PagedResultDto<VisualAsaImportExportLogDto>> Listar(ListarInput input);

        Task<VisualAsaImportExportLogDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}