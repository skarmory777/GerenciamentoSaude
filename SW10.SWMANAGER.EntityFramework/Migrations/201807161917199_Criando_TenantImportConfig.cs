namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criando_TenantImportConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenantImportConfigs",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TenantId = c.Long(),
                    ConnectionStringNameSw = c.String(),
                    ConnectionStringNameAsa = c.String(),
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
                    { "DynamicFilter_TenantImportConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.TenantImportConfigs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantImportConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
