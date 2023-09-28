namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class InclusaoTabelasAgendamento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAgendamentoCirurgico", "SalaCirurgicaId", "dbo.AteSalaCirurgica");
            DropIndex("dbo.AteAgendamentoCirurgico", new[] { "SalaCirurgicaId" });
            CreateTable(
                "dbo.AgendamentoSalaCirurgicaDisponibilidades",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AgendamentoCirurgicoId = c.Long(nullable: false),
                    SalaCirurgicaId = c.Long(nullable: false),
                    TipoCirurgiaId = c.Long(nullable: false),
                    IntervaloId = c.Long(nullable: false),
                    DataInicio = c.DateTime(nullable: false),
                    DataFim = c.DateTime(nullable: false),
                    HoraInicio = c.DateTime(nullable: false),
                    HoraFim = c.DateTime(nullable: false),
                    Domingo = c.Boolean(nullable: false),
                    Segunda = c.Boolean(nullable: false),
                    Terca = c.Boolean(nullable: false),
                    Quarta = c.Boolean(nullable: false),
                    Quinta = c.Boolean(nullable: false),
                    Sexta = c.Boolean(nullable: false),
                    Sabado = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoSalaCirurgicaDisponibilidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAgendamentoCirurgico", t => t.AgendamentoCirurgicoId, cascadeDelete: true)
                .ForeignKey("dbo.AteIntervalo", t => t.IntervaloId, cascadeDelete: true)
                .ForeignKey("dbo.AteSalaCirurgica", t => t.SalaCirurgicaId, cascadeDelete: true)
                .ForeignKey("dbo.AteTipoCirurgia", t => t.TipoCirurgiaId, cascadeDelete: true)
                .Index(t => t.AgendamentoCirurgicoId)
                .Index(t => t.SalaCirurgicaId)
                .Index(t => t.TipoCirurgiaId)
                .Index(t => t.IntervaloId);

            CreateTable(
                "dbo.AteTipoCirurgia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoCirurgia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            DropColumn("dbo.AteAgendamentoCirurgico", "SalaCirurgicaId");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAgendamentoCirurgico", "SalaCirurgicaId", c => c.Long());
            DropForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "TipoCirurgiaId", "dbo.AteTipoCirurgia");
            DropForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "SalaCirurgicaId", "dbo.AteSalaCirurgica");
            DropForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "IntervaloId", "dbo.AteIntervalo");
            DropForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "AgendamentoCirurgicoId", "dbo.AteAgendamentoCirurgico");
            DropIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", new[] { "IntervaloId" });
            DropIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", new[] { "TipoCirurgiaId" });
            DropIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", new[] { "SalaCirurgicaId" });
            DropIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", new[] { "AgendamentoCirurgicoId" });
            DropTable("dbo.AteTipoCirurgia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoCirurgia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AgendamentoSalaCirurgicaDisponibilidades",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoSalaCirurgicaDisponibilidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.AteAgendamentoCirurgico", "SalaCirurgicaId");
            AddForeignKey("dbo.AteAgendamentoCirurgico", "SalaCirurgicaId", "dbo.AteSalaCirurgica", "Id");
        }
    }
}
