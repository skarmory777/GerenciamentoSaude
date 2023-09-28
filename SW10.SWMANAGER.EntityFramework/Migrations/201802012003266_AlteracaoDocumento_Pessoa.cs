namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoDocumento_Pessoa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinDocumento", "ForncedorId", "dbo.SisFornecedor");
            DropIndex("dbo.FinDocumento", new[] { "ForncedorId" });
            AddColumn("dbo.FinDocumento", "PessoaId", c => c.Long());
            CreateIndex("dbo.FinDocumento", "PessoaId");
            AddForeignKey("dbo.FinDocumento", "PessoaId", "dbo.SisPessoa", "Id");
            DropColumn("dbo.FinDocumento", "ForncedorId");
        }

        public override void Down()
        {
            AddColumn("dbo.FinDocumento", "ForncedorId", c => c.Long());
            DropForeignKey("dbo.FinDocumento", "PessoaId", "dbo.SisPessoa");
            DropIndex("dbo.FinDocumento", new[] { "PessoaId" });
            DropColumn("dbo.FinDocumento", "PessoaId");
            CreateIndex("dbo.FinDocumento", "ForncedorId");
            AddForeignKey("dbo.FinDocumento", "ForncedorId", "dbo.SisFornecedor", "Id");
        }
    }
}
