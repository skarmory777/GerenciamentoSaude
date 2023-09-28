namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeRelacionamentoBaixaEmprestimo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.EstBaixaEmprestimo", "FornecedorId");
            CreateIndex("dbo.EstBaixaEmprestimo", "EmpresaId");
            AddForeignKey("dbo.EstBaixaEmprestimo", "EmpresaId", "dbo.Empresa", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstBaixaEmprestimo", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstBaixaEmprestimo", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstBaixaEmprestimo", "EmpresaId", "dbo.Empresa");
            DropIndex("dbo.EstBaixaEmprestimo", new[] { "EmpresaId" });
            DropIndex("dbo.EstBaixaEmprestimo", new[] { "FornecedorId" });
        }
    }
}
