namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class DocumentoFinanceiro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinDocumento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TipoDocumentoId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    ForncedorId = c.Long(),
                    Numero = c.String(),
                    DataEmissao = c.DateTime(),
                    IsCredito = c.Boolean(nullable: false),
                    ValorDocumento = c.Decimal(precision: 18, scale: 2),
                    ValorAcrescimoDecrescimo = c.Decimal(precision: 18, scale: 2),
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
                    { "DynamicFilter_Documento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.SisFornecedor", t => t.ForncedorId)
                .ForeignKey("dbo.FinTipoDocumento", t => t.TipoDocumentoId, cascadeDelete: true)
                .Index(t => t.TipoDocumentoId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ForncedorId);

            CreateTable(
                "dbo.FinTipoDocumento",
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
                    { "DynamicFilter_TipoDocumento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FinLancamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DocumentoId = c.Long(nullable: false),
                    DataVencimento = c.DateTime(),
                    ValorLancamento = c.Decimal(precision: 18, scale: 2),
                    ValorAcrescimoDecrescimo = c.Decimal(precision: 18, scale: 2),
                    Juros = c.Decimal(precision: 18, scale: 2),
                    Multa = c.Decimal(precision: 18, scale: 2),
                    SituacaoLancamentoId = c.Long(nullable: false),
                    DocumentoRelacionadoId = c.Long(),
                    MesCompetencia = c.Int(),
                    AnoCompetencia = c.Int(),
                    IsCredito = c.Boolean(nullable: false),
                    DataLancamento = c.DateTime(),
                    Parcela = c.Int(nullable: false),
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
                    { "DynamicFilter_Lancamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinDocumento", t => t.DocumentoId, cascadeDelete: true)
                .ForeignKey("dbo.FinDocumento", t => t.DocumentoRelacionadoId)
                .ForeignKey("dbo.FinSituacaoLancamento", t => t.SituacaoLancamentoId, cascadeDelete: true)
                .Index(t => t.DocumentoId)
                .Index(t => t.SituacaoLancamentoId)
                .Index(t => t.DocumentoRelacionadoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FinLancamento", "SituacaoLancamentoId", "dbo.FinSituacaoLancamento");
            DropForeignKey("dbo.FinLancamento", "DocumentoRelacionadoId", "dbo.FinDocumento");
            DropForeignKey("dbo.FinLancamento", "DocumentoId", "dbo.FinDocumento");
            DropForeignKey("dbo.FinDocumento", "TipoDocumentoId", "dbo.FinTipoDocumento");
            DropForeignKey("dbo.FinDocumento", "ForncedorId", "dbo.SisFornecedor");
            DropForeignKey("dbo.FinDocumento", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.FinLancamento", new[] { "DocumentoRelacionadoId" });
            DropIndex("dbo.FinLancamento", new[] { "SituacaoLancamentoId" });
            DropIndex("dbo.FinLancamento", new[] { "DocumentoId" });
            DropIndex("dbo.FinDocumento", new[] { "ForncedorId" });
            DropIndex("dbo.FinDocumento", new[] { "EmpresaId" });
            DropIndex("dbo.FinDocumento", new[] { "TipoDocumentoId" });
            DropTable("dbo.FinLancamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Lancamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinTipoDocumento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoDocumento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinDocumento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Documento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
