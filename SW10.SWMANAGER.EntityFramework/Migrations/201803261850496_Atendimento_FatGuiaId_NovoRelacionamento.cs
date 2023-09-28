namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Atendimento_FatGuiaId_NovoRelacionamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "FatGuiaId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "FatGuiaId");
            AddForeignKey("dbo.AteAtendimento", "FatGuiaId", "dbo.FatGuia", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "FatGuiaId", "dbo.FatGuia");
            DropIndex("dbo.AteAtendimento", new[] { "FatGuiaId" });
            DropColumn("dbo.AteAtendimento", "FatGuiaId");
        }
    }
}
