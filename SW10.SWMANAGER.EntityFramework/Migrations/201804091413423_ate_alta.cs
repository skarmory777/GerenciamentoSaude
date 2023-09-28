namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class ate_alta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAlta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Data = c.DateTime(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    AteGrupoCIDId = c.Long(),
                    AteMotivoAltaId = c.Long(),
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
                    { "DynamicFilter_Alta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoCID", t => t.AteGrupoCIDId)
                .ForeignKey("dbo.AssMotivoAlta", t => t.AteMotivoAltaId)
                .Index(t => t.AteGrupoCIDId)
                .Index(t => t.AteMotivoAltaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAlta", "AteMotivoAltaId", "dbo.AssMotivoAlta");
            DropForeignKey("dbo.AteAlta", "AteGrupoCIDId", "dbo.GrupoCID");
            DropIndex("dbo.AteAlta", new[] { "AteMotivoAltaId" });
            DropIndex("dbo.AteAlta", new[] { "AteGrupoCIDId" });
            DropTable("dbo.AteAlta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Alta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
