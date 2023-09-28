namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class TarefaConteudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisTarefa", "Conteudo", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisTarefa", "Conteudo");
        }
    }
}
