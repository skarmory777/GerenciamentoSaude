namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoAgendamentoItemFaturamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAgendamentoItemFaturamento", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AteAgendamentoItemFaturamento", "IsCirurgica", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AteAgendamentoItemFaturamento", "IsCirurgica");
            DropColumn("dbo.AteAgendamentoItemFaturamento", "Quantidade");
        }
    }
}
