namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Projeto_Intervalo_Responsavel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisTarefaIntervalo", "ResponsavelId", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.SisTarefaIntervalo", "ResponsavelId");
        }
    }
}
