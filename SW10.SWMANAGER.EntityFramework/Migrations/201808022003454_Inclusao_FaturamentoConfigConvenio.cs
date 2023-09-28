namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_FaturamentoConfigConvenio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatContaItem", "TabelaId", "dbo.FatTabela");
            DropIndex("dbo.FatContaItem", new[] { "TabelaId" });
            AddColumn("dbo.FatContaItem", "FaturamentoConfigConvenioId", c => c.Long());
            CreateIndex("dbo.FatContaItem", "FaturamentoConfigConvenioId");
            AddForeignKey("dbo.FatContaItem", "FaturamentoConfigConvenioId", "dbo.FatConfigConvenio", "Id");
            DropColumn("dbo.FatContaItem", "TabelaId");
        }

        public override void Down()
        {
            AddColumn("dbo.FatContaItem", "TabelaId", c => c.Long());
            DropForeignKey("dbo.FatContaItem", "FaturamentoConfigConvenioId", "dbo.FatConfigConvenio");
            DropIndex("dbo.FatContaItem", new[] { "FaturamentoConfigConvenioId" });
            DropColumn("dbo.FatContaItem", "FaturamentoConfigConvenioId");
            CreateIndex("dbo.FatContaItem", "TabelaId");
            AddForeignKey("dbo.FatContaItem", "TabelaId", "dbo.FatTabela", "Id");
        }
    }
}
