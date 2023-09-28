namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_campos_em_requsisicao_compra : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CmpRequisicaoItem", "EstProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.CmpRequisicaoItem", new[] { "EstProdutoId" });
            AddColumn("dbo.CmpRequisicaoItem", "EstProdutoAprovacaoId", c => c.Long());
            AddColumn("dbo.CmpRequisicaoItem", "UnidadeAprovacaoId", c => c.Long());
            AddColumn("dbo.CmpRequisicaoItem", "QuantidadeAprovacao", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CmpRequisicaoItem", "EstProdutoId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicaoItem", "EstProdutoId");
            CreateIndex("dbo.CmpRequisicaoItem", "EstProdutoAprovacaoId");
            CreateIndex("dbo.CmpRequisicaoItem", "UnidadeAprovacaoId");
            AddForeignKey("dbo.CmpRequisicaoItem", "EstProdutoAprovacaoId", "dbo.Est_Produto", "Id");
            AddForeignKey("dbo.CmpRequisicaoItem", "UnidadeAprovacaoId", "dbo.Est_Unidade", "Id");
            AddForeignKey("dbo.CmpRequisicaoItem", "EstProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicaoItem", "EstProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.CmpRequisicaoItem", "UnidadeAprovacaoId", "dbo.Est_Unidade");
            DropForeignKey("dbo.CmpRequisicaoItem", "EstProdutoAprovacaoId", "dbo.Est_Produto");
            DropIndex("dbo.CmpRequisicaoItem", new[] { "UnidadeAprovacaoId" });
            DropIndex("dbo.CmpRequisicaoItem", new[] { "EstProdutoAprovacaoId" });
            DropIndex("dbo.CmpRequisicaoItem", new[] { "EstProdutoId" });
            AlterColumn("dbo.CmpRequisicaoItem", "EstProdutoId", c => c.Long());
            DropColumn("dbo.CmpRequisicaoItem", "QuantidadeAprovacao");
            DropColumn("dbo.CmpRequisicaoItem", "UnidadeAprovacaoId");
            DropColumn("dbo.CmpRequisicaoItem", "EstProdutoAprovacaoId");
            CreateIndex("dbo.CmpRequisicaoItem", "EstProdutoId");
            AddForeignKey("dbo.CmpRequisicaoItem", "EstProdutoId", "dbo.Est_Produto", "Id");
        }
    }
}
