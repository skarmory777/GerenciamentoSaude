namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ParametrizacaoControleDeIp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisParametrizacoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsHabilitaControleDeIp = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Parametrizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SisParametrizacaoIps",
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
                    { "DynamicFilter_ParametrizacaoIp_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AbpUsers", "IsHabilitaControleDeIp", c => c.Boolean(nullable: false));
            AddColumn("dbo.AbpRoles", "IsHabilitaControleDeIp", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpRoles", "IsHabilitaControleDeIp");
            DropColumn("dbo.AbpUsers", "IsHabilitaControleDeIp");
            DropTable("dbo.SisParametrizacaoIps",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ParametrizacaoIp_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisParametrizacoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Parametrizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
