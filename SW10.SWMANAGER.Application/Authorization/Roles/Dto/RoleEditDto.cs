using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Authorization.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }

        public bool? IsHabilitaControleDeIp { get; set; }
    }
}