namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SisFormConfig_ConfiguracaoSalvamento_Bool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "SalvarTodos", c => c.Boolean());
            DropColumn("dbo.SisFormColConfig", "Salvamento");
        }

        public override void Down()
        {
            AddColumn("dbo.SisFormColConfig", "Salvamento", c => c.Int());
            DropColumn("dbo.SisFormColConfig", "SalvarTodos");
        }
    }
}
