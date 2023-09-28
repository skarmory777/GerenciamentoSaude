namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atualizando_PrescricaoItemReposta_OrdemCompra : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.CmpOrdemCompraItem", "EstProdutoId", c => c.Long(nullable: false));
            AddColumn("dbo.AssSolicitacaoAntimicrobianos", "AssPrescricaoItemRespostaId", c => c.Long());
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorTotal", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            //CreateIndex("dbo.CmpOrdemCompraItem", "EstProdutoId");
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "AssPrescricaoItemRespostaId");
            //AddForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssPrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssPrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta");
            DropForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "AssPrescricaoItemRespostaId" });
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "EstProdutoId" });
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AssSolicitacaoAntimicrobianos", "AssPrescricaoItemRespostaId");
            DropColumn("dbo.CmpOrdemCompraItem", "EstProdutoId");
        }
    }
}
