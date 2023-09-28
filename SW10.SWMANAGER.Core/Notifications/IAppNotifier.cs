using Abp;
using Abp.Notifications;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.MultiTenancy;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
