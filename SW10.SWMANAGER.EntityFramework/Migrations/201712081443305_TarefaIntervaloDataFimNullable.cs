namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class TarefaIntervaloDataFimNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisTarefaIntervalo", "Fim", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.SisTarefaIntervalo", "Fim", c => c.DateTime(nullable: false));
        }
    }
}
