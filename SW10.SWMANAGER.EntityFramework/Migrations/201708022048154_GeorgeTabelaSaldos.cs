namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeTabelaSaldos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProdutoSaldoEmprestimo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    FornecedorId = c.Long(nullable: false),
                    QuantidadeAtual = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_ProdutoSaldoEmprestimo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.FornecedorId);

            CreateTable(
                "dbo.ProdutoSaldo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoqueId = c.Long(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    LoteValidadeId = c.Long(nullable: false),
                    EmprestimoId = c.Long(nullable: false),
                    ConsignadoId = c.Long(nullable: false),
                    ValeId = c.Long(nullable: false),
                    QuantidadeAtual = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_ProdutoSaldo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fornecedor", t => t.ConsignadoId, cascadeDelete: false)
                .ForeignKey("dbo.Fornecedor", t => t.EmprestimoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Estoque", t => t.EstoqueId, cascadeDelete: false)
                .ForeignKey("dbo.LoteValidade", t => t.LoteValidadeId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.Fornecedor", t => t.ValeId, cascadeDelete: false)
                .Index(t => t.EstoqueId)
                .Index(t => t.ProdutoId)
                .Index(t => t.LoteValidadeId)
                .Index(t => t.EmprestimoId)
                .Index(t => t.ConsignadoId)
                .Index(t => t.ValeId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ProdutoSaldo", "ValeId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldo", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoSaldo", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.ProdutoSaldo", "EstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.ProdutoSaldo", "EmprestimoId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldo", "ConsignadoId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldoEmprestimo", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoSaldoEmprestimo", "FornecedorId", "dbo.Fornecedor");
            DropIndex("dbo.ProdutoSaldo", new[] { "ValeId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "ConsignadoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "EmprestimoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "LoteValidadeId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "EstoqueId" });
            DropIndex("dbo.ProdutoSaldoEmprestimo", new[] { "FornecedorId" });
            DropIndex("dbo.ProdutoSaldoEmprestimo", new[] { "ProdutoId" });
            DropTable("dbo.ProdutoSaldo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoSaldo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoSaldoEmprestimo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoSaldoEmprestimo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
