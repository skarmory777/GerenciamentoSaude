namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoLaudoMovimentoItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LauMovimento", "FaturamentocontaItemId", "dbo.FatContaItem");
            DropIndex("dbo.LauMovimento", new[] { "FaturamentocontaItemId" });
            AddColumn("dbo.LauMovimentoItem", "FaturamentocontaItemId", c => c.Long());
            CreateIndex("dbo.LauMovimentoItem", "FaturamentocontaItemId");
            AddForeignKey("dbo.LauMovimentoItem", "FaturamentocontaItemId", "dbo.FatContaItem", "Id");
            DropColumn("dbo.LauMovimento", "FaturamentocontaItemId");
        }

        public override void Down()
        {
            AddColumn("dbo.LauMovimento", "FaturamentocontaItemId", c => c.Long());
            DropForeignKey("dbo.LauMovimentoItem", "FaturamentocontaItemId", "dbo.FatContaItem");
            DropIndex("dbo.LauMovimentoItem", new[] { "FaturamentocontaItemId" });
            DropColumn("dbo.LauMovimentoItem", "FaturamentocontaItemId");
            CreateIndex("dbo.LauMovimento", "FaturamentocontaItemId");
            AddForeignKey("dbo.LauMovimento", "FaturamentocontaItemId", "dbo.FatContaItem", "Id");
        }
    }
}
