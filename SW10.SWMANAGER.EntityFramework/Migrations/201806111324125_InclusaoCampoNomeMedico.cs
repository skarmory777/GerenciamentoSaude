namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoNomeMedico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultado", "NomeMedicoSolicitante", c => c.String());
            AddColumn("dbo.LabResultado", "CRMSolicitante", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.LabResultado", "CRMSolicitante");
            DropColumn("dbo.LabResultado", "NomeMedicoSolicitante");
        }
    }
}
