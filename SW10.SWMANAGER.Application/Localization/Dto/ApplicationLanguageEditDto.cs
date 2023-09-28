using Abp.AutoMapper;
using Abp.Localization;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Localization.Dto
{
    [AutoMapFrom(typeof(ApplicationLanguage))]
    public class ApplicationLanguageEditDto
    {
        public virtual int? Id { get; set; }

        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ApplicationLanguage.MaxIconLength)]
        public virtual string Icon { get; set; }
    }
}