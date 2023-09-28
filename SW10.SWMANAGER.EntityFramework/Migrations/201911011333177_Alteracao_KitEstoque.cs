namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracao_KitEstoque : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstEtiqueta",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProdutoId = c.Long(),
                        LoteValidadeId = c.Long(),
                        UnidadeProdutoId = c.Long(),
                        EstoqueKitId = c.Long(),
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
                    { "DynamicFilter_EstoqueEtiqueta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstKit", t => t.EstoqueKitId)
                .ForeignKey("dbo.LoteValidade", t => t.LoteValidadeId)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId)
                .ForeignKey("dbo.Est_Unidade", t => t.UnidadeProdutoId)
                .Index(t => t.ProdutoId)
                .Index(t => t.LoteValidadeId)
                .Index(t => t.UnidadeProdutoId)
                .Index(t => t.EstoqueKitId);
            
            CreateTable(
                "dbo.EstPreMovimentoItemKitLoteValidade",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EstoquePreMovimentoItemKitId = c.Long(nullable: false),
                        LoteValidadeId = c.Long(nullable: false),
                        Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_EstoquePreMovimentoItemKitLoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstPreMovimentoItemKit", t => t.EstoquePreMovimentoItemKitId, cascadeDelete: true)
                .ForeignKey("dbo.LoteValidade", t => t.LoteValidadeId, cascadeDelete: true)
                .Index(t => t.EstoquePreMovimentoItemKitId)
                .Index(t => t.LoteValidadeId);
            
            CreateTable(
                "dbo.EstPreMovimentoItemKit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EstoquePreMovimentoItemId = c.Long(nullable: false),
                        EstoqueKitItemId = c.Long(nullable: false),
                        NumeroSerie = c.String(),
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
                    { "DynamicFilter_EstoquePreMovimentoItemKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstKitItem", t => t.EstoqueKitItemId, cascadeDelete: true)
                .ForeignKey("dbo.EstoquePreMovimentoItem", t => t.EstoquePreMovimentoItemId, cascadeDelete: true)
                .Index(t => t.EstoquePreMovimentoItemId)
                .Index(t => t.EstoqueKitItemId);
            
            AddColumn("dbo.EstoquePreMovimento", "Chave", c => c.String());
            AddColumn("dbo.EstKit", "ProdutoId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstKit", "ProdutoId");
            AddForeignKey("dbo.EstKit", "ProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstPreMovimentoItemKitLoteValidade", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.EstPreMovimentoItemKitLoteValidade", "EstoquePreMovimentoItemKitId", "dbo.EstPreMovimentoItemKit");
            DropForeignKey("dbo.EstPreMovimentoItemKit", "EstoquePreMovimentoItemId", "dbo.EstoquePreMovimentoItem");
            DropForeignKey("dbo.EstPreMovimentoItemKit", "EstoqueKitItemId", "dbo.EstKitItem");
            DropForeignKey("dbo.EstEtiqueta", "UnidadeProdutoId", "dbo.Est_Unidade");
            DropForeignKey("dbo.EstEtiqueta", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstEtiqueta", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.EstEtiqueta", "EstoqueKitId", "dbo.EstKit");
            DropForeignKey("dbo.EstKit", "ProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.EstPreMovimentoItemKit", new[] { "EstoqueKitItemId" });
            DropIndex("dbo.EstPreMovimentoItemKit", new[] { "EstoquePreMovimentoItemId" });
            DropIndex("dbo.EstPreMovimentoItemKitLoteValidade", new[] { "LoteValidadeId" });
            DropIndex("dbo.EstPreMovimentoItemKitLoteValidade", new[] { "EstoquePreMovimentoItemKitId" });
            DropIndex("dbo.EstEtiqueta", new[] { "EstoqueKitId" });
            DropIndex("dbo.EstEtiqueta", new[] { "UnidadeProdutoId" });
            DropIndex("dbo.EstEtiqueta", new[] { "LoteValidadeId" });
            DropIndex("dbo.EstEtiqueta", new[] { "ProdutoId" });
            DropIndex("dbo.EstKit", new[] { "ProdutoId" });
            DropColumn("dbo.EstKit", "ProdutoId");
            DropColumn("dbo.EstoquePreMovimento", "Chave");
            DropTable("dbo.EstPreMovimentoItemKit",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoItemKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstPreMovimentoItemKitLoteValidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoItemKitLoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstEtiqueta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueEtiqueta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
