namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicionando_IsEletivo_IsUrgencia_Convenio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "XmlPrimeirosDigitosMatricula", c => c.Int(nullable: false));
            AddColumn("dbo.SisConvenio", "IsEletivo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsUrgencia", c => c.Boolean(nullable: false));
            DropColumn("dbo.SisConvenio", "XmlPrimeirosDititosMatricula");
        }

        public override void Down()
        {
            AddColumn("dbo.SisConvenio", "XmlPrimeirosDititosMatricula", c => c.Int(nullable: false));
            DropColumn("dbo.SisConvenio", "IsUrgencia");
            DropColumn("dbo.SisConvenio", "IsEletivo");
            DropColumn("dbo.SisConvenio", "XmlPrimeirosDigitosMatricula");
        }
    }
}
