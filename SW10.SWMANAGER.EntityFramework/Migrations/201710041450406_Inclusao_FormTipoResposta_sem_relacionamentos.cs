namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_FormTipoResposta_sem_relacionamentos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssFormTipoResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    UnidadeId = c.Long(nullable: false),
                    VelocidadeInfusaoId = c.Long(nullable: false),
                    AplicacaoId = c.Long(nullable: false),
                    FrequenciaId = c.Long(nullable: false),
                    IsSeNecessario = c.Boolean(nullable: false),
                    IsUrgente = c.Boolean(nullable: false),
                    UnidadeOrganizacionalId = c.Long(nullable: false),
                    MedicoId = c.Long(nullable: false),
                    DataInicial = c.DateTime(nullable: false),
                    DiaAtual = c.Int(nullable: false),
                    TotalDias = c.Int(nullable: false),
                    Observacao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_FormTipoResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.AssFormTipoResposta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormTipoResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
