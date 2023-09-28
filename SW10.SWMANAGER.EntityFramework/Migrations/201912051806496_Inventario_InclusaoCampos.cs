namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventario_InclusaoCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "InventarioId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimentoItem", "InventarioItemId", c => c.Long());
            AddColumn("dbo.EstInventario", "GrupoId", c => c.Long());
            AddColumn("dbo.EstInventario", "GrupoClasseId", c => c.Long());
            AddColumn("dbo.EstInventario", "GrupoSubClasseId", c => c.Long());
            AddColumn("dbo.EstInventario", "ProdutoId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "InventarioId");
            CreateIndex("dbo.EstInventario", "GrupoId");
            CreateIndex("dbo.EstInventario", "GrupoClasseId");
            CreateIndex("dbo.EstInventario", "GrupoSubClasseId");
            CreateIndex("dbo.EstInventario", "ProdutoId");
            CreateIndex("dbo.EstoquePreMovimentoItem", "InventarioItemId");
            AddForeignKey("dbo.EstInventario", "GrupoId", "dbo.Est_Grupo", "Id");
            AddForeignKey("dbo.EstInventario", "GrupoClasseId", "dbo.Est_GrupoClasse", "Id");
            AddForeignKey("dbo.EstInventario", "GrupoSubClasseId", "dbo.Est_GrupoSubClasse", "Id");
            AddForeignKey("dbo.EstInventario", "ProdutoId", "dbo.Est_Produto", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "InventarioId", "dbo.EstInventario", "Id");
            AddForeignKey("dbo.EstoquePreMovimentoItem", "InventarioItemId", "dbo.EstInventarioItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimentoItem", "InventarioItemId", "dbo.EstInventarioItem");
            DropForeignKey("dbo.EstoquePreMovimento", "InventarioId", "dbo.EstInventario");
            DropForeignKey("dbo.EstInventario", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstInventario", "GrupoSubClasseId", "dbo.Est_GrupoSubClasse");
            DropForeignKey("dbo.EstInventario", "GrupoClasseId", "dbo.Est_GrupoClasse");
            DropForeignKey("dbo.EstInventario", "GrupoId", "dbo.Est_Grupo");
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "InventarioItemId" });
            DropIndex("dbo.EstInventario", new[] { "ProdutoId" });
            DropIndex("dbo.EstInventario", new[] { "GrupoSubClasseId" });
            DropIndex("dbo.EstInventario", new[] { "GrupoClasseId" });
            DropIndex("dbo.EstInventario", new[] { "GrupoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "InventarioId" });
            DropColumn("dbo.EstInventario", "ProdutoId");
            DropColumn("dbo.EstInventario", "GrupoSubClasseId");
            DropColumn("dbo.EstInventario", "GrupoClasseId");
            DropColumn("dbo.EstInventario", "GrupoId");
            DropColumn("dbo.EstoquePreMovimentoItem", "InventarioItemId");
            DropColumn("dbo.EstoquePreMovimento", "InventarioId");
        }
    }
}
