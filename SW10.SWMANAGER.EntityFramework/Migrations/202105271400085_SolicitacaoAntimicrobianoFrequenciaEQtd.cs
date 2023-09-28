namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolicitacaoAntimicrobianoFrequenciaEQtd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoAntimicrobianos", "AssFrequenciaId", c => c.Long());
            AddColumn("dbo.AssSolicitacaoAntimicrobianos", "Qtd", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "AssFrequenciaId");
            AddForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssFrequenciaId", "dbo.AssFrequencia", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssFrequenciaId", "dbo.AssFrequencia");
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "AssFrequenciaId" });
            DropColumn("dbo.AssSolicitacaoAntimicrobianos", "Qtd");
            DropColumn("dbo.AssSolicitacaoAntimicrobianos", "AssFrequenciaId");
        }
    }
}
