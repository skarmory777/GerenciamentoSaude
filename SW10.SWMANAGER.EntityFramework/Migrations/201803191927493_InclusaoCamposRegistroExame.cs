namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposRegistroExame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimento", "DataRegistro", c => c.DateTime(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsContraste", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteVenoso", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteOral", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "VolumeContrasteRetal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LauMovimentoItem", "IsIonico", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsBombaInsufora", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsContrasteVenoso", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsContrasteOral", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsContrasteRetal", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.LauMovimentoItem", "IsContrasteRetal");
            DropColumn("dbo.LauMovimentoItem", "IsContrasteOral");
            DropColumn("dbo.LauMovimentoItem", "IsContrasteVenoso");
            DropColumn("dbo.LauMovimentoItem", "IsBombaInsufora");
            DropColumn("dbo.LauMovimentoItem", "IsIonico");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteRetal");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteOral");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteVenoso");
            DropColumn("dbo.LauMovimentoItem", "VolumeContrasteTotal");
            DropColumn("dbo.LauMovimentoItem", "IsContraste");
            DropColumn("dbo.LauMovimento", "DataRegistro");
        }
    }
}
