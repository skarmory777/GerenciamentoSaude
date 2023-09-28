namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_ResultadoLaudo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LabResultadoLaudo", "MinimoAceitavelMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoAceitavelMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "NormalMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoAceitavelFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoAceitavelFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "NormalFeminino", c => c.Decimal(precision: 18, scale: 2));
        }

        public override void Down()
        {
            AlterColumn("dbo.LabResultadoLaudo", "NormalFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "NormalMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MaximoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabResultadoLaudo", "MinimoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
