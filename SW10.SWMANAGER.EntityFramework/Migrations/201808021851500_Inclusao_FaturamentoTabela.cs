namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_FaturamentoTabela : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "TabelaId", c => c.Long());
            CreateIndex("dbo.FatContaItem", "TabelaId");
            AddForeignKey("dbo.FatContaItem", "TabelaId", "dbo.FatTabela", "Id");
            DropColumn("dbo.FatContaItem", "CodigoTabela");
        }

        public override void Down()
        {
            AddColumn("dbo.FatContaItem", "CodigoTabela", c => c.String());
            DropForeignKey("dbo.FatContaItem", "TabelaId", "dbo.FatTabela");
            DropIndex("dbo.FatContaItem", new[] { "TabelaId" });
            DropColumn("dbo.FatContaItem", "TabelaId");
        }
    }
}
