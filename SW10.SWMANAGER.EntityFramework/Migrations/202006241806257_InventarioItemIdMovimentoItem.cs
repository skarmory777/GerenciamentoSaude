namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventarioItemIdMovimentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoqueMovimentoItem", "InventarioItemId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimentoItem", "InventarioItemId");
            AddForeignKey("dbo.EstoqueMovimentoItem", "InventarioItemId", "dbo.EstInventarioItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstoqueMovimentoItem", "InventarioItemId", "dbo.EstInventarioItem");
            DropIndex("dbo.EstoqueMovimentoItem", new[] { "InventarioItemId" });
            DropColumn("dbo.EstoqueMovimentoItem", "InventarioItemId");
        }
    }
}
