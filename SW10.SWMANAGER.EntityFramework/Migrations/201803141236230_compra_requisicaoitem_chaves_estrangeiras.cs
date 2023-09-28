namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class compra_requisicaoitem_chaves_estrangeiras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicaoItem", "CmpRequisicaoId", c => c.Long(nullable: false));
            AddColumn("dbo.CmpRequisicaoItem", "EstProdutoId", c => c.Long());
            CreateIndex("dbo.CmpRequisicaoItem", "CmpRequisicaoId");
            CreateIndex("dbo.CmpRequisicaoItem", "EstProdutoId");
            AddForeignKey("dbo.CmpRequisicaoItem", "EstProdutoId", "dbo.Est_Produto", "Id");
            AddForeignKey("dbo.CmpRequisicaoItem", "CmpRequisicaoId", "dbo.CmpRequisicao", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicaoItem", "CmpRequisicaoId", "dbo.CmpRequisicao");
            DropForeignKey("dbo.CmpRequisicaoItem", "EstProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.CmpRequisicaoItem", new[] { "EstProdutoId" });
            DropIndex("dbo.CmpRequisicaoItem", new[] { "CmpRequisicaoId" });
            DropColumn("dbo.CmpRequisicaoItem", "EstProdutoId");
            DropColumn("dbo.CmpRequisicaoItem", "CmpRequisicaoId");
        }
    }
}
