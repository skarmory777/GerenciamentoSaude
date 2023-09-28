namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_Classes_Brasindice_CamposNovos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatBrasApresentacao", "Quantidade", c => c.Single(nullable: false));
            AddColumn("dbo.FatBrasImport", "CodigoLaboratorio", c => c.String());
            AddColumn("dbo.FatBrasImport", "Laboratorio", c => c.String());
            AddColumn("dbo.FatBrasImport", "CodigoProduto", c => c.String());
            AddColumn("dbo.FatBrasImport", "Produto", c => c.String());
            AddColumn("dbo.FatBrasImport", "CodigoApresentacao", c => c.String());
            AddColumn("dbo.FatBrasImport", "Apresentacao", c => c.String());
            AddColumn("dbo.FatBrasImport", "PrecoUnitario", c => c.String());
            AddColumn("dbo.FatBrasImport", "PrecoTotal", c => c.String());
            AddColumn("dbo.FatBrasImport", "NumeroUnidades", c => c.String());
            AddColumn("dbo.FatBrasImport", "Tipo", c => c.String());
            AddColumn("dbo.FatBrasImport", "Versao", c => c.String());
            AddColumn("dbo.FatBrasImport", "Extra", c => c.String());
            AddColumn("dbo.FatBrasImport", "IsAtualizado", c => c.String());
            AddColumn("dbo.FatBrasImport", "CodigoBarra", c => c.String());
            AddColumn("dbo.FatBrasImport", "CodigoBrasTiss", c => c.String());
            AddColumn("dbo.FatBrasImport", "CodigoBrasTuss", c => c.String());
            AddColumn("dbo.FatBrasImport", "CodigoHierarquico", c => c.String());
            AddColumn("dbo.FatBrasPreco", "BrasItemId", c => c.Long(nullable: false));
            AddColumn("dbo.FatBrasPreco", "BrasApresentacaoId", c => c.Long());
            AddColumn("dbo.FatBrasPreco", "BrasLaboratorioId", c => c.Long());
            AddColumn("dbo.FatBrasPreco", "Preco", c => c.Double(nullable: false));
            AddColumn("dbo.FatBrasPreco", "Tipo", c => c.String());
            AddColumn("dbo.FatBrasPreco", "CodigoBrasTiss", c => c.String());
            AddColumn("dbo.FatBrasPreco", "CodigoBrasTuss", c => c.String());
            CreateIndex("dbo.FatBrasPreco", "BrasItemId");
            CreateIndex("dbo.FatBrasPreco", "BrasApresentacaoId");
            CreateIndex("dbo.FatBrasPreco", "BrasLaboratorioId");
            AddForeignKey("dbo.FatBrasPreco", "BrasApresentacaoId", "dbo.FatBrasApresentacao", "Id");
            AddForeignKey("dbo.FatBrasPreco", "BrasItemId", "dbo.FatBrasItem", "Id", cascadeDelete: false);
            AddForeignKey("dbo.FatBrasPreco", "BrasLaboratorioId", "dbo.FatBrasLaboratorio", "Id");
            DropColumn("dbo.FatBrasImport", "Codigo");
            DropColumn("dbo.FatBrasImport", "Descricao");
            DropColumn("dbo.FatBrasPreco", "Descricao");
        }

        public override void Down()
        {
            AddColumn("dbo.FatBrasPreco", "Descricao", c => c.String(maxLength: 100));
            AddColumn("dbo.FatBrasImport", "Descricao", c => c.String(maxLength: 100));
            AddColumn("dbo.FatBrasImport", "Codigo", c => c.String(maxLength: 10));
            DropForeignKey("dbo.FatBrasPreco", "BrasLaboratorioId", "dbo.FatBrasLaboratorio");
            DropForeignKey("dbo.FatBrasPreco", "BrasItemId", "dbo.FatBrasItem");
            DropForeignKey("dbo.FatBrasPreco", "BrasApresentacaoId", "dbo.FatBrasApresentacao");
            DropIndex("dbo.FatBrasPreco", new[] { "BrasLaboratorioId" });
            DropIndex("dbo.FatBrasPreco", new[] { "BrasApresentacaoId" });
            DropIndex("dbo.FatBrasPreco", new[] { "BrasItemId" });
            DropColumn("dbo.FatBrasPreco", "CodigoBrasTuss");
            DropColumn("dbo.FatBrasPreco", "CodigoBrasTiss");
            DropColumn("dbo.FatBrasPreco", "Tipo");
            DropColumn("dbo.FatBrasPreco", "Preco");
            DropColumn("dbo.FatBrasPreco", "BrasLaboratorioId");
            DropColumn("dbo.FatBrasPreco", "BrasApresentacaoId");
            DropColumn("dbo.FatBrasPreco", "BrasItemId");
            DropColumn("dbo.FatBrasImport", "CodigoHierarquico");
            DropColumn("dbo.FatBrasImport", "CodigoBrasTuss");
            DropColumn("dbo.FatBrasImport", "CodigoBrasTiss");
            DropColumn("dbo.FatBrasImport", "CodigoBarra");
            DropColumn("dbo.FatBrasImport", "IsAtualizado");
            DropColumn("dbo.FatBrasImport", "Extra");
            DropColumn("dbo.FatBrasImport", "Versao");
            DropColumn("dbo.FatBrasImport", "Tipo");
            DropColumn("dbo.FatBrasImport", "NumeroUnidades");
            DropColumn("dbo.FatBrasImport", "PrecoTotal");
            DropColumn("dbo.FatBrasImport", "PrecoUnitario");
            DropColumn("dbo.FatBrasImport", "Apresentacao");
            DropColumn("dbo.FatBrasImport", "CodigoApresentacao");
            DropColumn("dbo.FatBrasImport", "Produto");
            DropColumn("dbo.FatBrasImport", "CodigoProduto");
            DropColumn("dbo.FatBrasImport", "Laboratorio");
            DropColumn("dbo.FatBrasImport", "CodigoLaboratorio");
            DropColumn("dbo.FatBrasApresentacao", "Quantidade");
        }
    }
}
