namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposDeItemResultadoNullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LabItemResultado", "MinimoAceitavelMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoAceitavelMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "NormalMasculino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoAceitavelFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoAceitavelFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoFeminino", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "NormalFeminino", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LabItemResultado", "NormalFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoAceitavelFeminino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "NormalMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MaximoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LabItemResultado", "MinimoAceitavelMasculino", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
