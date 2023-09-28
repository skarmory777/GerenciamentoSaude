namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class gilberto_add_EstoqueGrupo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Est_EstoqueGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoqueId = c.Long(),
                    GrupoId = c.Long(),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_EstoqueGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Estoque", t => t.EstoqueId)
                .ForeignKey("dbo.Est_Grupo", t => t.GrupoId)
                .Index(t => t.EstoqueId)
                .Index(t => t.GrupoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Est_EstoqueGrupo", "GrupoId", "dbo.Est_Grupo");
            DropForeignKey("dbo.Est_EstoqueGrupo", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.Est_EstoqueGrupo", new[] { "GrupoId" });
            DropIndex("dbo.Est_EstoqueGrupo", new[] { "EstoqueId" });
            DropTable("dbo.Est_EstoqueGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
