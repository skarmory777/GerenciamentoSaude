using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}