namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class corrigir_classe_pessoa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sis_Pessoa", "Nascimento", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.Sis_Pessoa", "Nascimento", c => c.DateTime(nullable: false));
        }
    }
}
