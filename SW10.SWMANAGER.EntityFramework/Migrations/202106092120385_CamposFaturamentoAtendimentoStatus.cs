namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CamposFaturamentoAtendimentoStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatAtendimentoStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Cor = c.String(),
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
                    { "DynamicFilter_FaturamentoAtendimentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AteAtendimento", "FatAtendimentoStatusId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "FatAtendimentoStatusId");
            AddForeignKey("dbo.AteAtendimento", "FatAtendimentoStatusId", "dbo.FatAtendimentoStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "FatAtendimentoStatusId", "dbo.FatAtendimentoStatus");
            DropIndex("dbo.AteAtendimento", new[] { "FatAtendimentoStatusId" });
            DropColumn("dbo.AteAtendimento", "FatAtendimentoStatusId");
            DropTable("dbo.FatAtendimentoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoAtendimentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
