namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracoesRegistroExames : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LauMovimentoItem", "LauMovimentoId", "dbo.LauMovimento");
            DropIndex("dbo.LauMovimentoItem", new[] { "LauMovimentoId" });
            AddColumn("dbo.LauMovimento", "MedicoSolicitante", c => c.String());
            AddColumn("dbo.LauMovimento", "Tecnico", c => c.String());
            AlterColumn("dbo.LauMovimentoItem", "LauMovimentoId", c => c.Long());
            CreateIndex("dbo.LauMovimentoItem", "LauMovimentoId");
            AddForeignKey("dbo.LauMovimentoItem", "LauMovimentoId", "dbo.LauMovimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LauMovimentoItem", "LauMovimentoId", "dbo.LauMovimento");
            DropIndex("dbo.LauMovimentoItem", new[] { "LauMovimentoId" });
            AlterColumn("dbo.LauMovimentoItem", "LauMovimentoId", c => c.Long(nullable: false));
            DropColumn("dbo.LauMovimento", "Tecnico");
            DropColumn("dbo.LauMovimento", "MedicoSolicitante");
            CreateIndex("dbo.LauMovimentoItem", "LauMovimentoId");
            AddForeignKey("dbo.LauMovimentoItem", "LauMovimentoId", "dbo.LauMovimento", "Id", cascadeDelete: true);
        }
    }
}
