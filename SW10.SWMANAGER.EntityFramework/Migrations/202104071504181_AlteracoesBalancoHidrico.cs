namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracoesBalancoHidrico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteBalancoHidricos", "ConferidoManha", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteBalancoHidricos", "ConferidoManhaUserId", c => c.Long());
            AddColumn("dbo.AteBalancoHidricos", "DtConferidoManha", c => c.DateTime());
            AddColumn("dbo.AteBalancoHidricos", "ConferidoNoite", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteBalancoHidricos", "ConferidoNoiteUserId", c => c.Long());
            AddColumn("dbo.AteBalancoHidricos", "DtConferidoNoite", c => c.DateTime());
            AddColumn("dbo.AteBalancoHidricos", "ConferidoTotal", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteBalancoHidricos", "ConferidoTotalUserId", c => c.Long());
            AddColumn("dbo.AteBalancoHidricos", "DtConferidoTotal", c => c.DateTime());
            AddColumn("dbo.AteBalancoHidricos", "Evacuacoes", c => c.Boolean());
            AddColumn("dbo.AteBalancoHidricos", "Aspecto", c => c.String());
            AddColumn("dbo.AteBalancoHidricoItens", "IrrigacaodeEntrada", c => c.String());
            AddColumn("dbo.AteBalancoHidricoItens", "IrrigacaodeSaida", c => c.String());
            AddColumn("dbo.AteBalancoHidricoSinaisVitais", "PressaoIntracraniana", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteBalancoHidricoSinaisVitais", "PressaoIntracraniana");
            DropColumn("dbo.AteBalancoHidricoItens", "IrrigacaodeSaida");
            DropColumn("dbo.AteBalancoHidricoItens", "IrrigacaodeEntrada");
            DropColumn("dbo.AteBalancoHidricos", "Aspecto");
            DropColumn("dbo.AteBalancoHidricos", "Evacuacoes");
            DropColumn("dbo.AteBalancoHidricos", "DtConferidoTotal");
            DropColumn("dbo.AteBalancoHidricos", "ConferidoTotalUserId");
            DropColumn("dbo.AteBalancoHidricos", "ConferidoTotal");
            DropColumn("dbo.AteBalancoHidricos", "DtConferidoNoite");
            DropColumn("dbo.AteBalancoHidricos", "ConferidoNoiteUserId");
            DropColumn("dbo.AteBalancoHidricos", "ConferidoNoite");
            DropColumn("dbo.AteBalancoHidricos", "DtConferidoManha");
            DropColumn("dbo.AteBalancoHidricos", "ConferidoManhaUserId");
            DropColumn("dbo.AteBalancoHidricos", "ConferidoManha");
        }
    }
}
