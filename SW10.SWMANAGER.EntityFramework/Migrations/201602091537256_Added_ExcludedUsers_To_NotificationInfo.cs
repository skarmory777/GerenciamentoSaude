namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Added_ExcludedUsers_To_NotificationInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpNotifications", "ExcludedUserIds", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AbpNotifications", "ExcludedUserIds");
        }
    }
}
