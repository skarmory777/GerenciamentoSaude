namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_chave : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "Chave", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstoquePreMovimento", "Chave");
        }
    }
}
