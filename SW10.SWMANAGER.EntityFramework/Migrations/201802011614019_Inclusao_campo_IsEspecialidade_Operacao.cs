namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_campo_IsEspecialidade_Operacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisOperacao", "IsEspecialidade", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.SisOperacao", "IsEspecialidade");
        }
    }
}
