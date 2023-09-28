namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposAgendamentos_faturamentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "IsAgendaConsulta", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsAgendaCirurgia", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "IsAgendaExame", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatItem", "QuantidadeMinutos", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FatItem", "QuantidadeMinutos");
            DropColumn("dbo.FatItem", "IsAgendaExame");
            DropColumn("dbo.FatItem", "IsAgendaCirurgia");
            DropColumn("dbo.FatItem", "IsAgendaConsulta");
        }
    }
}
