namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoFaturamentoItemAutorizacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.FatFaturamentoItemAutorizacao", new[] { "FaturamentoItemId" });
            AlterColumn("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", c => c.Long());
            CreateIndex("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId");
            AddForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", "dbo.FatItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.FatFaturamentoItemAutorizacao", new[] { "FaturamentoItemId" });
            AlterColumn("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", c => c.Long(nullable: false));
            CreateIndex("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId");
            AddForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", "dbo.FatItem", "Id", cascadeDelete: true);
        }
    }
}
