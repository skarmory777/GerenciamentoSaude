namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Removendo_Relacao_FatItem_LabExame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatItem", "ExameIncluiId", "dbo.LabExame");
            DropIndex("dbo.FatItem", new[] { "ExameIncluiId" });
            DropColumn("dbo.FatItem", "ExameIncluiId");
        }

        public override void Down()
        {
            AddColumn("dbo.FatItem", "ExameIncluiId", c => c.Long());
            CreateIndex("dbo.FatItem", "ExameIncluiId");
            AddForeignKey("dbo.FatItem", "ExameIncluiId", "dbo.LabExame", "Id");
        }
    }
}
