using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public interface IConsultorTipoDadoNFAppService : IApplicationService
    {
        Task<PagedResultDto<ConsultorTipoDadoNFDto>> ListarTodos();
    }
}
