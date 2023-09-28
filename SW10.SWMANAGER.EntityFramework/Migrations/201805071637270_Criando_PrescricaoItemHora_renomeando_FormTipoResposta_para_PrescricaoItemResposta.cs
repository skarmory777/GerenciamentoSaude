namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Criando_PrescricaoItemHora_renomeando_FormTipoResposta_para_PrescricaoItemResposta : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AssFormTipoResposta", newName: "AssPrescricaoItemResposta");
            CreateTable(
                "dbo.AssPrescricaoItemHora",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AssPrescricaoItemRespostaId = c.Long(),
                    DiaMedicamento = c.Int(nullable: false),
                    DataMedicamento = c.DateTime(nullable: false),
                    Hora = c.String(),
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
                    { "DynamicFilter_PrescricaoItemHora_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssPrescricaoItemResposta", t => t.AssPrescricaoItemRespostaId)
                .Index(t => t.AssPrescricaoItemRespostaId);

            AlterTableAnnotations(
                "dbo.AssPrescricaoItemResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Quantidade = c.Decimal(precision: 18, scale: 2),
                    EstUnidadeId = c.Long(),
                    AssVelocidadeInfusaoId = c.Long(),
                    AssFormaAplicacaoId = c.Long(),
                    AssFrequenciaId = c.Long(),
                    IsSeNecessario = c.Boolean(nullable: false),
                    IsUrgente = c.Boolean(nullable: false),
                    IsDias = c.Boolean(nullable: false),
                    SisUnidadeOrganizacionalId = c.Long(),
                    SisMedicoId = c.Long(),
                    DataInicial = c.DateTime(nullable: false),
                    TotalDias = c.Int(),
                    Observacao = c.String(),
                    AssPrescricaoItemId = c.Long(),
                    AssDivisaoId = c.Long(),
                    AssPrescricaoMedicaId = c.Long(),
                    AssPrescricaoItemStatusId = c.Long(),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_FormTipoResposta_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    {
                        "DynamicFilter_PrescricaoItemResposta_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AddColumn("dbo.AssPrescricaoItemResposta", "IsDias", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssFrequencia", "HoraInicialMedicacao", c => c.String());
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoItemHora", "AssPrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta");
            DropIndex("dbo.AssPrescricaoItemHora", new[] { "AssPrescricaoItemRespostaId" });
            DropColumn("dbo.AssFrequencia", "HoraInicialMedicacao");
            DropColumn("dbo.AssPrescricaoItemResposta", "IsDias");
            AlterTableAnnotations(
                "dbo.AssPrescricaoItemResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Quantidade = c.Decimal(precision: 18, scale: 2),
                    EstUnidadeId = c.Long(),
                    AssVelocidadeInfusaoId = c.Long(),
                    AssFormaAplicacaoId = c.Long(),
                    AssFrequenciaId = c.Long(),
                    IsSeNecessario = c.Boolean(nullable: false),
                    IsUrgente = c.Boolean(nullable: false),
                    IsDias = c.Boolean(nullable: false),
                    SisUnidadeOrganizacionalId = c.Long(),
                    SisMedicoId = c.Long(),
                    DataInicial = c.DateTime(nullable: false),
                    TotalDias = c.Int(),
                    Observacao = c.String(),
                    AssPrescricaoItemId = c.Long(),
                    AssDivisaoId = c.Long(),
                    AssPrescricaoMedicaId = c.Long(),
                    AssPrescricaoItemStatusId = c.Long(),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_FormTipoResposta_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    {
                        "DynamicFilter_PrescricaoItemResposta_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            DropTable("dbo.AssPrescricaoItemHora",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrescricaoItemHora_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameTable(name: "dbo.AssPrescricaoItemResposta", newName: "AssFormTipoResposta");
        }
    }
}
