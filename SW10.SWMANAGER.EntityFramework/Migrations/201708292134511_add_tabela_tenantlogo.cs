namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class add_tabela_tenantlogo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenantLogo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Logotipo = c.Binary(),
                    LogotipoMimeType = c.String(),
                    TenantId = c.Long(),
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
                    { "DynamicFilter_TenantLogo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.TenantLogo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantLogo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
