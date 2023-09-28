namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Marcus_ParecerEspecialista : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParecerEspecialista",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataSolicitacao = c.DateTime(nullable: false),
                    DescricaoSolicitacao = c.String(),
                    AtendimentoId = c.Long(nullable: false),
                    MedicoSolicitanteId = c.Long(nullable: false),
                    MedicoSolicitadoId = c.Long(),
                    EspecialidadeSolicitadaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ParecerEspecialista_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId, cascadeDelete: false)
                .ForeignKey("dbo.Especialidade", t => t.EspecialidadeSolicitadaId)
                .ForeignKey("dbo.Medico", t => t.MedicoSolicitadoId)
                .ForeignKey("dbo.Medico", t => t.MedicoSolicitanteId, cascadeDelete: false)
                .Index(t => t.AtendimentoId)
                .Index(t => t.MedicoSolicitanteId)
                .Index(t => t.MedicoSolicitadoId)
                .Index(t => t.EspecialidadeSolicitadaId);

            CreateTable(
                "dbo.ParecerEspecialistaResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataParecer = c.DateTime(nullable: false),
                    ParecerEspecialistaSolicitacaoId = c.Long(nullable: false),
                    MedicoRespondenteId = c.Long(nullable: false),
                    Parecer = c.String(),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ParecerEspecialistaResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medico", t => t.MedicoRespondenteId, cascadeDelete: false)
                .ForeignKey("dbo.ParecerEspecialista", t => t.ParecerEspecialistaSolicitacaoId, cascadeDelete: false)
                .Index(t => t.ParecerEspecialistaSolicitacaoId)
                .Index(t => t.MedicoRespondenteId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ParecerEspecialistaResposta", "ParecerEspecialistaSolicitacaoId", "dbo.ParecerEspecialista");
            DropForeignKey("dbo.ParecerEspecialistaResposta", "MedicoRespondenteId", "dbo.Medico");
            DropForeignKey("dbo.ParecerEspecialista", "MedicoSolicitanteId", "dbo.Medico");
            DropForeignKey("dbo.ParecerEspecialista", "MedicoSolicitadoId", "dbo.Medico");
            DropForeignKey("dbo.ParecerEspecialista", "EspecialidadeSolicitadaId", "dbo.Especialidade");
            DropForeignKey("dbo.ParecerEspecialista", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.ParecerEspecialistaResposta", new[] { "MedicoRespondenteId" });
            DropIndex("dbo.ParecerEspecialistaResposta", new[] { "ParecerEspecialistaSolicitacaoId" });
            DropIndex("dbo.ParecerEspecialista", new[] { "EspecialidadeSolicitadaId" });
            DropIndex("dbo.ParecerEspecialista", new[] { "MedicoSolicitadoId" });
            DropIndex("dbo.ParecerEspecialista", new[] { "MedicoSolicitanteId" });
            DropIndex("dbo.ParecerEspecialista", new[] { "AtendimentoId" });
            DropTable("dbo.ParecerEspecialistaResposta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ParecerEspecialistaResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ParecerEspecialista",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ParecerEspecialista_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
