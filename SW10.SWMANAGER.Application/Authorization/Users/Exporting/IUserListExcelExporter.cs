using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}