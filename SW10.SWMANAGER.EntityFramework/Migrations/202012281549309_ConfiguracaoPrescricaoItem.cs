namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ConfiguracaoPrescricaoItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssConfiguracaoPrescricaoItemCampo",
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
                    { "DynamicFilter_ConfiguracaoPrescricaoItemCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssConfiguracaoPrescricaoItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConfiguracaoPrescricaoItemCampoId = c.Long(nullable: false),
                        PrescricaoItemId = c.Long(),
                        DivisaoId = c.Long(),
                        IsBlock = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        DefaultValue = c.String(),
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
                    { "DynamicFilter_ConfiguracaoPrescricaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssConfiguracaoPrescricaoItemCampo", t => t.ConfiguracaoPrescricaoItemCampoId, cascadeDelete: true)
                .ForeignKey("dbo.AssDivisao", t => t.DivisaoId)
                .ForeignKey("dbo.AssPrescricaoItem", t => t.PrescricaoItemId)
                .Index(t => t.ConfiguracaoPrescricaoItemCampoId)
                .Index(t => t.PrescricaoItemId)
                .Index(t => t.DivisaoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssConfiguracaoPrescricaoItem", "PrescricaoItemId", "dbo.AssPrescricaoItem");
            DropForeignKey("dbo.AssConfiguracaoPrescricaoItem", "DivisaoId", "dbo.AssDivisao");
            DropForeignKey("dbo.AssConfiguracaoPrescricaoItem", "ConfiguracaoPrescricaoItemCampoId", "dbo.AssConfiguracaoPrescricaoItemCampo");
            DropIndex("dbo.AssConfiguracaoPrescricaoItem", new[] { "DivisaoId" });
            DropIndex("dbo.AssConfiguracaoPrescricaoItem", new[] { "PrescricaoItemId" });
            DropIndex("dbo.AssConfiguracaoPrescricaoItem", new[] { "ConfiguracaoPrescricaoItemCampoId" });
            DropTable("dbo.AssConfiguracaoPrescricaoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConfiguracaoPrescricaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssConfiguracaoPrescricaoItemCampo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConfiguracaoPrescricaoItemCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
