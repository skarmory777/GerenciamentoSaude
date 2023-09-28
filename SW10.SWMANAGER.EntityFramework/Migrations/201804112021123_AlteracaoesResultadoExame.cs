namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoesResultadoExame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LabResultadoExame", "LabExameId", "dbo.LabExame");
            DropIndex("dbo.LabResultadoExame", new[] { "LabExameId" });
            AddColumn("dbo.LabResultadoExame", "LabFaturamentoItemId", c => c.Long());
            CreateIndex("dbo.LabResultadoExame", "LabFaturamentoItemId");
            AddForeignKey("dbo.LabResultadoExame", "LabFaturamentoItemId", "dbo.FatItem", "Id");
            DropColumn("dbo.LabResultadoExame", "LabExameId");
        }

        public override void Down()
        {
            AddColumn("dbo.LabResultadoExame", "LabExameId", c => c.Long());
            DropForeignKey("dbo.LabResultadoExame", "LabFaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.LabResultadoExame", new[] { "LabFaturamentoItemId" });
            DropColumn("dbo.LabResultadoExame", "LabFaturamentoItemId");
            CreateIndex("dbo.LabResultadoExame", "LabExameId");
            AddForeignKey("dbo.LabResultadoExame", "LabExameId", "dbo.LabExame", "Id");
        }
    }
}
