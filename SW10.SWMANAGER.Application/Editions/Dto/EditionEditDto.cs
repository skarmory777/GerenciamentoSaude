using Abp.Application.Editions;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Editions.Dto
{
    [AutoMap(typeof(Edition))]
    public class EditionEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}