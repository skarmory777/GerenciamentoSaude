namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class campos_Atendimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "ValidadeCarteira", c => c.DateTime());
            AddColumn("dbo.AteAtendimento", "ValidadeSenha", c => c.DateTime());
            AddColumn("dbo.AteAtendimento", "DataAutorizacao", c => c.DateTime());
            AddColumn("dbo.AteAtendimento", "Senha", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "Senha");
            DropColumn("dbo.AteAtendimento", "DataAutorizacao");
            DropColumn("dbo.AteAtendimento", "ValidadeSenha");
            DropColumn("dbo.AteAtendimento", "ValidadeCarteira");
        }
    }
}
