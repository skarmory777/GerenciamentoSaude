namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Lau_Grupo_Modelo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LauGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_LaudoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LauModeloLaudo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    LauGrupoId = c.Long(nullable: false),
                    Modelo = c.String(),
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
                    { "DynamicFilter_LaudoModeloLaudo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LauGrupo", t => t.LauGrupoId, cascadeDelete: true)
                .Index(t => t.LauGrupoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.LauModeloLaudo", "LauGrupoId", "dbo.LauGrupo");
            DropIndex("dbo.LauModeloLaudo", new[] { "LauGrupoId" });
            DropTable("dbo.LauModeloLaudo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaudoModeloLaudo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LauGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaudoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
