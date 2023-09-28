namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatConta_FatGuia_NovoModelo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatConta", "Fat_Guia_Id", c => c.Long());
            CreateIndex("dbo.FatConta", "Fat_Guia_Id");
            AddForeignKey("dbo.FatConta", "Fat_Guia_Id", "dbo.FatGuia", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatConta", "Fat_Guia_Id", "dbo.FatGuia");
            DropIndex("dbo.FatConta", new[] { "Fat_Guia_Id" });
            DropColumn("dbo.FatConta", "Fat_Guia_Id");
        }
    }
}
