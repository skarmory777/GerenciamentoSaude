namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeMotivoPerdaProduto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TipoDocumento", "TipoEntradaId", "dbo.TipoEntrada");
            DropIndex("dbo.TipoDocumento", new[] { "TipoEntradaId" });
            CreateTable(
                "dbo.MotivoPerdaProduto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_MotivoPerdaProduto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.TipoDocumento", "IsEntrada", c => c.Boolean(nullable: false));
            DropColumn("dbo.TipoDocumento", "IsDocumentoFiscal");
            DropColumn("dbo.TipoDocumento", "IsAtualizaPreco");
            DropColumn("dbo.TipoDocumento", "TipoEntradaId");
        }

        public override void Down()
        {
            AddColumn("dbo.TipoDocumento", "TipoEntradaId", c => c.Long(nullable: false));
            AddColumn("dbo.TipoDocumento", "IsAtualizaPreco", c => c.Boolean(nullable: false));
            AddColumn("dbo.TipoDocumento", "IsDocumentoFiscal", c => c.Boolean(nullable: false));
            DropColumn("dbo.TipoDocumento", "IsEntrada");
            DropTable("dbo.MotivoPerdaProduto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoPerdaProduto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.TipoDocumento", "TipoEntradaId");
            AddForeignKey("dbo.TipoDocumento", "TipoEntradaId", "dbo.TipoEntrada", "Id", cascadeDelete: true);
        }
    }
}
