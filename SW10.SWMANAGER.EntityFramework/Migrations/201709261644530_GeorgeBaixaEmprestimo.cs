namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeBaixaEmprestimo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstBaixaEmprestimoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstBaixaEmprestimoId = c.Long(nullable: false),
                    EstBaixaMovimentoItemEntradaId = c.Long(nullable: false),
                    EstBaixaMovimentoItemSaidaId = c.Long(nullable: false),
                    QuantidadeBaixa = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_EstoqueBaixaEmprestimoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstBaixaEmprestimo", t => t.EstBaixaEmprestimoId, cascadeDelete: false)
                .ForeignKey("dbo.EstoqueMovimentoItem", t => t.EstBaixaMovimentoItemEntradaId, cascadeDelete: false)
                .ForeignKey("dbo.EstoqueMovimentoItem", t => t.EstBaixaMovimentoItemSaidaId, cascadeDelete: false)
                .Index(t => t.EstBaixaEmprestimoId)
                .Index(t => t.EstBaixaMovimentoItemEntradaId)
                .Index(t => t.EstBaixaMovimentoItemSaidaId);

            CreateTable(
                "dbo.EstBaixaEmprestimo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Documento = c.String(),
                    FornecedorId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
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
                    { "DynamicFilter_EstoqueBaixaEmprestimo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstBaixaEmprestimoItem", "EstBaixaMovimentoItemSaidaId", "dbo.EstoqueMovimentoItem");
            DropForeignKey("dbo.EstBaixaEmprestimoItem", "EstBaixaMovimentoItemEntradaId", "dbo.EstoqueMovimentoItem");
            DropForeignKey("dbo.EstBaixaEmprestimoItem", "EstBaixaEmprestimoId", "dbo.EstBaixaEmprestimo");
            DropIndex("dbo.EstBaixaEmprestimoItem", new[] { "EstBaixaMovimentoItemSaidaId" });
            DropIndex("dbo.EstBaixaEmprestimoItem", new[] { "EstBaixaMovimentoItemEntradaId" });
            DropIndex("dbo.EstBaixaEmprestimoItem", new[] { "EstBaixaEmprestimoId" });
            DropTable("dbo.EstBaixaEmprestimo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueBaixaEmprestimo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstBaixaEmprestimoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueBaixaEmprestimoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
