namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_ComentarioAutorizacaoProcedimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAutorizacaoProcedimento", "NumeroGuia", c => c.String());
            DropColumn("dbo.AteAutorizacaoProcedimento", "NumerioGuia");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAutorizacaoProcedimento", "NumerioGuia", c => c.String());
            DropColumn("dbo.AteAutorizacaoProcedimento", "NumeroGuia");
        }
    }
}
