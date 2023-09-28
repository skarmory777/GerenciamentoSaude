namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Retirando_Abreviacoes_dos_nomes_dos_campos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoExame", "Observacao", c => c.String());
            AddColumn("dbo.LabResultadoExame", "Quantidade", c => c.Int());
            DropColumn("dbo.LabResultadoExame", "ObsExame");
            DropColumn("dbo.LabResultadoExame", "QtdExame");
        }

        public override void Down()
        {
            AddColumn("dbo.LabResultadoExame", "QtdExame", c => c.Int());
            AddColumn("dbo.LabResultadoExame", "ObsExame", c => c.String());
            DropColumn("dbo.LabResultadoExame", "Quantidade");
            DropColumn("dbo.LabResultadoExame", "Observacao");
        }
    }
}
