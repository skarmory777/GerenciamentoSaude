namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoResultadoLaudo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LabResultadoLaudo", "ResultadoExameId");
            CreateIndex("dbo.LabResultadoLaudo", "ItemResultadoId");
            CreateIndex("dbo.LabResultadoLaudo", "TabelaResultadoId");
            CreateIndex("dbo.LabResultadoLaudo", "UnidadeId");
            AddForeignKey("dbo.LabResultadoLaudo", "ItemResultadoId", "dbo.LabItemResultado", "Id");
            AddForeignKey("dbo.LabResultadoLaudo", "UnidadeId", "dbo.LabUnidade", "Id");
            AddForeignKey("dbo.LabResultadoLaudo", "ResultadoExameId", "dbo.LabResultadoExame", "Id");
            AddForeignKey("dbo.LabResultadoLaudo", "TabelaResultadoId", "dbo.LabTabelaResultado", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoLaudo", "TabelaResultadoId", "dbo.LabTabelaResultado");
            DropForeignKey("dbo.LabResultadoLaudo", "ResultadoExameId", "dbo.LabResultadoExame");
            DropForeignKey("dbo.LabResultadoLaudo", "UnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabResultadoLaudo", "ItemResultadoId", "dbo.LabItemResultado");
            DropIndex("dbo.LabResultadoLaudo", new[] { "UnidadeId" });
            DropIndex("dbo.LabResultadoLaudo", new[] { "TabelaResultadoId" });
            DropIndex("dbo.LabResultadoLaudo", new[] { "ItemResultadoId" });
            DropIndex("dbo.LabResultadoLaudo", new[] { "ResultadoExameId" });
        }
    }
}
