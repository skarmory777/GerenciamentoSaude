namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Pablo_Atendimento_20170630 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Atendimento", "QtdSessoes", c => c.Int());
            AlterColumn("dbo.Atendimento", "DataRetorno", c => c.DateTime());
            AlterColumn("dbo.Atendimento", "DataRevisao", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.Atendimento", "DataRevisao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Atendimento", "DataRetorno", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Atendimento", "QtdSessoes", c => c.Int(nullable: false));
        }
    }
}
