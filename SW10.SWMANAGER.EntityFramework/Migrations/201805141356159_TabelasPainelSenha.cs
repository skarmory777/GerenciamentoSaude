namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class TabelasPainelSenha : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteFila",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NumeroInicial = c.Int(nullable: false),
                    NumeroFinal = c.Int(nullable: false),
                    IsZera = c.Boolean(nullable: false),
                    HoraZera = c.DateTime(),
                    IsAtivo = c.Boolean(nullable: false),
                    IsDomingo = c.Boolean(nullable: false),
                    IsSegunda = c.Boolean(nullable: false),
                    IsTerca = c.Boolean(nullable: false),
                    IsQuarta = c.Boolean(nullable: false),
                    IsQuinta = c.Boolean(nullable: false),
                    IsSexta = c.Boolean(nullable: false),
                    IsSabado = c.Boolean(nullable: false),
                    Cor = c.String(),
                    IsNaoImprimeSenha = c.Boolean(nullable: false),
                    TipoLocalChamadaInicialId = c.Long(),
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
                    { "DynamicFilter_Fila_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteTipoLocalChamada", t => t.TipoLocalChamadaInicialId)
                .Index(t => t.TipoLocalChamadaInicialId);

            CreateTable(
                "dbo.AteTipoLocalChamada",
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
                    { "DynamicFilter_TipoLocalChamada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteLocalChamada",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TipoLocalChamadaId = c.Long(),
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
                    { "DynamicFilter_LocalChamada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteTipoLocalChamada", t => t.TipoLocalChamadaId)
                .Index(t => t.TipoLocalChamadaId);

            CreateTable(
                "dbo.AteLocalChamadaFila",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FilaId = c.Long(),
                    LocalChamadaId = c.Long(),
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
                    { "DynamicFilter_LocalChamadaFila_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteFila", t => t.FilaId)
                .ForeignKey("dbo.AteLocalChamada", t => t.LocalChamadaId)
                .Index(t => t.FilaId)
                .Index(t => t.LocalChamadaId);

            CreateTable(
                "dbo.AtePainel",
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
                    { "DynamicFilter_Painel_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AtePainelTipoLocalChamada",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TipoLocalChamadaId = c.Long(),
                    PainelId = c.Long(),
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
                    { "DynamicFilter_PainelTipoLocalChamada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AtePainel", t => t.PainelId)
                .ForeignKey("dbo.AteTipoLocalChamada", t => t.TipoLocalChamadaId)
                .Index(t => t.TipoLocalChamadaId)
                .Index(t => t.PainelId);

            CreateTable(
                "dbo.AteSenha",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataHora = c.DateTime(nullable: false),
                    Numero = c.Int(nullable: false),
                    FilaId = c.Long(nullable: false),
                    AtendimentoId = c.Long(),
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
                    { "DynamicFilter_Senha_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId)
                .ForeignKey("dbo.AteFila", t => t.FilaId, cascadeDelete: true)
                .Index(t => t.FilaId)
                .Index(t => t.AtendimentoId);

            CreateTable(
                "dbo.AtSenhaMov",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SenhaId = c.Long(nullable: false),
                    LocalChamadaId = c.Long(),
                    TipoLocalChamadaId = c.Long(),
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
                    { "DynamicFilter_SenhaMovimentacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteLocalChamada", t => t.LocalChamadaId)
                .ForeignKey("dbo.AteSenha", t => t.SenhaId, cascadeDelete: true)
                .ForeignKey("dbo.AteTipoLocalChamada", t => t.TipoLocalChamadaId)
                .Index(t => t.SenhaId)
                .Index(t => t.LocalChamadaId)
                .Index(t => t.TipoLocalChamadaId);

            CreateTable(
                "dbo.AteSenhaMovPainel",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PainelId = c.Long(nullable: false),
                    SenhaMovimentacaoId = c.Long(nullable: false),
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
                    { "DynamicFilter_SenhaMovimentacaoPainel_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AtePainel", t => t.PainelId, cascadeDelete: true)
                .ForeignKey("dbo.AtSenhaMov", t => t.SenhaMovimentacaoId, cascadeDelete: true)
                .Index(t => t.PainelId)
                .Index(t => t.SenhaMovimentacaoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteSenhaMovPainel", "SenhaMovimentacaoId", "dbo.AtSenhaMov");
            DropForeignKey("dbo.AteSenhaMovPainel", "PainelId", "dbo.AtePainel");
            DropForeignKey("dbo.AtSenhaMov", "TipoLocalChamadaId", "dbo.AteTipoLocalChamada");
            DropForeignKey("dbo.AtSenhaMov", "SenhaId", "dbo.AteSenha");
            DropForeignKey("dbo.AtSenhaMov", "LocalChamadaId", "dbo.AteLocalChamada");
            DropForeignKey("dbo.AteSenha", "FilaId", "dbo.AteFila");
            DropForeignKey("dbo.AteSenha", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.AtePainelTipoLocalChamada", "TipoLocalChamadaId", "dbo.AteTipoLocalChamada");
            DropForeignKey("dbo.AtePainelTipoLocalChamada", "PainelId", "dbo.AtePainel");
            DropForeignKey("dbo.AteLocalChamadaFila", "LocalChamadaId", "dbo.AteLocalChamada");
            DropForeignKey("dbo.AteLocalChamadaFila", "FilaId", "dbo.AteFila");
            DropForeignKey("dbo.AteLocalChamada", "TipoLocalChamadaId", "dbo.AteTipoLocalChamada");
            DropForeignKey("dbo.AteFila", "TipoLocalChamadaInicialId", "dbo.AteTipoLocalChamada");
            DropIndex("dbo.AteSenhaMovPainel", new[] { "SenhaMovimentacaoId" });
            DropIndex("dbo.AteSenhaMovPainel", new[] { "PainelId" });
            DropIndex("dbo.AtSenhaMov", new[] { "TipoLocalChamadaId" });
            DropIndex("dbo.AtSenhaMov", new[] { "LocalChamadaId" });
            DropIndex("dbo.AtSenhaMov", new[] { "SenhaId" });
            DropIndex("dbo.AteSenha", new[] { "AtendimentoId" });
            DropIndex("dbo.AteSenha", new[] { "FilaId" });
            DropIndex("dbo.AtePainelTipoLocalChamada", new[] { "PainelId" });
            DropIndex("dbo.AtePainelTipoLocalChamada", new[] { "TipoLocalChamadaId" });
            DropIndex("dbo.AteLocalChamadaFila", new[] { "LocalChamadaId" });
            DropIndex("dbo.AteLocalChamadaFila", new[] { "FilaId" });
            DropIndex("dbo.AteLocalChamada", new[] { "TipoLocalChamadaId" });
            DropIndex("dbo.AteFila", new[] { "TipoLocalChamadaInicialId" });
            DropTable("dbo.AteSenhaMovPainel",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SenhaMovimentacaoPainel_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AtSenhaMov",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SenhaMovimentacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteSenha",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Senha_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AtePainelTipoLocalChamada",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PainelTipoLocalChamada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AtePainel",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Painel_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteLocalChamadaFila",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LocalChamadaFila_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteLocalChamada",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LocalChamada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteTipoLocalChamada",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLocalChamada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteFila",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Fila_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
