namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ColunaSizeFormularioDinamico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "Size", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisFormColConfig", "Size");
        }
    }
}
