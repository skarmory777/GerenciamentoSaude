namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class KitExameItem_ajuste_nome : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.KitExameItem", newName: "LabKitExameItem");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.LabKitExameItem", newName: "KitExameItem");
        }
    }
}
