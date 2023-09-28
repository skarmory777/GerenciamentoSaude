namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Removendo_Codigo_Descricao_dos_Cadastrados : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TipoCadastroExistente", newName: "SisTipoCadastroExistente");
            AlterTableAnnotations(
                "dbo.SisTipoCadastroExistente",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_TipoCadastroExistente_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AlterTableAnnotations(
                "dbo.TipoPessoa",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_TipoPessoa_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AddColumn("dbo.SisTipoCadastroExistente", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisTipoCadastroExistente", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.SisTipoCadastroExistente", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisTipoCadastroExistente", "DeleterUserId", c => c.Long());
            AddColumn("dbo.SisTipoCadastroExistente", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.SisTipoCadastroExistente", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.SisTipoCadastroExistente", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.SisTipoCadastroExistente", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.SisTipoCadastroExistente", "CreatorUserId", c => c.Long());
            AddColumn("dbo.TipoPessoa", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.TipoPessoa", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.TipoPessoa", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TipoPessoa", "DeleterUserId", c => c.Long());
            AddColumn("dbo.TipoPessoa", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.TipoPessoa", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.TipoPessoa", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.TipoPessoa", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.TipoPessoa", "CreatorUserId", c => c.Long());
            AlterColumn("dbo.AteTipoAtendimento", "Descricao", c => c.String());
            AlterColumn("dbo.SisNacionalidade", "Descricao", c => c.String());
            AlterColumn("dbo.SisNaturalidade", "Descricao", c => c.String());
            AlterColumn("dbo.SisProfissao", "Descricao", c => c.String());
            AlterColumn("dbo.SisReligiao", "Descricao", c => c.String());
            AlterColumn("dbo.SisUnidadeOrganizacional", "Descricao", c => c.String());
            AlterColumn("dbo.CentroCusto", "Descricao", c => c.String());
            AlterColumn("dbo.GrupoCentroCusto", "Descricao", c => c.String());
            AlterColumn("dbo.AteUnidadeInternacaoTipo", "Descricao", c => c.String());
            AlterColumn("dbo.SisConselho", "Descricao", c => c.String());
            AlterColumn("dbo.SisTipoParticipacao", "Descricao", c => c.String());
            AlterColumn("dbo.TipoVinculoEmpregaticio", "Descricao", c => c.String());
            AlterColumn("dbo.LeitoStatus", "Descricao", c => c.String());
            AlterColumn("dbo.SisTabelaDominio", "Descricao", c => c.String());
            AlterColumn("dbo.SisGrupoTipoTabelaDominio", "Descricao", c => c.String());
            AlterColumn("dbo.SisTipoTabelaDominio", "Descricao", c => c.String());
            AlterColumn("dbo.AssMotivoAlta", "Descricao", c => c.String());
            AlterColumn("dbo.AssMotivoAltaTipoAlta", "Descricao", c => c.String());
            AlterColumn("dbo.SisOrigem", "Descricao", c => c.String());
            AlterColumn("dbo.SisTipoSanguineo", "Descricao", c => c.String());
            AlterColumn("dbo.SisPrestador", "Descricao", c => c.String());
            AlterColumn("dbo.SisTipoPrestador", "Descricao", c => c.String());
            AlterColumn("dbo.FatBrasItem", "Descricao", c => c.String());
            AlterColumn("dbo.FatTipoGrupo", "Descricao", c => c.String());
            AlterColumn("dbo.CapituloCID", "Descricao", c => c.String());
            AlterColumn("dbo.Cfop", "Descricao", c => c.String());
            AlterColumn("dbo.ConsultorTabela", "Descricao", c => c.String());
            AlterColumn("dbo.ConsultorTabelaCampo", "Descricao", c => c.String());
            AlterColumn("dbo.ControleProducao", "Descricao", c => c.String());
            AlterColumn("dbo.AssTipoResposta", "Descricao", c => c.String());
            AlterColumn("dbo.FatBrasLaboratorio", "Descricao", c => c.String());
            AlterColumn("dbo.FatBrasApresentacao", "Descricao", c => c.String());
            AlterColumn("dbo.FatTabela", "Descricao", c => c.String());
            AlterColumn("dbo.TipoLeito", "Descricao", c => c.String());
            AlterColumn("dbo.FatContaKit", "Descricao", c => c.String());
            AlterColumn("dbo.Terceirizado", "Descricao", c => c.String());
            AlterColumn("dbo.Feriado", "Descricao", c => c.String());
            AlterColumn("dbo.GrauInstrucao", "Descricao", c => c.String());
            AlterColumn("dbo.GrupoCID", "Codigo", c => c.String(maxLength: 10));
            AlterColumn("dbo.GrupoCID", "Descricao", c => c.String());
            AlterColumn("dbo.GrupoProcedimento", "Descricao", c => c.String());
            AlterColumn("dbo.Indicacao", "Descricao", c => c.String());
            AlterColumn("dbo.InstituicaoTransferencia", "Descricao", c => c.String());
            AlterColumn("dbo.LeitoCaracteristica", "Descricao", c => c.String());
            AlterColumn("dbo.LeitoServico", "Descricao", c => c.String());
            AlterColumn("dbo.MotivoCancelamento", "Descricao", c => c.String());
            AlterColumn("dbo.MotivoCaucao", "Descricao", c => c.String());
            AlterColumn("dbo.MotivoTransferenciaLeito", "Descricao", c => c.String());
            AlterColumn("dbo.Parentesco", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoAcaoTerapeutica", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoCodigoMedicamento", "Codigo", c => c.String(maxLength: 10));
            AlterColumn("dbo.ProdutoCodigoMedicamento", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoGrupoTratamento", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoLocalizacao", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoPortaria", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoTipoUnidade", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoClasse", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoGrupo", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoSubClasse", "Descricao", c => c.String());
            AlterColumn("dbo.ProdutoSubstancia", "Descricao", c => c.String());
            AlterColumn("dbo.Regiao", "Descricao", c => c.String());
            AlterColumn("dbo.TipoEntrada", "Descricao", c => c.String());
            AlterColumn("dbo.SisTipoTelefone", "Descricao", c => c.String());
            AlterColumn("dbo.UnidadeInternacao", "Descricao", c => c.String());
            DropColumn("dbo.AteTipoAtendimento", "CodTipoAtendimento");
        }

        public override void Down()
        {
            AddColumn("dbo.AteTipoAtendimento", "CodTipoAtendimento", c => c.String(maxLength: 10));
            AlterColumn("dbo.UnidadeInternacao", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.SisTipoTelefone", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.TipoEntrada", "Descricao", c => c.String(maxLength: 30));
            AlterColumn("dbo.Regiao", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoSubstancia", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoSubClasse", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoGrupo", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoClasse", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoTipoUnidade", "Descricao", c => c.String(maxLength: 30));
            AlterColumn("dbo.ProdutoPortaria", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoLocalizacao", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoGrupoTratamento", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProdutoCodigoMedicamento", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ProdutoCodigoMedicamento", "Codigo", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ProdutoAcaoTerapeutica", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.Parentesco", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.MotivoTransferenciaLeito", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.MotivoCaucao", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.MotivoCancelamento", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.LeitoServico", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.LeitoCaracteristica", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.InstituicaoTransferencia", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.Indicacao", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.GrupoProcedimento", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.GrupoCID", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.GrupoCID", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.GrauInstrucao", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Feriado", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Terceirizado", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.FatContaKit", "Descricao", c => c.String(maxLength: 100));
            AlterColumn("dbo.TipoLeito", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.FatTabela", "Descricao", c => c.String(maxLength: 100));
            AlterColumn("dbo.FatBrasApresentacao", "Descricao", c => c.String(maxLength: 100));
            AlterColumn("dbo.FatBrasLaboratorio", "Descricao", c => c.String(maxLength: 100));
            AlterColumn("dbo.AssTipoResposta", "Descricao", c => c.String(maxLength: 60));
            AlterColumn("dbo.ControleProducao", "Descricao", c => c.String(maxLength: 500));
            AlterColumn("dbo.ConsultorTabelaCampo", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.ConsultorTabela", "Descricao", c => c.String(maxLength: 500));
            AlterColumn("dbo.Cfop", "Descricao", c => c.String(maxLength: 350));
            AlterColumn("dbo.CapituloCID", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.FatTipoGrupo", "Descricao", c => c.String(maxLength: 100));
            AlterColumn("dbo.FatBrasItem", "Descricao", c => c.String(maxLength: 100));
            AlterColumn("dbo.SisTipoPrestador", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisPrestador", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisTipoSanguineo", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisOrigem", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AssMotivoAltaTipoAlta", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.AssMotivoAlta", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.SisTipoTabelaDominio", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.SisGrupoTipoTabelaDominio", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.SisTabelaDominio", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.LeitoStatus", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.TipoVinculoEmpregaticio", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisTipoParticipacao", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisConselho", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AteUnidadeInternacaoTipo", "Descricao", c => c.String(maxLength: 255));
            AlterColumn("dbo.GrupoCentroCusto", "Descricao", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CentroCusto", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisUnidadeOrganizacional", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisReligiao", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisProfissao", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisNaturalidade", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.SisNacionalidade", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.AteTipoAtendimento", "Descricao", c => c.String(maxLength: 255));
            DropColumn("dbo.TipoPessoa", "CreatorUserId");
            DropColumn("dbo.TipoPessoa", "CreationTime");
            DropColumn("dbo.TipoPessoa", "LastModifierUserId");
            DropColumn("dbo.TipoPessoa", "LastModificationTime");
            DropColumn("dbo.TipoPessoa", "DeletionTime");
            DropColumn("dbo.TipoPessoa", "DeleterUserId");
            DropColumn("dbo.TipoPessoa", "IsDeleted");
            DropColumn("dbo.TipoPessoa", "Codigo");
            DropColumn("dbo.TipoPessoa", "IsSistema");
            DropColumn("dbo.SisTipoCadastroExistente", "CreatorUserId");
            DropColumn("dbo.SisTipoCadastroExistente", "CreationTime");
            DropColumn("dbo.SisTipoCadastroExistente", "LastModifierUserId");
            DropColumn("dbo.SisTipoCadastroExistente", "LastModificationTime");
            DropColumn("dbo.SisTipoCadastroExistente", "DeletionTime");
            DropColumn("dbo.SisTipoCadastroExistente", "DeleterUserId");
            DropColumn("dbo.SisTipoCadastroExistente", "IsDeleted");
            DropColumn("dbo.SisTipoCadastroExistente", "Codigo");
            DropColumn("dbo.SisTipoCadastroExistente", "IsSistema");
            AlterTableAnnotations(
                "dbo.TipoPessoa",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_TipoPessoa_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AlterTableAnnotations(
                "dbo.SisTipoCadastroExistente",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_TipoCadastroExistente_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            RenameTable(name: "dbo.SisTipoCadastroExistente", newName: "TipoCadastroExistente");
        }
    }
}
