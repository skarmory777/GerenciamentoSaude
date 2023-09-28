using Abp.Application.Services;
using SW10.SWMANAGER.Sessions.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
