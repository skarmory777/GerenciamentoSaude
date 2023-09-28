namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Visitante_FatConta_FatContaItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteVisitante",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                    Documento = c.String(),
                    IsAcompanhante = c.Boolean(nullable: false),
                    IsVisitandte = c.Boolean(nullable: false),
                    IsMedico = c.Boolean(nullable: false),
                    DataEntrada = c.DateTime(nullable: false),
                    DataSaida = c.DateTime(nullable: false),
                    UnidadeOrganizacionalId = c.Long(),
                    AtendimentoId = c.Long(),
                    FornedcedorId = c.Long(),
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
                    { "DynamicFilter_Visitante_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId)
                .ForeignKey("dbo.Fornecedor", t => t.FornedcedorId)
                .ForeignKey("dbo.UnidadeOrganizacional", t => t.UnidadeOrganizacionalId)
                .Index(t => t.UnidadeOrganizacionalId)
                .Index(t => t.AtendimentoId)
                .Index(t => t.FornedcedorId);

            CreateTable(
                "dbo.FatContaItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 100),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoContaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FaturamentoContaItemFaturamentoContas",
                c => new
                {
                    FaturamentoContaItem_Id = c.Long(nullable: false),
                    FaturamentoConta_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.FaturamentoContaItem_Id, t.FaturamentoConta_Id })
                .ForeignKey("dbo.FatContaItem", t => t.FaturamentoContaItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.FatConta", t => t.FaturamentoConta_Id, cascadeDelete: true)
                .Index(t => t.FaturamentoContaItem_Id)
                .Index(t => t.FaturamentoConta_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoConta_Id", "dbo.FatConta");
            DropForeignKey("dbo.FaturamentoContaItemFaturamentoContas", "FaturamentoContaItem_Id", "dbo.FatContaItem");
            DropForeignKey("dbo.AteVisitante", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.AteVisitante", "FornedcedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.AteVisitante", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.FaturamentoContaItemFaturamentoContas", new[] { "FaturamentoConta_Id" });
            DropIndex("dbo.FaturamentoContaItemFaturamentoContas", new[] { "FaturamentoContaItem_Id" });
            DropIndex("dbo.AteVisitante", new[] { "FornedcedorId" });
            DropIndex("dbo.AteVisitante", new[] { "AtendimentoId" });
            DropIndex("dbo.AteVisitante", new[] { "UnidadeOrganizacionalId" });
            DropTable("dbo.FaturamentoContaItemFaturamentoContas");
            DropTable("dbo.FatContaItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoContaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteVisitante",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Visitante_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
