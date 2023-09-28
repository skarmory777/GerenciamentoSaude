namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Correcao_nome_tabela_AteSenhaMovimentacao : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AtSenhaMov", newName: "AteSenhaMov");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.AteSenhaMov", newName: "AtSenhaMov");
        }
    }
}
