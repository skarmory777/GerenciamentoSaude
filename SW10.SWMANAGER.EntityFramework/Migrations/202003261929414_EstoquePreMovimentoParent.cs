namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstoquePreMovimentoParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "EstoquePreMovimentoParentId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "EstoquePreMovimentoParentId");
            AddForeignKey("dbo.EstoquePreMovimento", "EstoquePreMovimentoParentId", "dbo.EstoquePreMovimento", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstoquePreMovimentoParentId", "dbo.EstoquePreMovimento");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoquePreMovimentoParentId" });
            DropColumn("dbo.EstoquePreMovimento", "EstoquePreMovimentoParentId");
        }
    }
}
