namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaCamposResultadoLaudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabItemResultado", "MinimoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MaximoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MinimoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MaximoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "NormalMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MinimoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MaximoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MinimoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LabItemResultado", "MaximoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "NormalFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.LabItemResultado", "ErroMinimo");
            DropColumn("dbo.LabItemResultado", "ErroMaximo");
            DropColumn("dbo.LabItemResultado", "AlteradoMinimo");
            DropColumn("dbo.LabItemResultado", "AlteradoMaximo");
            DropColumn("dbo.LabItemResultado", "AceitavelMinimo");
            DropColumn("dbo.LabItemResultado", "AceitavelMaximo");
            DropColumn("dbo.LabItemResultado", "Normal");
            DropColumn("dbo.LabItemResultado", "ErroMinimoFeminino");
            DropColumn("dbo.LabItemResultado", "AlteradoMinimoFeminino");
            DropColumn("dbo.LabItemResultado", "AceitavelMaximoFeminino");
            DropColumn("dbo.LabItemResultado", "ErroMaximoFeminino");
            DropColumn("dbo.LabResultadoLaudo", "TextoImpresso");
        }

        public override void Down()
        {
            AddColumn("dbo.LabResultadoLaudo", "TextoImpresso", c => c.String());
            AddColumn("dbo.LabItemResultado", "ErroMaximoFeminino", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "AceitavelMaximoFeminino", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "AlteradoMinimoFeminino", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "ErroMinimoFeminino", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "Normal", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "AceitavelMaximo", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "AceitavelMinimo", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "AlteradoMaximo", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "AlteradoMinimo", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "ErroMaximo", c => c.Double(nullable: false));
            AddColumn("dbo.LabItemResultado", "ErroMinimo", c => c.Double(nullable: false));
            AlterColumn("dbo.LabItemResultado", "NormalFeminino", c => c.Double(nullable: false));
            DropColumn("dbo.LabItemResultado", "MaximoFeminino");
            DropColumn("dbo.LabItemResultado", "MinimoFeminino");
            DropColumn("dbo.LabItemResultado", "MaximoAceitavelFeminino");
            DropColumn("dbo.LabItemResultado", "MinimoAceitavelFeminino");
            DropColumn("dbo.LabItemResultado", "NormalMasculino");
            DropColumn("dbo.LabItemResultado", "MaximoMasculino");
            DropColumn("dbo.LabItemResultado", "MinimoMasculino");
            DropColumn("dbo.LabItemResultado", "MaximoAceitavelMasculino");
            DropColumn("dbo.LabItemResultado", "MinimoAceitavelMasculino");
        }
    }
}
