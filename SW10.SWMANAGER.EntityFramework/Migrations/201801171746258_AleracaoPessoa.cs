namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AleracaoPessoa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisPessoa", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.SisPessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.SisPessoa", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.SisPessoa", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropIndex("dbo.SisPessoa", new[] { "TipoLogradouroId" });
            DropIndex("dbo.SisPessoa", new[] { "CidadeId" });
            DropIndex("dbo.SisPessoa", new[] { "EstadoId" });
            DropIndex("dbo.SisPessoa", new[] { "PaisId" });
            CreateTable(
                "dbo.SisTipoPessoa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsReceber = c.Boolean(nullable: false),
                    IsPagar = c.Boolean(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_SisTipoPessoa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.SisEndereco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PesssoaId = c.Long(),
                    TipoLogradouroId = c.Long(),
                    Cep = c.String(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    TipoEnderecoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_Endereco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisCidade", t => t.CidadeId)
                .ForeignKey("dbo.SisEstado", t => t.EstadoId)
                .ForeignKey("dbo.SisPais", t => t.PaisId)
                .ForeignKey("dbo.SisPessoa", t => t.PesssoaId)
                .ForeignKey("dbo.TipoEnderecoes", t => t.TipoEnderecoId)
                .ForeignKey("dbo.SisTipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.PesssoaId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId)
                .Index(t => t.TipoEnderecoId);

            CreateTable(
                "dbo.TipoEnderecoes",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_TipoEndereco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.SisPessoa", "TipoPessoaId", c => c.Long(nullable: false));
            AddColumn("dbo.SisPessoa", "EmissaoRg", c => c.DateTime());
            AddColumn("dbo.SisPessoa", "ReligiaoId", c => c.Long());
            AddColumn("dbo.SisPessoa", "FisicaJuricada", c => c.String(maxLength: 1));
            AddColumn("dbo.SisPessoa", "IsAtivo", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SisTipoLogradouro", "Descricao", c => c.String());
            AlterColumn("dbo.SisPessoa", "NomeCompleto", c => c.String(maxLength: 100));
            AlterColumn("dbo.SisPessoa", "Cpf", c => c.String(maxLength: 14));
            AlterColumn("dbo.SisPessoa", "RazaoSocial", c => c.String());
            AlterColumn("dbo.SisPessoa", "NomeFantasia", c => c.String());
            AlterColumn("dbo.SisPessoa", "Cnpj", c => c.String());
            CreateIndex("dbo.SisPessoa", "TipoPessoaId");
            CreateIndex("dbo.SisPessoa", "ReligiaoId");
            AddForeignKey("dbo.SisPessoa", "ReligiaoId", "dbo.SisReligiao", "Id");
            AddForeignKey("dbo.SisPessoa", "TipoPessoaId", "dbo.SisTipoPessoa", "Id", cascadeDelete: true);
            DropColumn("dbo.SisPessoa", "Cep");
            DropColumn("dbo.SisPessoa", "TipoLogradouroId");
            DropColumn("dbo.SisPessoa", "Logradouro");
            DropColumn("dbo.SisPessoa", "Complemento");
            DropColumn("dbo.SisPessoa", "Numero");
            DropColumn("dbo.SisPessoa", "Bairro");
            DropColumn("dbo.SisPessoa", "CidadeId");
            DropColumn("dbo.SisPessoa", "EstadoId");
            DropColumn("dbo.SisPessoa", "PaisId");
            DropColumn("dbo.SisPessoa", "Emissao");
            DropColumn("dbo.SisPessoa", "Religiao");
        }

        public override void Down()
        {
            AddColumn("dbo.SisPessoa", "Religiao", c => c.Int());
            AddColumn("dbo.SisPessoa", "Emissao", c => c.DateTime());
            AddColumn("dbo.SisPessoa", "PaisId", c => c.Long());
            AddColumn("dbo.SisPessoa", "EstadoId", c => c.Long());
            AddColumn("dbo.SisPessoa", "CidadeId", c => c.Long());
            AddColumn("dbo.SisPessoa", "Bairro", c => c.String(maxLength: 40));
            AddColumn("dbo.SisPessoa", "Numero", c => c.String(maxLength: 20));
            AddColumn("dbo.SisPessoa", "Complemento", c => c.String(maxLength: 30));
            AddColumn("dbo.SisPessoa", "Logradouro", c => c.String(maxLength: 80));
            AddColumn("dbo.SisPessoa", "TipoLogradouroId", c => c.Long());
            AddColumn("dbo.SisPessoa", "Cep", c => c.String(maxLength: 9));
            DropForeignKey("dbo.SisEndereco", "TipoLogradouroId", "dbo.SisTipoLogradouro");
            DropForeignKey("dbo.SisEndereco", "TipoEnderecoId", "dbo.TipoEnderecoes");
            DropForeignKey("dbo.SisEndereco", "PesssoaId", "dbo.SisPessoa");
            DropForeignKey("dbo.SisEndereco", "PaisId", "dbo.SisPais");
            DropForeignKey("dbo.SisEndereco", "EstadoId", "dbo.SisEstado");
            DropForeignKey("dbo.SisEndereco", "CidadeId", "dbo.SisCidade");
            DropForeignKey("dbo.SisPessoa", "TipoPessoaId", "dbo.SisTipoPessoa");
            DropForeignKey("dbo.SisPessoa", "ReligiaoId", "dbo.SisReligiao");
            DropIndex("dbo.SisEndereco", new[] { "TipoEnderecoId" });
            DropIndex("dbo.SisEndereco", new[] { "PaisId" });
            DropIndex("dbo.SisEndereco", new[] { "EstadoId" });
            DropIndex("dbo.SisEndereco", new[] { "CidadeId" });
            DropIndex("dbo.SisEndereco", new[] { "TipoLogradouroId" });
            DropIndex("dbo.SisEndereco", new[] { "PesssoaId" });
            DropIndex("dbo.SisPessoa", new[] { "ReligiaoId" });
            DropIndex("dbo.SisPessoa", new[] { "TipoPessoaId" });
            AlterColumn("dbo.SisPessoa", "Cnpj", c => c.String(nullable: false));
            AlterColumn("dbo.SisPessoa", "NomeFantasia", c => c.String(nullable: false));
            AlterColumn("dbo.SisPessoa", "RazaoSocial", c => c.String(nullable: false));
            AlterColumn("dbo.SisPessoa", "Cpf", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.SisPessoa", "NomeCompleto", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.SisTipoLogradouro", "Descricao", c => c.String(maxLength: 255));
            DropColumn("dbo.SisPessoa", "IsAtivo");
            DropColumn("dbo.SisPessoa", "FisicaJuricada");
            DropColumn("dbo.SisPessoa", "ReligiaoId");
            DropColumn("dbo.SisPessoa", "EmissaoRg");
            DropColumn("dbo.SisPessoa", "TipoPessoaId");
            DropTable("dbo.TipoEnderecoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoEndereco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisEndereco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Endereco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisTipoPessoa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisTipoPessoa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.SisPessoa", "PaisId");
            CreateIndex("dbo.SisPessoa", "EstadoId");
            CreateIndex("dbo.SisPessoa", "CidadeId");
            CreateIndex("dbo.SisPessoa", "TipoLogradouroId");
            AddForeignKey("dbo.SisPessoa", "TipoLogradouroId", "dbo.SisTipoLogradouro", "Id");
            AddForeignKey("dbo.SisPessoa", "PaisId", "dbo.SisPais", "Id");
            AddForeignKey("dbo.SisPessoa", "EstadoId", "dbo.SisEstado", "Id");
            AddForeignKey("dbo.SisPessoa", "CidadeId", "dbo.SisCidade", "Id");
        }
    }
}
