namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_EstoqueKitItem_EstoqueSolicitacaoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstSolicitacaoItem", "EstoqueKitItemId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstSolicitacaoItem", "EstoqueKitItemId");
        }
    }
}
