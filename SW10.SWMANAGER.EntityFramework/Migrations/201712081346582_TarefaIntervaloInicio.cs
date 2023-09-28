namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class TarefaIntervaloInicio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisTarefa", "IntervaloInicio", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.SisTarefa", "IntervaloInicio");
        }
    }
}
