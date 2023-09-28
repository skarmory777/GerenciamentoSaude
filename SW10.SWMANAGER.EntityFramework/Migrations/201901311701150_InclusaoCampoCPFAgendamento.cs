namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoCPFAgendamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgendamentoConsulta", "CPF", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AgendamentoConsulta", "CPF");
        }
    }
}
