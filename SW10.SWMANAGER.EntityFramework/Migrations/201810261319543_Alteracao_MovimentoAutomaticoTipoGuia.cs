namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_MovimentoAutomaticoTipoGuia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisMovAutomaticoTipoGuia", "FaturamentoGuiaId", c => c.Long(nullable: false));
            CreateIndex("dbo.SisMovAutomaticoTipoGuia", "FaturamentoGuiaId");
            AddForeignKey("dbo.SisMovAutomaticoTipoGuia", "FaturamentoGuiaId", "dbo.FatGuia", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisMovAutomaticoTipoGuia", "FaturamentoGuiaId", "dbo.FatGuia");
            DropIndex("dbo.SisMovAutomaticoTipoGuia", new[] { "FaturamentoGuiaId" });
            DropColumn("dbo.SisMovAutomaticoTipoGuia", "FaturamentoGuiaId");
        }
    }
}
