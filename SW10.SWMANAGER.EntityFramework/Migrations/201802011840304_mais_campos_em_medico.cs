namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class mais_campos_em_medico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisMedico", "CodigoCredenciamentoConvenio", c => c.String());
            AddColumn("dbo.SisMedico", "IsAtivo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisMedico", "IsCorpoClinico", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisMedico", "Apelido", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisMedico", "Apelido");
            DropColumn("dbo.SisMedico", "IsCorpoClinico");
            DropColumn("dbo.SisMedico", "IsAtivo");
            DropColumn("dbo.SisMedico", "CodigoCredenciamentoConvenio");
        }
    }
}
