namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_Ulili : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabColetaExameInformacao", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabColetaExame", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabTecnico", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabFormatacao", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabEquipamento", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabUnidade", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabTabelaResultado", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabTipoResultado", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.LabMetodo", "Codigo", c => c.String(maxLength: 10));
        }

        public override void Down()
        {
            DropColumn("dbo.LabColetaExameInformacao", "Codigo");
            DropColumn("dbo.LabColetaExame", "Codigo");
            DropColumn("dbo.LabFormatacao", "Codigo");
            DropColumn("dbo.LabEquipamento", "Codigo");
            DropColumn("dbo.LabUnidade", "Codigo");
            DropColumn("dbo.LabTabelaResultado", "Codigo");
            DropColumn("dbo.LabTipoResultado", "Codigo");
            DropColumn("dbo.LabMetodo", "Codigo");
        }
    }
}
