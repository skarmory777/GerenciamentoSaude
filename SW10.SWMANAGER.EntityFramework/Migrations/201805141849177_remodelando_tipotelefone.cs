namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class remodelando_tipotelefone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisEmpresa", "TipoTelefone1Id", c => c.Long());
            AddColumn("dbo.SisEmpresa", "TipoTelefone2Id", c => c.Long());
            AddColumn("dbo.SisEmpresa", "TipoTelefone3Id", c => c.Long());
            AddColumn("dbo.SisEmpresa", "TipoTelefone4Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone1Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone2Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone3Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone4Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone1Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone2Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone3Id", c => c.Long());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone4Id", c => c.Long());
            AddColumn("dbo.SisPrestador", "TipoTelefone1Id", c => c.Long());
            AddColumn("dbo.SisPrestador", "TipoTelefone2Id", c => c.Long());
            AddColumn("dbo.SisPrestador", "TipoTelefone3Id", c => c.Long());
            AddColumn("dbo.SisPrestador", "TipoTelefone4Id", c => c.Long());
            CreateIndex("dbo.SisEmpresa", "TipoTelefone1Id");
            CreateIndex("dbo.SisEmpresa", "TipoTelefone2Id");
            CreateIndex("dbo.SisEmpresa", "TipoTelefone3Id");
            CreateIndex("dbo.SisEmpresa", "TipoTelefone4Id");
            CreateIndex("dbo.FornecedorPessoaFisica", "TipoTelefone1Id");
            CreateIndex("dbo.FornecedorPessoaFisica", "TipoTelefone2Id");
            CreateIndex("dbo.FornecedorPessoaFisica", "TipoTelefone3Id");
            CreateIndex("dbo.FornecedorPessoaFisica", "TipoTelefone4Id");
            CreateIndex("dbo.FornecedorPessoaJuridica", "TipoTelefone1Id");
            CreateIndex("dbo.FornecedorPessoaJuridica", "TipoTelefone2Id");
            CreateIndex("dbo.FornecedorPessoaJuridica", "TipoTelefone3Id");
            CreateIndex("dbo.FornecedorPessoaJuridica", "TipoTelefone4Id");
            CreateIndex("dbo.SisPrestador", "TipoTelefone1Id");
            CreateIndex("dbo.SisPrestador", "TipoTelefone2Id");
            CreateIndex("dbo.SisPrestador", "TipoTelefone3Id");
            CreateIndex("dbo.SisPrestador", "TipoTelefone4Id");
            AddForeignKey("dbo.SisEmpresa", "TipoTelefone1Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisEmpresa", "TipoTelefone2Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisEmpresa", "TipoTelefone3Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisEmpresa", "TipoTelefone4Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone1Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone2Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone3Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone4Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone1Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone2Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone3Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone4Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisPrestador", "TipoTelefone1Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisPrestador", "TipoTelefone2Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisPrestador", "TipoTelefone3Id", "dbo.SisTipoTelefone", "Id");
            AddForeignKey("dbo.SisPrestador", "TipoTelefone4Id", "dbo.SisTipoTelefone", "Id");
            DropColumn("dbo.SisEmpresa", "TipoTelefone1");
            DropColumn("dbo.SisEmpresa", "TipoTelefone2");
            DropColumn("dbo.SisEmpresa", "TipoTelefone3");
            DropColumn("dbo.SisEmpresa", "TipoTelefone4");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone1");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone2");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone3");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone4");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone1");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone2");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone3");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone4");
            DropColumn("dbo.SisPrestador", "TipoTelefone1");
            DropColumn("dbo.SisPrestador", "TipoTelefone2");
            DropColumn("dbo.SisPrestador", "TipoTelefone3");
            DropColumn("dbo.SisPrestador", "TipoTelefone4");
        }

        public override void Down()
        {
            AddColumn("dbo.SisPrestador", "TipoTelefone4", c => c.Int());
            AddColumn("dbo.SisPrestador", "TipoTelefone3", c => c.Int());
            AddColumn("dbo.SisPrestador", "TipoTelefone2", c => c.Int());
            AddColumn("dbo.SisPrestador", "TipoTelefone1", c => c.Int());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone4", c => c.Int());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone3", c => c.Int());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone2", c => c.Int());
            AddColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone1", c => c.Int());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone4", c => c.Int());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone3", c => c.Int());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone2", c => c.Int());
            AddColumn("dbo.FornecedorPessoaFisica", "TipoTelefone1", c => c.Int());
            AddColumn("dbo.SisEmpresa", "TipoTelefone4", c => c.Int());
            AddColumn("dbo.SisEmpresa", "TipoTelefone3", c => c.Int());
            AddColumn("dbo.SisEmpresa", "TipoTelefone2", c => c.Int());
            AddColumn("dbo.SisEmpresa", "TipoTelefone1", c => c.Int());
            DropForeignKey("dbo.SisPrestador", "TipoTelefone4Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisPrestador", "TipoTelefone3Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisPrestador", "TipoTelefone2Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisPrestador", "TipoTelefone1Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone4Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone3Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone2Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "TipoTelefone1Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone4Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone3Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone2Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.FornecedorPessoaFisica", "TipoTelefone1Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisEmpresa", "TipoTelefone4Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisEmpresa", "TipoTelefone3Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisEmpresa", "TipoTelefone2Id", "dbo.SisTipoTelefone");
            DropForeignKey("dbo.SisEmpresa", "TipoTelefone1Id", "dbo.SisTipoTelefone");
            DropIndex("dbo.SisPrestador", new[] { "TipoTelefone4Id" });
            DropIndex("dbo.SisPrestador", new[] { "TipoTelefone3Id" });
            DropIndex("dbo.SisPrestador", new[] { "TipoTelefone2Id" });
            DropIndex("dbo.SisPrestador", new[] { "TipoTelefone1Id" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "TipoTelefone4Id" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "TipoTelefone3Id" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "TipoTelefone2Id" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "TipoTelefone1Id" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "TipoTelefone4Id" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "TipoTelefone3Id" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "TipoTelefone2Id" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "TipoTelefone1Id" });
            DropIndex("dbo.SisEmpresa", new[] { "TipoTelefone4Id" });
            DropIndex("dbo.SisEmpresa", new[] { "TipoTelefone3Id" });
            DropIndex("dbo.SisEmpresa", new[] { "TipoTelefone2Id" });
            DropIndex("dbo.SisEmpresa", new[] { "TipoTelefone1Id" });
            DropColumn("dbo.SisPrestador", "TipoTelefone4Id");
            DropColumn("dbo.SisPrestador", "TipoTelefone3Id");
            DropColumn("dbo.SisPrestador", "TipoTelefone2Id");
            DropColumn("dbo.SisPrestador", "TipoTelefone1Id");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone4Id");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone3Id");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone2Id");
            DropColumn("dbo.FornecedorPessoaJuridica", "TipoTelefone1Id");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone4Id");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone3Id");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone2Id");
            DropColumn("dbo.FornecedorPessoaFisica", "TipoTelefone1Id");
            DropColumn("dbo.SisEmpresa", "TipoTelefone4Id");
            DropColumn("dbo.SisEmpresa", "TipoTelefone3Id");
            DropColumn("dbo.SisEmpresa", "TipoTelefone2Id");
            DropColumn("dbo.SisEmpresa", "TipoTelefone1Id");
        }
    }
}
