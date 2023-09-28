namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class MudancasInventario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstStatusInventarioItem",
                c => new
                    {
                        Id = c.Long(nullable: false),
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
                    { "DynamicFilter_StatusInventarioItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EstInventarioItem", "StatusInventarioItemId", c => c.Long(nullable: false));
            AddColumn("dbo.EstInventarioItem", "TemDivergencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstInventarioItem", "UsuarioDivergenciaId", c => c.Long());
            AddColumn("dbo.EstInventarioItem", "DivergenciaResolvida", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstInventarioItem", "DataDivergenciaResolvida", c => c.DateTime());
            CreateIndex("dbo.EstInventarioItem", "StatusInventarioItemId");
            CreateIndex("dbo.EstInventarioItem", "UsuarioDivergenciaId");
            AddForeignKey("dbo.EstInventarioItem", "StatusInventarioItemId", "dbo.EstStatusInventarioItem", "Id", cascadeDelete: false);
            AddForeignKey("dbo.EstInventarioItem", "UsuarioDivergenciaId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstInventarioItem", "UsuarioDivergenciaId", "dbo.AbpUsers");
            DropForeignKey("dbo.EstInventarioItem", "StatusInventarioItemId", "dbo.EstStatusInventarioItem");
            DropIndex("dbo.EstInventarioItem", new[] { "UsuarioDivergenciaId" });
            DropIndex("dbo.EstInventarioItem", new[] { "StatusInventarioItemId" });
            DropColumn("dbo.EstInventarioItem", "DataDivergenciaResolvida");
            DropColumn("dbo.EstInventarioItem", "DivergenciaResolvida");
            DropColumn("dbo.EstInventarioItem", "UsuarioDivergenciaId");
            DropColumn("dbo.EstInventarioItem", "TemDivergencia");
            DropColumn("dbo.EstInventarioItem", "StatusInventarioItemId");
            DropTable("dbo.EstStatusInventarioItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_StatusInventarioItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
