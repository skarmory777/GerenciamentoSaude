namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoAgendamentoConsulta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgendamentoConsulta", "QuantidadeHorarios", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AgendamentoConsulta", "QuantidadeHorarios");
        }
    }
}
