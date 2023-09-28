namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoRelacionamentoDocumentoAnexo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinDocumento", "AnexoListaId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinDocumento", "AnexoListaId");
        }
    }
}
