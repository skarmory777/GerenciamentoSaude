namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_Taxas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatTaxa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    DataInicio = c.DateTime(),
                    DataFim = c.DateTime(),
                    Percentual = c.Double(nullable: false),
                    IsAmbulatorio = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsIncideFilme = c.Boolean(nullable: false),
                    IsIncideManual = c.Boolean(nullable: false),
                    IsImplicita = c.Boolean(nullable: false),
                    IsTodosLocal = c.Boolean(nullable: false),
                    IsTodosTurno = c.Boolean(nullable: false),
                    IsTodosTipoLeito = c.Boolean(nullable: false),
                    IsTodosGrupo = c.Boolean(nullable: false),
                    IsTodosItem = c.Boolean(nullable: false),
                    IsTodosConvenio = c.Boolean(nullable: false),
                    IsTodosPlano = c.Boolean(nullable: false),
                    LocalImpressao = c.String(),
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
                    { "DynamicFilter_FaturamentoTaxa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.FatTaxa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
