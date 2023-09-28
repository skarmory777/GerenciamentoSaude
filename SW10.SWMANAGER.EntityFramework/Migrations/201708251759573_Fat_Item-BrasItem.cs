namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_ItemBrasItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "BrasItemId", c => c.Long());
            CreateIndex("dbo.FatItem", "BrasItemId");
            AddForeignKey("dbo.FatItem", "BrasItemId", "dbo.FatBrasItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItem", "BrasItemId", "dbo.FatBrasItem");
            DropIndex("dbo.FatItem", new[] { "BrasItemId" });
            DropColumn("dbo.FatItem", "BrasItemId");
        }
    }
}
