using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Web.Models.Account
{
    public class ImpersonateModel
    {
        public int? TenantId { get; set; }

        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}