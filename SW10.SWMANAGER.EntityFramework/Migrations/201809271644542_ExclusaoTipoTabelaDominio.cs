namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class ExclusaoTipoTabelaDominio : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.SisGrupoTipoTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio");

            DropForeignKey("dbo.SisGrupoTipoTabelaDominio", "FK_dbo.GrupoTipoTabelaDominio_dbo.TipoTabelaDominio_TipoTabelaDominioId");

            //DropForeignKey("dbo.SisTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio");

            DropForeignKey("dbo.SisTabelaDominio", "FK_dbo.TabelaDominio_dbo.TipoTabelaDominio_TipoTabelaDominioId");

            DropIndex("dbo.SisTabelaDominio", new[] { "TipoTabelaDominioId" });
            DropIndex("dbo.SisGrupoTipoTabelaDominio", new[] { "TipoTabelaDominioId" });
            DropTable("dbo.SisTipoTabelaDominio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.SisTipoTabelaDominio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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

            CreateIndex("dbo.SisGrupoTipoTabelaDominio", "TipoTabelaDominioId");
            CreateIndex("dbo.SisTabelaDominio", "TipoTabelaDominioId");
            AddForeignKey("dbo.SisTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio", "Id");
            AddForeignKey("dbo.SisGrupoTipoTabelaDominio", "TipoTabelaDominioId", "dbo.SisTipoTabelaDominio", "Id");
        }
    }
}
