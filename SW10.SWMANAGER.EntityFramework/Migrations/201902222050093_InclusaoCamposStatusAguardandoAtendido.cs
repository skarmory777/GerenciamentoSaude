namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposStatusAguardandoAtendido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "StatusAguardando", c => c.Int());
            AddColumn("dbo.AteAtendimento", "StatusAtendido", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "StatusAtendido");
            DropColumn("dbo.AteAtendimento", "StatusAguardando");
        }
    }
}
