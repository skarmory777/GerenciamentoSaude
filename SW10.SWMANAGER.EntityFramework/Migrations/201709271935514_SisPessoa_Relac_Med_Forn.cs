namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SisPessoa_Relac_Med_Forn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Convenio", "SisPessoaId", c => c.Long());
            AddColumn("dbo.Paciente", "SisPessoaId", c => c.Long());
            AddColumn("dbo.Fornecedor", "SisPessoaId", c => c.Long());
            CreateIndex("dbo.Convenio", "SisPessoaId");
            CreateIndex("dbo.Paciente", "SisPessoaId");
            CreateIndex("dbo.Fornecedor", "SisPessoaId");
            AddForeignKey("dbo.Convenio", "SisPessoaId", "dbo.SisPessoa", "Id");
            AddForeignKey("dbo.Paciente", "SisPessoaId", "dbo.SisPessoa", "Id");
            AddForeignKey("dbo.Fornecedor", "SisPessoaId", "dbo.SisPessoa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Fornecedor", "SisPessoaId", "dbo.SisPessoa");
            DropForeignKey("dbo.Paciente", "SisPessoaId", "dbo.SisPessoa");
            DropForeignKey("dbo.Convenio", "SisPessoaId", "dbo.SisPessoa");
            DropIndex("dbo.Fornecedor", new[] { "SisPessoaId" });
            DropIndex("dbo.Paciente", new[] { "SisPessoaId" });
            DropIndex("dbo.Convenio", new[] { "SisPessoaId" });
            DropColumn("dbo.Fornecedor", "SisPessoaId");
            DropColumn("dbo.Paciente", "SisPessoaId");
            DropColumn("dbo.Convenio", "SisPessoaId");
        }
    }
}
