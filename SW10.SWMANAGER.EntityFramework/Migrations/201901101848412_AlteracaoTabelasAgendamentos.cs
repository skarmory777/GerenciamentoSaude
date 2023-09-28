namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTabelasAgendamentos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgendamentoConsulta", "MedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.AgendamentoConsulta", new[] { "MedicoId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "MedicoEspecialidadeId" });
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "AgendamentoCirurgicoId" });
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "FaturamentoItemId" });
            AlterColumn("dbo.AgendamentoConsulta", "MedicoId", c => c.Long());
            AlterColumn("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", c => c.Long());
            AlterColumn("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", c => c.Long());
            AlterColumn("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", c => c.Long());
            CreateIndex("dbo.AgendamentoConsulta", "MedicoId");
            CreateIndex("dbo.AgendamentoConsulta", "MedicoEspecialidadeId");
            CreateIndex("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId");
            CreateIndex("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId");
            AddForeignKey("dbo.AgendamentoConsulta", "MedicoId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", "dbo.SisMedicoEspecialidade", "Id");
            AddForeignKey("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico", "Id");
            AddForeignKey("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", "dbo.FatItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropForeignKey("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", "dbo.SisMedicoEspecialidade");
            DropForeignKey("dbo.AgendamentoConsulta", "MedicoId", "dbo.SisMedico");
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AteAgendamentoItemFaturamento", new[] { "AgendamentoCirurgicoId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "MedicoEspecialidadeId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "MedicoId" });
            AlterColumn("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", c => c.Long(nullable: false));
            AlterColumn("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", c => c.Long(nullable: false));
            AlterColumn("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", c => c.Long(nullable: false));
            AlterColumn("dbo.AgendamentoConsulta", "MedicoId", c => c.Long(nullable: false));
            CreateIndex("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId");
            CreateIndex("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId");
            CreateIndex("dbo.AgendamentoConsulta", "MedicoEspecialidadeId");
            CreateIndex("dbo.AgendamentoConsulta", "MedicoId");
            AddForeignKey("dbo.AteAgendamentoItemFaturamento", "FaturamentoItemId", "dbo.FatItem", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AteAgendamentoItemFaturamento", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", "dbo.SisMedicoEspecialidade", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AgendamentoConsulta", "MedicoId", "dbo.SisMedico", "Id", cascadeDelete: true);
        }
    }
}
