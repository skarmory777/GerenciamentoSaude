namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class TabelasQuitacaoBancos : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Banco", newName: "FinBanco");
            CreateTable(
                "dbo.FinAgencia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    BancoId = c.Long(nullable: false),
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
                    { "DynamicFilter_Agencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinBanco", t => t.BancoId, cascadeDelete: true)
                .Index(t => t.BancoId);

            CreateTable(
                "dbo.FinContaCorrente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TipoContaCorrenteId = c.Long(nullable: false),
                    AgenciaId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    DataAbertura = c.DateTime(nullable: false),
                    NomeGerente = c.String(),
                    LimiteCredito = c.Decimal(precision: 18, scale: 2),
                    Observacao = c.String(),
                    IsContaNaoOperacional = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ContaCorrente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinAgencia", t => t.AgenciaId, cascadeDelete: true)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.FinTipoContaCorrente", t => t.TipoContaCorrenteId, cascadeDelete: true)
                .Index(t => t.TipoContaCorrenteId)
                .Index(t => t.AgenciaId)
                .Index(t => t.EmpresaId);

            CreateTable(
                "dbo.FinTipoContaCorrente",
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
                    { "DynamicFilter_TipoContaCorrente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FinLancamentoQuitacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    LancamentoId = c.Long(nullable: false),
                    QuitacaoId = c.Long(nullable: false),
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
                    { "DynamicFilter_LancamentoQuitacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinLancamento", t => t.LancamentoId, cascadeDelete: true)
                .ForeignKey("dbo.FinQuitacao", t => t.QuitacaoId, cascadeDelete: true)
                .Index(t => t.LancamentoId)
                .Index(t => t.QuitacaoId);

            CreateTable(
                "dbo.FinQuitacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ContaCorrenteId = c.Long(nullable: false),
                    MeioPagamentoId = c.Long(nullable: false),
                    Numero = c.String(),
                    DataMovimento = c.DateTime(),
                    DataCompensado = c.DateTime(),
                    DataConsolidado = c.DateTime(),
                    Desconto = c.Decimal(precision: 18, scale: 2),
                    Acrescimo = c.Decimal(precision: 18, scale: 2),
                    MoraMulta = c.Decimal(precision: 18, scale: 2),
                    Observacao = c.String(),
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
                    { "DynamicFilter_Quitacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinContaCorrente", t => t.ContaCorrenteId, cascadeDelete: true)
                .ForeignKey("dbo.FinMeioPagamento", t => t.MeioPagamentoId, cascadeDelete: true)
                .Index(t => t.ContaCorrenteId)
                .Index(t => t.MeioPagamentoId);

            AlterColumn("dbo.FinBanco", "Codigo", c => c.String(maxLength: 10));
            AlterColumn("dbo.FinBanco", "Descricao", c => c.String());
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinLancamentoQuitacao", "QuitacaoId", "dbo.FinQuitacao");
            DropForeignKey("dbo.FinQuitacao", "MeioPagamentoId", "dbo.FinMeioPagamento");
            DropForeignKey("dbo.FinQuitacao", "ContaCorrenteId", "dbo.FinContaCorrente");
            DropForeignKey("dbo.FinLancamentoQuitacao", "LancamentoId", "dbo.FinLancamento");
            DropForeignKey("dbo.FinContaCorrente", "TipoContaCorrenteId", "dbo.FinTipoContaCorrente");
            DropForeignKey("dbo.FinContaCorrente", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FinContaCorrente", "AgenciaId", "dbo.FinAgencia");
            DropForeignKey("dbo.FinAgencia", "BancoId", "dbo.FinBanco");
            DropIndex("dbo.FinQuitacao", new[] { "MeioPagamentoId" });
            DropIndex("dbo.FinQuitacao", new[] { "ContaCorrenteId" });
            DropIndex("dbo.FinLancamentoQuitacao", new[] { "QuitacaoId" });
            DropIndex("dbo.FinLancamentoQuitacao", new[] { "LancamentoId" });
            DropIndex("dbo.FinContaCorrente", new[] { "EmpresaId" });
            DropIndex("dbo.FinContaCorrente", new[] { "AgenciaId" });
            DropIndex("dbo.FinContaCorrente", new[] { "TipoContaCorrenteId" });
            DropIndex("dbo.FinAgencia", new[] { "BancoId" });
            AlterColumn("dbo.FinBanco", "Descricao", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.FinBanco", "Codigo", c => c.Int(nullable: false));
            DropTable("dbo.FinQuitacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Quitacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinLancamentoQuitacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LancamentoQuitacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinTipoContaCorrente",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoContaCorrente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinContaCorrente",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContaCorrente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinAgencia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Agencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameTable(name: "dbo.FinBanco", newName: "Banco");
        }
    }
}
