namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoSalaCirurgica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteSalaCirurgica", "CorAgendamento", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AteSalaCirurgica", "CorAgendamento");
        }
    }
}
