namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class SisPessoa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisPessoa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    NomeCompleto = c.String(nullable: false, maxLength: 100),
                    Nascimento = c.DateTime(nullable: false),
                    Sexo = c.Int(),
                    CorPele = c.Int(),
                    ProfissaoId = c.Long(),
                    Escolaridade = c.Int(),
                    Rg = c.String(maxLength: 20),
                    Emissor = c.String(maxLength: 20),
                    Emissao = c.DateTime(),
                    Cpf = c.String(nullable: false, maxLength: 14),
                    NaturalidadeId = c.Long(),
                    NacionalidadeId = c.Long(),
                    EstadoCivil = c.Int(),
                    NomeMae = c.String(maxLength: 100),
                    NomePai = c.String(maxLength: 100),
                    Religiao = c.Int(),
                    Foto = c.Binary(),
                    FotoMimeType = c.String(),
                    RazaoSocial = c.String(nullable: false),
                    NomeFantasia = c.String(nullable: false),
                    Cnpj = c.String(nullable: false),
                    InscricaoEstadual = c.String(),
                    InscricaoMunicipal = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_SisPessoa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Nacionalidade", t => t.NacionalidadeId)
                .ForeignKey("dbo.Naturalidade", t => t.NaturalidadeId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.Profissao", t => t.ProfissaoId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId)
                .Index(t => t.ProfissaoId)
                .Index(t => t.NaturalidadeId)
                .Index(t => t.NacionalidadeId);

            AddColumn("dbo.Medico", "SisPessoaId", c => c.Long());
            CreateIndex("dbo.Medico", "SisPessoaId");
            AddForeignKey("dbo.Medico", "SisPessoaId", "dbo.SisPessoa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Medico", "SisPessoaId", "dbo.SisPessoa");
            DropForeignKey("dbo.SisPessoa", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.SisPessoa", "ProfissaoId", "dbo.Profissao");
            DropForeignKey("dbo.SisPessoa", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.SisPessoa", "NaturalidadeId", "dbo.Naturalidade");
            DropForeignKey("dbo.SisPessoa", "NacionalidadeId", "dbo.Nacionalidade");
            DropForeignKey("dbo.SisPessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.SisPessoa", "CidadeId", "dbo.Cidade");
            DropIndex("dbo.SisPessoa", new[] { "NacionalidadeId" });
            DropIndex("dbo.SisPessoa", new[] { "NaturalidadeId" });
            DropIndex("dbo.SisPessoa", new[] { "ProfissaoId" });
            DropIndex("dbo.SisPessoa", new[] { "PaisId" });
            DropIndex("dbo.SisPessoa", new[] { "EstadoId" });
            DropIndex("dbo.SisPessoa", new[] { "CidadeId" });
            DropIndex("dbo.SisPessoa", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Medico", new[] { "SisPessoaId" });
            DropColumn("dbo.Medico", "SisPessoaId");
            DropTable("dbo.SisPessoa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisPessoa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
