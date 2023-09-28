namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ranca_fora_campo_isativo_em_especialidade : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SisEspecialidade", "IsAtivo");
        }

        public override void Down()
        {
            AddColumn("dbo.SisEspecialidade", "IsAtivo", c => c.Long(nullable: false));
        }
    }
}
