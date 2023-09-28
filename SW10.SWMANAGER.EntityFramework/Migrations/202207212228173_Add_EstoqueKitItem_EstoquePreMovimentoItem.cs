namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_EstoqueKitItem_EstoquePreMovimentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimentoItem", "EstoqueKitItemId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimentoItem", "EstoqueKitItemId");
            AddForeignKey("dbo.EstoquePreMovimentoItem", "EstoqueKitItemId", "dbo.EstKitItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimentoItem", "EstoqueKitItemId", "dbo.EstKitItem");
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "EstoqueKitItemId" });
            DropColumn("dbo.EstoquePreMovimentoItem", "EstoqueKitItemId");
        }
    }
}
