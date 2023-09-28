namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SisFormConfig_ConfiguracaoSalvamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "Salvamento", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.SisFormColConfig", "Salvamento");
        }
    }
}
