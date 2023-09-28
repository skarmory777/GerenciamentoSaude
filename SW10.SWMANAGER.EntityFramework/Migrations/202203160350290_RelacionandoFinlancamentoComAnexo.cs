namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelacionandoFinlancamentoComAnexo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinLancamento", "AnexoListaId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinLancamento", "AnexoListaId");
        }
    }
}
