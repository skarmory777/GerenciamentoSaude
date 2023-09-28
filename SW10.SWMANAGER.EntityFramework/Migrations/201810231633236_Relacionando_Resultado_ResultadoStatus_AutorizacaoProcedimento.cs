namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Relacionando_Resultado_ResultadoStatus_AutorizacaoProcedimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultado", "LabResultadoStatusId", c => c.Long());
            AddColumn("dbo.LabResultado", "AteAutorizacaoProcedimentoId", c => c.Long());
            CreateIndex("dbo.LabResultado", "LabResultadoStatusId");
            CreateIndex("dbo.LabResultado", "AteAutorizacaoProcedimentoId");
            AddForeignKey("dbo.LabResultado", "AteAutorizacaoProcedimentoId", "dbo.AteAutorizacaoProcedimento", "Id");
            AddForeignKey("dbo.LabResultado", "LabResultadoStatusId", "dbo.LabResultadoStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultado", "LabResultadoStatusId", "dbo.LabResultadoStatus");
            DropForeignKey("dbo.LabResultado", "AteAutorizacaoProcedimentoId", "dbo.AteAutorizacaoProcedimento");
            DropIndex("dbo.LabResultado", new[] { "AteAutorizacaoProcedimentoId" });
            DropIndex("dbo.LabResultado", new[] { "LabResultadoStatusId" });
            DropColumn("dbo.LabResultado", "AteAutorizacaoProcedimentoId");
            DropColumn("dbo.LabResultado", "LabResultadoStatusId");
        }
    }
}
