namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Add_FriendUserName_To_Friendship_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppFriendships", "FriendUserName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AppFriendships", "FriendUserName");
        }
    }
}
