using SW10.SWMANAGER.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.Configuration.Host.Dto
{
    public class SendTestEmailInput
    {
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}