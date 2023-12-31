namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Revised_Notification_Entities : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.AbpUserNotifications", "NotificationId", "TenantNotificationId");
        }

        public override void Down()
        {
            RenameColumn("dbo.AbpUserNotifications", "TenantNotificationId", "NotificationId");
        }
    }
}
