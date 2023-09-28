namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Refazendo_Relacao_FatItem_AutoRelacionamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "ExameIncluiId", c => c.Long());
            CreateIndex("dbo.FatItem", "ExameIncluiId");
            AddForeignKey("dbo.FatItem", "ExameIncluiId", "dbo.FatItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItem", "ExameIncluiId", "dbo.FatItem");
            DropIndex("dbo.FatItem", new[] { "ExameIncluiId" });
            DropColumn("dbo.FatItem", "ExameIncluiId");
        }
    }
}
