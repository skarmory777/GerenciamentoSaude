namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criacao_PrescricaoStatus_PrescricaoItemStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssPrescricaoItemStatus",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Cor = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_PrescricaoItemStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssPrescricaoStatus",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Cor = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_PrescricaoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "AssPrescricaoStatusId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId");
            CreateIndex("dbo.AssPrescricaoMedica", "AssPrescricaoStatusId");
            AddForeignKey("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId", "dbo.AssPrescricaoItemStatus", "Id");
            AddForeignKey("dbo.AssPrescricaoMedica", "AssPrescricaoStatusId", "dbo.AssPrescricaoStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoMedica", "AssPrescricaoStatusId", "dbo.AssPrescricaoStatus");
            DropForeignKey("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId", "dbo.AssPrescricaoItemStatus");
            DropIndex("dbo.AssPrescricaoMedica", new[] { "AssPrescricaoStatusId" });
            DropIndex("dbo.AssPrescricaoItem", new[] { "AssPrescricaoItemStatusId" });
            DropColumn("dbo.AssPrescricaoMedica", "AssPrescricaoStatusId");
            DropColumn("dbo.AssPrescricaoItem", "AssPrescricaoItemStatusId");
            DropTable("dbo.AssPrescricaoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrescricaoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssPrescricaoItemStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrescricaoItemStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
