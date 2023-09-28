namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicionando_HoraInicialPrescricao_em_UnidadeOrganizacional : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisUnidadeOrganizacional", "HoraInicioPrescricao", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisUnidadeOrganizacional", "HoraInicioPrescricao");
        }
    }
}
