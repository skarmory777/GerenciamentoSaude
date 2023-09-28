namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class VelocidadeInfusaoFormaAplicacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssVelocidadeInfusaoFormaAplicacao",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VelocidadeInfusaoId = c.Long(nullable: false),
                        FormaApplicacaoId = c.Long(nullable: false),
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
                    { "DynamicFilter_VelocidadeInfusaoFormaAplicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssFormaAplicacao", t => t.FormaApplicacaoId, cascadeDelete: true)
                .ForeignKey("dbo.AssVelocidadeInfusao", t => t.VelocidadeInfusaoId, cascadeDelete: true)
                .Index(t => t.VelocidadeInfusaoId)
                .Index(t => t.FormaApplicacaoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssVelocidadeInfusaoFormaAplicacao", "VelocidadeInfusaoId", "dbo.AssVelocidadeInfusao");
            DropForeignKey("dbo.AssVelocidadeInfusaoFormaAplicacao", "FormaApplicacaoId", "dbo.AssFormaAplicacao");
            DropIndex("dbo.AssVelocidadeInfusaoFormaAplicacao", new[] { "FormaApplicacaoId" });
            DropIndex("dbo.AssVelocidadeInfusaoFormaAplicacao", new[] { "VelocidadeInfusaoId" });
            DropTable("dbo.AssVelocidadeInfusaoFormaAplicacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VelocidadeInfusaoFormaAplicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
