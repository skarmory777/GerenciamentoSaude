namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesCamposLaboratorio : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LabResultado", name: "AssSolicitacaoExame", newName: "SolicitacaoExameId");
            RenameColumn(table: "dbo.LabResultadoExame", name: "AssSolicitacaoExameItem", newName: "SolicitacaoExameItemId");
            RenameIndex(table: "dbo.LabResultado", name: "IX_AssSolicitacaoExame", newName: "IX_SolicitacaoExameId");
            RenameIndex(table: "dbo.LabResultadoExame", name: "IX_AssSolicitacaoExameItem", newName: "IX_SolicitacaoExameItemId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.LabResultadoExame", name: "IX_SolicitacaoExameItemId", newName: "IX_AssSolicitacaoExameItem");
            RenameIndex(table: "dbo.LabResultado", name: "IX_SolicitacaoExameId", newName: "IX_AssSolicitacaoExame");
            RenameColumn(table: "dbo.LabResultadoExame", name: "SolicitacaoExameItemId", newName: "AssSolicitacaoExameItem");
            RenameColumn(table: "dbo.LabResultado", name: "SolicitacaoExameId", newName: "AssSolicitacaoExame");
        }
    }
}
