namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atualizando_FinDocumento_PreMovimentoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinDocumento", "PreMovimentoId", c => c.Long());
            CreateIndex("dbo.FinDocumento", "PreMovimentoId");
            AddForeignKey("dbo.FinDocumento", "PreMovimentoId", "dbo.EstoquePreMovimento", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinDocumento", "PreMovimentoId", "dbo.EstoquePreMovimento");
            DropIndex("dbo.FinDocumento", new[] { "PreMovimentoId" });
            DropColumn("dbo.FinDocumento", "PreMovimentoId");
        }
    }
}
