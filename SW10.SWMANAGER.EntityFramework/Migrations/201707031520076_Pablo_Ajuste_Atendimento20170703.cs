namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Pablo_Ajuste_Atendimento20170703 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Atendimento", "DataPreatendimento", c => c.DateTime());
            AlterColumn("dbo.Atendimento", "DataPrevistaAtendimento", c => c.DateTime());
            AlterColumn("dbo.Atendimento", "DataAlta", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.Atendimento", "DataAlta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Atendimento", "DataPrevistaAtendimento", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Atendimento", "DataPreatendimento", c => c.DateTime(nullable: false));
        }
    }
}
