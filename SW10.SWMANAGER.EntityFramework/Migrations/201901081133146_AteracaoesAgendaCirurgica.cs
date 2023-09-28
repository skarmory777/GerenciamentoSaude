namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AteracaoesAgendaCirurgica : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", new[] { "AgendamentoCirurgicoId" });
            AddColumn("dbo.AteAgendamentoCirurgico", "AgendamentoSalaCirurgicaDisponibilidadeId", c => c.Long());
            CreateIndex("dbo.AteAgendamentoCirurgico", "AgendamentoSalaCirurgicaDisponibilidadeId");
            AddForeignKey("dbo.AteAgendamentoCirurgico", "AgendamentoSalaCirurgicaDisponibilidadeId", "dbo.AgendamentoSalaCirurgicaDisponibilidades", "Id");
            DropColumn("dbo.AgendamentoSalaCirurgicaDisponibilidades", "AgendamentoCirurgicoId");
        }

        public override void Down()
        {
            AddColumn("dbo.AgendamentoSalaCirurgicaDisponibilidades", "AgendamentoCirurgicoId", c => c.Long(nullable: false));
            DropForeignKey("dbo.AteAgendamentoCirurgico", "AgendamentoSalaCirurgicaDisponibilidadeId", "dbo.AgendamentoSalaCirurgicaDisponibilidades");
            DropIndex("dbo.AteAgendamentoCirurgico", new[] { "AgendamentoSalaCirurgicaDisponibilidadeId" });
            DropColumn("dbo.AteAgendamentoCirurgico", "AgendamentoSalaCirurgicaDisponibilidadeId");
            CreateIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", "AgendamentoCirurgicoId");
            AddForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico", "Id", cascadeDelete: true);
        }
    }
}
