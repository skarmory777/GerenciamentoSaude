namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_campo_isativo_em_especialidade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisEspecialidade", "IsAtivo", c => c.Long(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.SisEspecialidade", "IsAtivo");
        }
    }
}
