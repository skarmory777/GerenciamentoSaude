namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Projeto_Conteudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisProjeto", "Conteudo", c => c.String());
            AddColumn("dbo.SisProjeto", "ProjetoId", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.SisProjeto", "ProjetoId");
            DropColumn("dbo.SisProjeto", "Conteudo");
        }
    }
}
