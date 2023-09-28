namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTabelaResultadoLaudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoLaudo", "Ordem", c => c.Int());
            AddColumn("dbo.LabResultadoLaudo", "OrdemRegistro", c => c.Int());
            AddColumn("dbo.LabResultadoLaudo", "MinimoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MaximoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MinimoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MaximoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "NormalMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MinimoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MaximoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MinimoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "MaximoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "NormalFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabResultadoLaudo", "TextoImpresso", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.LabResultadoLaudo", "TextoImpresso");
            DropColumn("dbo.LabResultadoLaudo", "NormalFeminino");
            DropColumn("dbo.LabResultadoLaudo", "MaximoFeminino");
            DropColumn("dbo.LabResultadoLaudo", "MinimoFeminino");
            DropColumn("dbo.LabResultadoLaudo", "MaximoAceitavelFeminino");
            DropColumn("dbo.LabResultadoLaudo", "MinimoAceitavelFeminino");
            DropColumn("dbo.LabResultadoLaudo", "NormalMasculino");
            DropColumn("dbo.LabResultadoLaudo", "MaximoMasculino");
            DropColumn("dbo.LabResultadoLaudo", "MinimoMasculino");
            DropColumn("dbo.LabResultadoLaudo", "MaximoAceitavelMasculino");
            DropColumn("dbo.LabResultadoLaudo", "MinimoAceitavelMasculino");
            DropColumn("dbo.LabResultadoLaudo", "OrdemRegistro");
            DropColumn("dbo.LabResultadoLaudo", "Ordem");
        }
    }
}
