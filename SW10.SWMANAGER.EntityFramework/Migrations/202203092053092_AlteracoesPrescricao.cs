namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracoesPrescricao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItem", "IsControleDosagem", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItem", "MinimoAceitavel", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.AssPrescricaoItem", "MaximoAceitavel", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.AssPrescricaoItem", "MinimoBloqueio", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.AssPrescricaoItem", "MaximoBloqueio", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.AssPrescricaoItemResposta", "JustificativaBloqueioDosagemAceitavel", c => c.String());
            AddColumn("dbo.AssPrescricaoItemResposta", "JustificativaBloqueioId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "AteLeitoId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoMedica", "AteLeitoId");
            AddForeignKey("dbo.AssPrescricaoMedica", "AteLeitoId", "dbo.AteLeito", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoMedica", "AteLeitoId", "dbo.AteLeito");
            DropIndex("dbo.AssPrescricaoMedica", new[] { "AteLeitoId" });
            DropColumn("dbo.AssPrescricaoMedica", "AteLeitoId");
            DropColumn("dbo.AssPrescricaoItemResposta", "JustificativaBloqueioId");
            DropColumn("dbo.AssPrescricaoItemResposta", "JustificativaBloqueioDosagemAceitavel");
            DropColumn("dbo.AssPrescricaoItem", "MaximoBloqueio");
            DropColumn("dbo.AssPrescricaoItem", "MinimoBloqueio");
            DropColumn("dbo.AssPrescricaoItem", "MaximoAceitavel");
            DropColumn("dbo.AssPrescricaoItem", "MinimoAceitavel");
            DropColumn("dbo.AssPrescricaoItem", "IsControleDosagem");
        }
    }
}
