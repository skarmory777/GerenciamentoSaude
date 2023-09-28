namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTipos_Laudo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LauMovimento", "VolumeContrasteTotal", c => c.Int());
            AlterColumn("dbo.LauMovimento", "VolumeContrasteVenoso", c => c.Int());
            AlterColumn("dbo.LauMovimento", "VolumeContrasteOral", c => c.Int());
            AlterColumn("dbo.LauMovimento", "VolumeContrasteRetal", c => c.Int());
        }

        public override void Down()
        {
            AlterColumn("dbo.LauMovimento", "VolumeContrasteRetal", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LauMovimento", "VolumeContrasteOral", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LauMovimento", "VolumeContrasteVenoso", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LauMovimento", "VolumeContrasteTotal", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
