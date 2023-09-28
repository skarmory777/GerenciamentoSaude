using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Common.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<ComboboxItemDto>> GetEditionsForCombobox();

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        string GetDefaultEditionName();
    }
}