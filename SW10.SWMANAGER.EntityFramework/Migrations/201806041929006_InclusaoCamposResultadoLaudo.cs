namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposResultadoLaudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoLaudo", "TipoResultadoId", c => c.Long());
            AddColumn("dbo.LabResultadoLaudo", "CasaDecimal", c => c.Int());
            CreateIndex("dbo.LabResultadoLaudo", "TipoResultadoId");
            AddForeignKey("dbo.LabResultadoLaudo", "TipoResultadoId", "dbo.LabTipoResultado", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoLaudo", "TipoResultadoId", "dbo.LabTipoResultado");
            DropIndex("dbo.LabResultadoLaudo", new[] { "TipoResultadoId" });
            DropColumn("dbo.LabResultadoLaudo", "CasaDecimal");
            DropColumn("dbo.LabResultadoLaudo", "TipoResultadoId");
        }
    }
}
