namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_Guia_E_Grupo_OutrasDespesas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatGuia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 100),
                    IsAmbulatorio = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoGuia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FatGuiaConvenio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(),
                    GuiaId = c.Long(),
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
                    { "DynamicFilter_FaturamentoGuiaConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId)
                .ForeignKey("dbo.FatGuia", t => t.GuiaId)
                .Index(t => t.ConvenioId)
                .Index(t => t.GuiaId);

            AddColumn("dbo.FatGrupo", "CodTipoOutraDespesa", c => c.String());
            AddColumn("dbo.FatGrupo", "IsOutraDespesa", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatGuiaConvenio", "GuiaId", "dbo.FatGuia");
            DropForeignKey("dbo.FatGuiaConvenio", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatGuiaConvenio", new[] { "GuiaId" });
            DropIndex("dbo.FatGuiaConvenio", new[] { "ConvenioId" });
            DropColumn("dbo.FatGrupo", "IsOutraDespesa");
            DropColumn("dbo.FatGrupo", "CodTipoOutraDespesa");
            DropTable("dbo.FatGuiaConvenio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGuiaConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatGuia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGuia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
