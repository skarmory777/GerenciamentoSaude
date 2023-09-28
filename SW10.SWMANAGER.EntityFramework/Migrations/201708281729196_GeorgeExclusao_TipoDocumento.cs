namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeExclusao_TipoDocumento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "TipoDocumentoId", "dbo.TipoDocumento");
            DropForeignKey("dbo.EstoqueMovimento", "TipoDocumentoId", "dbo.TipoDocumento");
            DropIndex("dbo.EstoqueMovimento", new[] { "TipoDocumentoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "TipoDocumentoId" });
            DropTable("dbo.TipoDocumento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoDocumento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.TipoDocumento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 30),
                    IsEntrada = c.Boolean(nullable: false),
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
                    { "DynamicFilter_TipoDocumento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.EstoquePreMovimento", "TipoDocumentoId");
            CreateIndex("dbo.EstoqueMovimento", "TipoDocumentoId");
            AddForeignKey("dbo.EstoqueMovimento", "TipoDocumentoId", "dbo.TipoDocumento", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "TipoDocumentoId", "dbo.TipoDocumento", "Id");
        }
    }
}
