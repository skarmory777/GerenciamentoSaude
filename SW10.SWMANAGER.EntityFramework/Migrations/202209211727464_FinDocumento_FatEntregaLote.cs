namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinDocumento_FatEntregaLote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinDocumento", "FatEntregaoteId", c => c.Long());
            CreateIndex("dbo.FinDocumento", "FatEntregaoteId");
            AddForeignKey("dbo.FinDocumento", "FatEntregaoteId", "dbo.FatEntregaLote", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinDocumento", "FatEntregaoteId", "dbo.FatEntregaLote");
            DropIndex("dbo.FinDocumento", new[] { "FatEntregaoteId" });
            DropColumn("dbo.FinDocumento", "FatEntregaoteId");
        }
    }
}
