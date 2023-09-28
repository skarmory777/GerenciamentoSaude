namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeIncluindoCamposMovimentos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoqueMovimento", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstoquePreMovimento", "FornecedorId", "dbo.Fornecedor");
            DropIndex("dbo.EstoqueMovimento", new[] { "FornecedorId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "FornecedorId" });
            AddColumn("dbo.EstoquePreMovimento", "AtendimentoId", c => c.Long());
            AlterColumn("dbo.EstoqueMovimento", "FornecedorId", c => c.Long());
            AlterColumn("dbo.EstoquePreMovimento", "FornecedorId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "FornecedorId");
            CreateIndex("dbo.EstoquePreMovimento", "FornecedorId");
            CreateIndex("dbo.EstoquePreMovimento", "AtendimentoId");
            AddForeignKey("dbo.EstoquePreMovimento", "AtendimentoId", "dbo.Atendimento", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "FornecedorId", "dbo.Fornecedor", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "FornecedorId", "dbo.Fornecedor", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstoqueMovimento", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstoquePreMovimento", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.EstoquePreMovimento", new[] { "AtendimentoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "FornecedorId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "FornecedorId" });
            AlterColumn("dbo.EstoquePreMovimento", "FornecedorId", c => c.Long(nullable: false));
            AlterColumn("dbo.EstoqueMovimento", "FornecedorId", c => c.Long(nullable: false));
            DropColumn("dbo.EstoquePreMovimento", "AtendimentoId");
            CreateIndex("dbo.EstoquePreMovimento", "FornecedorId");
            CreateIndex("dbo.EstoqueMovimento", "FornecedorId");
            AddForeignKey("dbo.EstoquePreMovimento", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoqueMovimento", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }
    }
}
