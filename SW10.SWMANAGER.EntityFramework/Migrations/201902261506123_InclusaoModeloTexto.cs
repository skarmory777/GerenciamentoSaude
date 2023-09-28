namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class InclusaoModeloTexto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TamanhoModelo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    LarguraPixel = c.Double(nullable: false),
                    AlturaPixel = c.Double(nullable: false),
                    LarguraCm = c.Double(nullable: false),
                    AlturaCm = c.Double(nullable: false),
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
                    { "DynamicFilter_TamanhoModelo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoModelo",
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
                    { "DynamicFilter_TipoModelo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoModeloVariaveis",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TipoModeloId = c.Long(nullable: false),
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
                    { "DynamicFilter_TipoModeloVariaveis_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoModelo", t => t.TipoModeloId, cascadeDelete: true)
                .Index(t => t.TipoModeloId);

            AddColumn("dbo.AteTextoModelo", "TipoModeloId", c => c.Long());
            AddColumn("dbo.AteTextoModelo", "TamanhoModeloId", c => c.Long());
            CreateIndex("dbo.AteTextoModelo", "TipoModeloId");
            CreateIndex("dbo.AteTextoModelo", "TamanhoModeloId");
            AddForeignKey("dbo.AteTextoModelo", "TamanhoModeloId", "dbo.TamanhoModelo", "Id");
            AddForeignKey("dbo.AteTextoModelo", "TipoModeloId", "dbo.TipoModelo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteTextoModelo", "TipoModeloId", "dbo.TipoModelo");
            DropForeignKey("dbo.TipoModeloVariaveis", "TipoModeloId", "dbo.TipoModelo");
            DropForeignKey("dbo.AteTextoModelo", "TamanhoModeloId", "dbo.TamanhoModelo");
            DropIndex("dbo.TipoModeloVariaveis", new[] { "TipoModeloId" });
            DropIndex("dbo.AteTextoModelo", new[] { "TamanhoModeloId" });
            DropIndex("dbo.AteTextoModelo", new[] { "TipoModeloId" });
            DropColumn("dbo.AteTextoModelo", "TamanhoModeloId");
            DropColumn("dbo.AteTextoModelo", "TipoModeloId");
            DropTable("dbo.TipoModeloVariaveis",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoModeloVariaveis_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoModelo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoModelo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TamanhoModelo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TamanhoModelo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
