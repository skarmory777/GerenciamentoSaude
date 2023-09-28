namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignkey_SolicitacaoExameItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimento", "SolicitacaoExameItemId", c => c.Long());
            CreateIndex("dbo.LauMovimento", "SolicitacaoExameItemId");
            AddForeignKey("dbo.LauMovimento", "SolicitacaoExameItemId", "dbo.AssSolicitacaoExameItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LauMovimento", "SolicitacaoExameItemId", "dbo.AssSolicitacaoExameItem");
            DropIndex("dbo.LauMovimento", new[] { "SolicitacaoExameItemId" });
            DropColumn("dbo.LauMovimento", "SolicitacaoExameItemId");
        }
    }
}
