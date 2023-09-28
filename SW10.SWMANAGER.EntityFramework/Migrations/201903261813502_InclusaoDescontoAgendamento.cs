namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoDescontoAgendamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAgendamentoItemFaturamento", "ValorDesconto", c => c.Decimal(precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.AteAgendamentoItemFaturamento", "ValorDesconto");
        }
    }
}
