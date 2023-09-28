namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Removendo_Tabelas_inutilizadas_Adicionando_campos_em_Frequencia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssParecerEspecialistaResposta", "SisMedicoRespondenteId", "dbo.SisMedico");
            DropForeignKey("dbo.AssParecerEspecialistaResposta", "AssParecerEspecialistaSolicitacaoId", "dbo.AssParecerEspecialista");
            DropForeignKey("dbo.AssParecerEspecialista", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.AssParecerEspecialista", "SisEspecialidadeSolicitadaId", "dbo.SisEspecialidade");
            DropForeignKey("dbo.AssParecerEspecialista", "SisMedicoSolicitadoId", "dbo.SisMedico");
            DropForeignKey("dbo.AssParecerEspecialista", "SisMedicoSolicitanteId", "dbo.SisMedico");
            DropForeignKey("dbo.AssAdmissaoMedica", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.AssAdmissaoMedica", "FormRespostaId", "dbo.SisFormResposta");
            DropForeignKey("dbo.AssAdmissaoMedica", "PrestadorId", "dbo.SisPrestador");
            DropForeignKey("dbo.AssAdmissaoMedica", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.AssEvolucaoMedica", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.AssEvolucaoMedica", "SisFormRespostaId", "dbo.SisFormResposta");
            DropIndex("dbo.AssAdmissaoMedica", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.AssAdmissaoMedica", new[] { "AtendimentoId" });
            DropIndex("dbo.AssAdmissaoMedica", new[] { "PrestadorId" });
            DropIndex("dbo.AssAdmissaoMedica", new[] { "FormRespostaId" });
            DropIndex("dbo.AssParecerEspecialistaResposta", new[] { "AssParecerEspecialistaSolicitacaoId" });
            DropIndex("dbo.AssParecerEspecialistaResposta", new[] { "SisMedicoRespondenteId" });
            DropIndex("dbo.AssParecerEspecialista", new[] { "AtendimentoId" });
            DropIndex("dbo.AssParecerEspecialista", new[] { "SisMedicoSolicitanteId" });
            DropIndex("dbo.AssParecerEspecialista", new[] { "SisMedicoSolicitadoId" });
            DropIndex("dbo.AssParecerEspecialista", new[] { "SisEspecialidadeSolicitadaId" });
            DropIndex("dbo.AssEvolucaoMedica", new[] { "AtendimentoId" });
            DropIndex("dbo.AssEvolucaoMedica", new[] { "SisFormRespostaId" });
            AddColumn("dbo.AssFrequencia", "Horarios", c => c.String());
            AddColumn("dbo.AssFrequencia", "IsSos", c => c.Boolean(nullable: false));
            DropTable("dbo.AssAdmissaoMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AdmissaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssParecerEspecialistaResposta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ParecerEspecialistaResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssParecerEspecialista",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ParecerEspecialista_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssPrescricaoMedicaAprazamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrescricaoMedicaAprazamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssEvolucaoMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EvolucaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.AssEvolucaoMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AtendimentoId = c.Long(nullable: false),
                    SisFormRespostaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_EvolucaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssPrescricaoMedicaAprazamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Intervalo = c.Int(nullable: false),
                    Horarios = c.String(),
                    IsSos = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_PrescricaoMedicaAprazamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssParecerEspecialistaResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataParecer = c.DateTime(nullable: false),
                    AssParecerEspecialistaSolicitacaoId = c.Long(nullable: false),
                    SisMedicoRespondenteId = c.Long(nullable: false),
                    Parecer = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssParecerEspecialista",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataSolicitacao = c.DateTime(nullable: false),
                    DescricaoSolicitacao = c.String(),
                    AtendimentoId = c.Long(nullable: false),
                    SisMedicoSolicitanteId = c.Long(nullable: false),
                    SisMedicoSolicitadoId = c.Long(),
                    SisEspecialidadeSolicitadaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssAdmissaoMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataAdmissao = c.DateTime(nullable: false),
                    SisUnidadeOrganizacionalId = c.Long(nullable: false),
                    AtendimentoId = c.Long(nullable: false),
                    PrestadorId = c.Long(nullable: false),
                    Observacao = c.String(),
                    FormRespostaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_AdmissaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            DropColumn("dbo.AssFrequencia", "IsSos");
            DropColumn("dbo.AssFrequencia", "Horarios");
            CreateIndex("dbo.AssEvolucaoMedica", "SisFormRespostaId");
            CreateIndex("dbo.AssEvolucaoMedica", "AtendimentoId");
            CreateIndex("dbo.AssParecerEspecialistaResposta", "SisMedicoRespondenteId");
            CreateIndex("dbo.AssParecerEspecialistaResposta", "AssParecerEspecialistaSolicitacaoId");
            CreateIndex("dbo.AssParecerEspecialista", "SisEspecialidadeSolicitadaId");
            CreateIndex("dbo.AssParecerEspecialista", "SisMedicoSolicitadoId");
            CreateIndex("dbo.AssParecerEspecialista", "SisMedicoSolicitanteId");
            CreateIndex("dbo.AssParecerEspecialista", "AtendimentoId");
            CreateIndex("dbo.AssAdmissaoMedica", "FormRespostaId");
            CreateIndex("dbo.AssAdmissaoMedica", "PrestadorId");
            CreateIndex("dbo.AssAdmissaoMedica", "AtendimentoId");
            CreateIndex("dbo.AssAdmissaoMedica", "SisUnidadeOrganizacionalId");
            AddForeignKey("dbo.AssEvolucaoMedica", "SisFormRespostaId", "dbo.SisFormResposta", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssEvolucaoMedica", "AtendimentoId", "dbo.AteAtendimento", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssAdmissaoMedica", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssAdmissaoMedica", "PrestadorId", "dbo.SisPrestador", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssAdmissaoMedica", "FormRespostaId", "dbo.SisFormResposta", "Id");
            AddForeignKey("dbo.AssAdmissaoMedica", "AtendimentoId", "dbo.AteAtendimento", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssParecerEspecialistaResposta", "AssParecerEspecialistaSolicitacaoId", "dbo.AssParecerEspecialista", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssParecerEspecialistaResposta", "SisMedicoRespondenteId", "dbo.SisMedico", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssParecerEspecialista", "SisMedicoSolicitanteId", "dbo.SisMedico", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssParecerEspecialista", "SisMedicoSolicitadoId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.AssParecerEspecialista", "SisEspecialidadeSolicitadaId", "dbo.SisEspecialidade", "Id");
            AddForeignKey("dbo.AssParecerEspecialista", "AtendimentoId", "dbo.AteAtendimento", "Id", cascadeDelete: true);
        }
    }
}
