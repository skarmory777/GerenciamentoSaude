namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class RecriandoTipoTabelaDominio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisTipoTabelaDominio",
                c => new
                {
                    Id = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_TipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.SisTabelaDominio", "TipoTabelaDominioId");
            CreateIndex("dbo.SisGrupoTipoTabelaDominio", "TipoTabelaDominioId");
            AddForeignKey("dbo.SisGrupoTipoTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio", "Id");
            AddForeignKey("dbo.SisTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio");
            DropForeignKey("dbo.SisGrupoTipoTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio");
            DropIndex("dbo.SisGrupoTipoTabelaDominio", new[] { "TipoTabelaDominioId" });
            DropIndex("dbo.SisTabelaDominio", new[] { "TipoTabelaDominioId" });
            DropTable("dbo.SisTipoTabelaDominio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
