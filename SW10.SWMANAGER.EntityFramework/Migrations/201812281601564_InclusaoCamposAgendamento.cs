namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposAgendamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgendamentoConsulta", "IsConsulta", c => c.Boolean(nullable: false));
            AddColumn("dbo.AgendamentoConsulta", "IsCirurgia", c => c.Boolean(nullable: false));
            AddColumn("dbo.AgendamentoConsulta", "IsExames", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAgendamentoCirurgico", "AgendamentoConsultaId", c => c.Long(nullable: false));
            CreateIndex("dbo.AteAgendamentoCirurgico", "AgendamentoConsultaId");
            AddForeignKey("dbo.AteAgendamentoCirurgico", "AgendamentoConsultaId", "dbo.AgendamentoConsulta", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAgendamentoCirurgico", "AgendamentoConsultaId", "dbo.AgendamentoConsulta");
            DropIndex("dbo.AteAgendamentoCirurgico", new[] { "AgendamentoConsultaId" });
            DropColumn("dbo.AteAgendamentoCirurgico", "AgendamentoConsultaId");
            DropColumn("dbo.AgendamentoConsulta", "IsExames");
            DropColumn("dbo.AgendamentoConsulta", "IsCirurgia");
            DropColumn("dbo.AgendamentoConsulta", "IsConsulta");
        }
    }
}
