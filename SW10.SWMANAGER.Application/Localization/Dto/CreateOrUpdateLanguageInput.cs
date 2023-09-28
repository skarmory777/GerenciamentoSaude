using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}