namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposLaboratorio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoExameItem", "IsPendencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssSolicitacaoExameItem", "PendenciaUserId", c => c.Long());
            AddColumn("dbo.AssSolicitacaoExameItem", "PendenciaDateTime", c => c.DateTime());
            AddColumn("dbo.AssSolicitacaoExameItem", "MotivoPendencia", c => c.String());
            AddColumn("dbo.AssSolicitacaoExame", "PendenciaUserId", c => c.Long());
            AddColumn("dbo.AssSolicitacaoExame", "PendenciaDateTime", c => c.DateTime());
            AddColumn("dbo.AssSolicitacaoExame", "MotivoPendencia", c => c.String());
            AddColumn("dbo.LabResultado", "AssSolicitacaoExame", c => c.Long());
            AddColumn("dbo.LabResultado", "IsPendencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabResultado", "PendenciaUserId", c => c.Long());
            AddColumn("dbo.LabResultado", "PendenciaDateTime", c => c.DateTime());
            AddColumn("dbo.LabResultado", "MotivoPendencia", c => c.String());
            AddColumn("dbo.LabResultadoExame", "AssSolicitacaoExameItem", c => c.Long());
            AddColumn("dbo.LabResultadoExame", "IsPendencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabResultadoExame", "PendenciaUserId", c => c.Long());
            AddColumn("dbo.LabResultadoExame", "PendenciaDateTime", c => c.DateTime());
            AddColumn("dbo.LabResultadoExame", "MotivoPendencia", c => c.String());
            CreateIndex("dbo.LabResultado", "AssSolicitacaoExame");
            CreateIndex("dbo.LabResultadoExame", "AssSolicitacaoExameItem");
            AddForeignKey("dbo.LabResultado", "AssSolicitacaoExame", "dbo.AssSolicitacaoExame", "Id");
            AddForeignKey("dbo.LabResultadoExame", "AssSolicitacaoExameItem", "dbo.AssSolicitacaoExameItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "AssSolicitacaoExameItem", "dbo.AssSolicitacaoExameItem");
            DropForeignKey("dbo.LabResultado", "AssSolicitacaoExame", "dbo.AssSolicitacaoExame");
            DropIndex("dbo.LabResultadoExame", new[] { "AssSolicitacaoExameItem" });
            DropIndex("dbo.LabResultado", new[] { "AssSolicitacaoExame" });
            DropColumn("dbo.LabResultadoExame", "MotivoPendencia");
            DropColumn("dbo.LabResultadoExame", "PendenciaDateTime");
            DropColumn("dbo.LabResultadoExame", "PendenciaUserId");
            DropColumn("dbo.LabResultadoExame", "IsPendencia");
            DropColumn("dbo.LabResultadoExame", "AssSolicitacaoExameItem");
            DropColumn("dbo.LabResultado", "MotivoPendencia");
            DropColumn("dbo.LabResultado", "PendenciaDateTime");
            DropColumn("dbo.LabResultado", "PendenciaUserId");
            DropColumn("dbo.LabResultado", "IsPendencia");
            DropColumn("dbo.LabResultado", "AssSolicitacaoExame");
            DropColumn("dbo.AssSolicitacaoExame", "MotivoPendencia");
            DropColumn("dbo.AssSolicitacaoExame", "PendenciaDateTime");
            DropColumn("dbo.AssSolicitacaoExame", "PendenciaUserId");
            DropColumn("dbo.AssSolicitacaoExameItem", "MotivoPendencia");
            DropColumn("dbo.AssSolicitacaoExameItem", "PendenciaDateTime");
            DropColumn("dbo.AssSolicitacaoExameItem", "PendenciaUserId");
            DropColumn("dbo.AssSolicitacaoExameItem", "IsPendencia");
        }
    }
}
