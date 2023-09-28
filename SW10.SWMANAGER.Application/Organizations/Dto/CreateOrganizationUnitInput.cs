using Abp.Organizations;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Organizations.Dto
{
    public class CreateOrganizationUnitInput
    {
        public long? ParentId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}