using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Dashboards
{
    public interface IVWTesteAppService : IApplicationService
    {

        Task<PagedResultDto<VWTesteDto>> Listar(ListarVWTesteInput input);

        Task<VWTesteDto> Obter(long id);
    }
}
