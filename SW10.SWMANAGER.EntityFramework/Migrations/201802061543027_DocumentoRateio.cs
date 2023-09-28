namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class DocumentoRateio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinDocumentoRateio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DocumentoId = c.Long(),
                    CentroCustoId = c.Long(nullable: false),
                    ContaAdministrativaId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    Valor = c.Decimal(precision: 18, scale: 2),
                    IsCredito = c.Boolean(nullable: false),
                    Observacao = c.String(),
                    IsImposto = c.Boolean(nullable: false),
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
                    { "DynamicFilter_DocumentoRateio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId, cascadeDelete: true)
                .ForeignKey("dbo.FinContaAdministrativa", t => t.ContaAdministrativaId, cascadeDelete: true)
                .ForeignKey("dbo.FinDocumento", t => t.DocumentoId)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .Index(t => t.DocumentoId)
                .Index(t => t.CentroCustoId)
                .Index(t => t.ContaAdministrativaId)
                .Index(t => t.EmpresaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FinDocumentoRateio", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FinDocumentoRateio", "DocumentoId", "dbo.FinDocumento");
            DropForeignKey("dbo.FinDocumentoRateio", "ContaAdministrativaId", "dbo.FinContaAdministrativa");
            DropForeignKey("dbo.FinDocumentoRateio", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.FinDocumentoRateio", new[] { "EmpresaId" });
            DropIndex("dbo.FinDocumentoRateio", new[] { "ContaAdministrativaId" });
            DropIndex("dbo.FinDocumentoRateio", new[] { "CentroCustoId" });
            DropIndex("dbo.FinDocumentoRateio", new[] { "DocumentoId" });
            DropTable("dbo.FinDocumentoRateio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DocumentoRateio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
