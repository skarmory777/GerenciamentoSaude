using Abp.Localization;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Localization.Dto
{
    public class SetDefaultLanguageInput
    {
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}