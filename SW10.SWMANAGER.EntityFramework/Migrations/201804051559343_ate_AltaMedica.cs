namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class ate_AltaMedica : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAltaMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Data = c.DateTime(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_AltaMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssMotivoAlta", t => t.AteMotivoAltaId)
                .Index(t => t.AteMotivoAltaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAltaMedica", "AteMotivoAltaId", "dbo.AssMotivoAlta");
            DropIndex("dbo.AteAltaMedica", new[] { "AteMotivoAltaId" });
            DropTable("dbo.AteAltaMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AltaMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
