namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_GrupoConvenio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatGrupoConvenio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(),
                    GrupoId = c.Long(),
                    IsOutraDespesa = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoGrupoConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId)
                .ForeignKey("dbo.FatGrupo", t => t.GrupoId)
                .Index(t => t.ConvenioId)
                .Index(t => t.GrupoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatGrupoConvenio", "GrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatGrupoConvenio", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatGrupoConvenio", new[] { "GrupoId" });
            DropIndex("dbo.FatGrupoConvenio", new[] { "ConvenioId" });
            DropTable("dbo.FatGrupoConvenio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGrupoConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
