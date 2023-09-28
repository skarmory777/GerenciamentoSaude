namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoAutorizacaoProcedimento_IsProrrogacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.AteAutorizacaoProcedimentoItem", new[] { "FaturamentoItemId" });
            AddColumn("dbo.AteAutorizacaoProcedimento", "IsProrrogacao", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", c => c.Long());
            CreateIndex("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId");
            AddForeignKey("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", "dbo.FatItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.AteAutorizacaoProcedimentoItem", new[] { "FaturamentoItemId" });
            AlterColumn("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", c => c.Long(nullable: false));
            DropColumn("dbo.AteAutorizacaoProcedimento", "IsProrrogacao");
            CreateIndex("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId");
            AddForeignKey("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", "dbo.FatItem", "Id", cascadeDelete: true);
        }
    }
}
