namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_Relacionamento_AutorizacaoProcedimentoItem_ResultadoExame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoExame", "AteAutorizacaoProcedimentoitemId", c => c.Long());
            CreateIndex("dbo.LabResultadoExame", "AteAutorizacaoProcedimentoitemId");
            AddForeignKey("dbo.LabResultadoExame", "AteAutorizacaoProcedimentoitemId", "dbo.AteAutorizacaoProcedimentoItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "AteAutorizacaoProcedimentoitemId", "dbo.AteAutorizacaoProcedimentoItem");
            DropIndex("dbo.LabResultadoExame", new[] { "AteAutorizacaoProcedimentoitemId" });
            DropColumn("dbo.LabResultadoExame", "AteAutorizacaoProcedimentoitemId");
        }
    }
}
