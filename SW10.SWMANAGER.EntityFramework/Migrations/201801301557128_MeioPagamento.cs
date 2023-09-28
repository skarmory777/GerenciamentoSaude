namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class MeioPagamento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinContaAdministrativaEmpresa", "EnpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.FinContaAdministrativaEmpresa", new[] { "EnpresaId" });
            RenameColumn(table: "dbo.FinContaAdministrativaEmpresa", name: "EnpresaId", newName: "EmpresaId");
            CreateTable(
                "dbo.FinMeioPagamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DiasRetencaoDebito = c.Int(),
                    DiasRetencaoCredito = c.Int(),
                    TaxaAdministracao = c.Decimal(precision: 18, scale: 2),
                    MascaraCredito = c.String(),
                    MascaraDebito = c.String(),
                    DescricaoMascaraCredito = c.String(),
                    DescricaoMascaraDebito = c.String(),
                    IsNumeroDocumentoObrigatorio = c.Boolean(nullable: false),
                    IsPagamentoEletronico = c.Boolean(nullable: false),
                    TipoMeioPagamentoId = c.Long(nullable: false),
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
                    { "DynamicFilter_MeioPagamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinTipoMeioPagamento", t => t.TipoMeioPagamentoId, cascadeDelete: true)
                .Index(t => t.TipoMeioPagamentoId);

            CreateTable(
                "dbo.FinTipoMeioPagamento",
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
                    { "DynamicFilter_TipoMeioPagamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AlterColumn("dbo.FinContaAdministrativaEmpresa", "EmpresaId", c => c.Long());
            CreateIndex("dbo.FinContaAdministrativaEmpresa", "EmpresaId");
            AddForeignKey("dbo.FinContaAdministrativaEmpresa", "EmpresaId", "dbo.SisEmpresa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinContaAdministrativaEmpresa", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FinMeioPagamento", "TipoMeioPagamentoId", "dbo.FinTipoMeioPagamento");
            DropIndex("dbo.FinMeioPagamento", new[] { "TipoMeioPagamentoId" });
            DropIndex("dbo.FinContaAdministrativaEmpresa", new[] { "EmpresaId" });
            AlterColumn("dbo.FinContaAdministrativaEmpresa", "EmpresaId", c => c.Long(nullable: false));
            DropTable("dbo.FinTipoMeioPagamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoMeioPagamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinMeioPagamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MeioPagamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameColumn(table: "dbo.FinContaAdministrativaEmpresa", name: "EmpresaId", newName: "EnpresaId");
            CreateIndex("dbo.FinContaAdministrativaEmpresa", "EnpresaId");
            AddForeignKey("dbo.FinContaAdministrativaEmpresa", "EnpresaId", "dbo.SisEmpresa", "Id", cascadeDelete: true);
        }
    }
}
