namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_RegistroTabela : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisRegistroArquivo", "RegistroTabelaId", "dbo.SisRegistroTabela");
            DropPrimaryKey("dbo.SisRegistroTabela");
            AlterColumn("dbo.SisRegistroTabela", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.SisRegistroTabela", "Id");
            AddForeignKey("dbo.SisRegistroArquivo", "RegistroTabelaId", "dbo.SisRegistroTabela", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisRegistroArquivo", "RegistroTabelaId", "dbo.SisRegistroTabela");
            DropPrimaryKey("dbo.SisRegistroTabela");
            AlterColumn("dbo.SisRegistroTabela", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.SisRegistroTabela", "Id");
            AddForeignKey("dbo.SisRegistroArquivo", "RegistroTabelaId", "dbo.SisRegistroTabela", "Id");
        }
    }
}
