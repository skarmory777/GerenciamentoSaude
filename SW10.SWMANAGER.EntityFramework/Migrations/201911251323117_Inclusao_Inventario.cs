namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_Inventario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstInventario",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DataInventario = c.DateTime(nullable: false),
                        TipoInventarioId = c.Long(nullable: false),
                        StatusInventarioId = c.Long(nullable: false),
                        EstoqueId = c.Long(nullable: false),
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
                    { "DynamicFilter_Inventario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Estoque", t => t.EstoqueId, cascadeDelete: true)
                .ForeignKey("dbo.EstStatusInventario", t => t.StatusInventarioId, cascadeDelete: true)
                .ForeignKey("dbo.EstTipoInventario", t => t.TipoInventarioId, cascadeDelete: true)
                .Index(t => t.TipoInventarioId)
                .Index(t => t.StatusInventarioId)
                .Index(t => t.EstoqueId);
            
            CreateTable(
                "dbo.EstStatusInventario",
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
                    { "DynamicFilter_StatusInventario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EstTipoInventario",
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
                    { "DynamicFilter_TipoInventario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EstInventarioItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InventarioId = c.Long(nullable: false),
                        ProdutoId = c.Long(nullable: false),
                        LoteValidadeId = c.Long(),
                        NumeroSerie = c.String(),
                        QuantidadeEstoque = c.Decimal(precision: 18, scale: 2),
                        QuantidadeContagem = c.Decimal(precision: 18, scale: 2),
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
                    { "DynamicFilter_InventarioItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstInventario", t => t.InventarioId, cascadeDelete: true)
                .ForeignKey("dbo.LoteValidade", t => t.LoteValidadeId)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: true)
                .Index(t => t.InventarioId)
                .Index(t => t.ProdutoId)
                .Index(t => t.LoteValidadeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstInventarioItem", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstInventarioItem", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.EstInventarioItem", "InventarioId", "dbo.EstInventario");
            DropForeignKey("dbo.EstInventario", "TipoInventarioId", "dbo.EstTipoInventario");
            DropForeignKey("dbo.EstInventario", "StatusInventarioId", "dbo.EstStatusInventario");
            DropForeignKey("dbo.EstInventario", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.EstInventarioItem", new[] { "LoteValidadeId" });
            DropIndex("dbo.EstInventarioItem", new[] { "ProdutoId" });
            DropIndex("dbo.EstInventarioItem", new[] { "InventarioId" });
            DropIndex("dbo.EstInventario", new[] { "EstoqueId" });
            DropIndex("dbo.EstInventario", new[] { "StatusInventarioId" });
            DropIndex("dbo.EstInventario", new[] { "TipoInventarioId" });
            DropTable("dbo.EstInventarioItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_InventarioItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstTipoInventario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoInventario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstStatusInventario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_StatusInventario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstInventario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Inventario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
