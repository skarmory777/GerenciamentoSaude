namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Add_campo_intervalo_classe_Frequencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssFrequencia", "Intervalo", c => c.Long(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AssFrequencia", "Intervalo");
        }
    }
}
