namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Marcus_AdmissaoMedica : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdmissaoMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    DataAdmissao = c.DateTime(nullable: false),
                    UnidadeInternacaoId = c.Long(nullable: false),
                    AtendimentoId = c.Long(nullable: false),
                    PrestadorId = c.Long(nullable: false),
                    Observacao = c.String(),
                    FormConfigId = c.Long(),
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
                    { "DynamicFilter_AdmissaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId, cascadeDelete: false)
                .ForeignKey("dbo.FormConfig", t => t.FormConfigId)
                .ForeignKey("dbo.Prestador", t => t.PrestadorId, cascadeDelete: false)
                .ForeignKey("dbo.UnidadeInternacao", t => t.UnidadeInternacaoId, cascadeDelete: false)
                .Index(t => t.UnidadeInternacaoId)
                .Index(t => t.AtendimentoId)
                .Index(t => t.PrestadorId)
                .Index(t => t.FormConfigId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AdmissaoMedica", "UnidadeInternacaoId", "dbo.UnidadeInternacao");
            DropForeignKey("dbo.AdmissaoMedica", "PrestadorId", "dbo.Prestador");
            DropForeignKey("dbo.AdmissaoMedica", "FormConfigId", "dbo.FormConfig");
            DropForeignKey("dbo.AdmissaoMedica", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.AdmissaoMedica", new[] { "FormConfigId" });
            DropIndex("dbo.AdmissaoMedica", new[] { "PrestadorId" });
            DropIndex("dbo.AdmissaoMedica", new[] { "AtendimentoId" });
            DropIndex("dbo.AdmissaoMedica", new[] { "UnidadeInternacaoId" });
            DropTable("dbo.AdmissaoMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AdmissaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
