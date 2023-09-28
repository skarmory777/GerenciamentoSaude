namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoNomeClasse : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProdutoEstoque", newName: "Est_ProdutoEstoque");
            DropForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoEstoqueId", "dbo.ProdutoEstoque");
            DropForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoLocalizacaoId", "dbo.ProdutoLocalizacao");
            DropIndex("dbo.ProdutoRelacaoEstoque", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoRelacaoEstoque", new[] { "ProdutoEstoqueId" });
            DropIndex("dbo.ProdutoRelacaoEstoque", new[] { "ProdutoLocalizacaoId" });
            AddColumn("dbo.Est_ProdutoEstoque", "ProdutoId", c => c.Long(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "EstoqueId", c => c.Long(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "EstoqueMinimo", c => c.Long(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "EstoqueMaximo", c => c.Long(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "PontoPedido", c => c.Long(nullable: false));
            CreateIndex("dbo.Est_ProdutoEstoque", "ProdutoId");
            CreateIndex("dbo.Est_ProdutoEstoque", "EstoqueId");
            //AddForeignKey("dbo.Est_ProdutoEstoque", "EstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Est_ProdutoEstoque", "ProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
            DropColumn("dbo.Est_ProdutoEstoque", "Descricao");
            DropColumn("dbo.Est_ProdutoEstoque", "Tipo");
            DropColumn("dbo.Est_ProdutoEstoque", "ProcessoCota");
            DropColumn("dbo.Est_ProdutoEstoque", "IsCodigoDeBarra");
            DropColumn("dbo.Est_ProdutoEstoque", "IsCustoMedio");
            DropColumn("dbo.Est_ProdutoEstoque", "IsUtilizaCota");
            DropColumn("dbo.Est_ProdutoEstoque", "IsChecarSaldo");
            DropColumn("dbo.Est_ProdutoEstoque", "IsCalcularConsumoDemanda");
            DropTable("dbo.ProdutoRelacaoEstoque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.ProdutoRelacaoEstoque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoEstoqueId = c.Long(nullable: false),
                    ProdutoLocalizacaoId = c.Long(nullable: false),
                    EstoqueMinimo = c.Long(nullable: false),
                    EstoqueMaximo = c.Long(nullable: false),
                    PontoPedido = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Est_ProdutoEstoque", "IsCalcularConsumoDemanda", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "IsChecarSaldo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "IsUtilizaCota", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "IsCustoMedio", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "IsCodigoDeBarra", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_ProdutoEstoque", "ProcessoCota", c => c.String(maxLength: 20));
            AddColumn("dbo.Est_ProdutoEstoque", "Tipo", c => c.String(maxLength: 12));
            AddColumn("dbo.Est_ProdutoEstoque", "Descricao", c => c.String(maxLength: 255));
            //DropForeignKey("dbo.Est_ProdutoEstoque", "ProdutoId", "dbo.Est_Produto");
            //DropForeignKey("dbo.Est_ProdutoEstoque", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.Est_ProdutoEstoque", new[] { "EstoqueId" });
            DropIndex("dbo.Est_ProdutoEstoque", new[] { "ProdutoId" });
            DropColumn("dbo.Est_ProdutoEstoque", "PontoPedido");
            DropColumn("dbo.Est_ProdutoEstoque", "EstoqueMaximo");
            DropColumn("dbo.Est_ProdutoEstoque", "EstoqueMinimo");
            DropColumn("dbo.Est_ProdutoEstoque", "EstoqueId");
            DropColumn("dbo.Est_ProdutoEstoque", "ProdutoId");
            CreateIndex("dbo.ProdutoRelacaoEstoque", "ProdutoLocalizacaoId");
            CreateIndex("dbo.ProdutoRelacaoEstoque", "ProdutoEstoqueId");
            CreateIndex("dbo.ProdutoRelacaoEstoque", "ProdutoId");
            AddForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoLocalizacaoId", "dbo.ProdutoLocalizacao", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoEstoqueId", "dbo.ProdutoEstoque", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Est_ProdutoEstoque", newName: "ProdutoEstoque");
        }
    }
}
