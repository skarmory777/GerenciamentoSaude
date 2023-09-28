namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizarEstoqueKitItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstKitItem", "UnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.EstKitItem", new[] { "UnidadeId" });
            AlterColumn("dbo.EstKitItem", "UnidadeId", c => c.Long());
            CreateIndex("dbo.EstKitItem", "UnidadeId");
            AddForeignKey("dbo.EstKitItem", "UnidadeId", "dbo.Est_Unidade", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstKitItem", "UnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.EstKitItem", new[] { "UnidadeId" });
            AlterColumn("dbo.EstKitItem", "UnidadeId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstKitItem", "UnidadeId");
            AddForeignKey("dbo.EstKitItem", "UnidadeId", "dbo.Est_Unidade", "Id", cascadeDelete: true);
        }
    }
}
