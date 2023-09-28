using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using SW10.SWMANAGER.Authorization;

namespace SW10.SWMANAGER.Notifications
{
    public class AppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewUserRegistered,
                    displayName: L("NewUserRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewTenantRegistered,
                    displayName: L("NewTenantRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                );
            
            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.UserNotification,
                    displayName: L("UserNotification")
                )
            );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SWMANAGERConsts.LocalizationSourceName);
        }
    }
}
