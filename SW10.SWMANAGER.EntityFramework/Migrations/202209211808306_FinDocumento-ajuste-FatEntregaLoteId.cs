namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinDocumentoajusteFatEntregaLoteId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FinDocumento", name: "FatEntregaoteId", newName: "FatEntregaLoteId");
            RenameIndex(table: "dbo.FinDocumento", name: "IX_FatEntregaoteId", newName: "IX_FatEntregaLoteId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.FinDocumento", name: "IX_FatEntregaLoteId", newName: "IX_FatEntregaoteId");
            RenameColumn(table: "dbo.FinDocumento", name: "FatEntregaLoteId", newName: "FatEntregaoteId");
        }
    }
}
