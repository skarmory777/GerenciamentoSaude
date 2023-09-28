namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Imagens_Laudos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LauMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    AtendimentoId = c.Long(nullable: false),
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
                    { "DynamicFilter_LaudoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId, cascadeDelete: false)
                .Index(t => t.Codigo, name: "IX_LauMovimento_Codigo")
                .Index(t => t.AtendimentoId);

            CreateTable(
                "dbo.LauMovimentoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    LauMovimentoId = c.Long(nullable: false),
                    FatItemId = c.Long(nullable: false),
                    AssSolicitacaoExameItemId = c.Long(),
                    LauMovItemStatusId = c.Long(nullable: false),
                    ConvenioId = c.Long(),
                    LeitoId = c.Long(),
                    TecnicoId = c.Long(nullable: false),
                    IsContraste = c.Boolean(nullable: false),
                    QtdeConstraste = c.String(),
                    Obs = c.String(),
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
                    { "DynamicFilter_LaudoMovimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.FatItem", t => t.FatItemId, cascadeDelete: false)
                .ForeignKey("dbo.LauMovimento", t => t.LauMovimentoId, cascadeDelete: false)
                .ForeignKey("dbo.LauMovItemStatus", t => t.LauMovItemStatusId, cascadeDelete: false)
                .ForeignKey("dbo.Leito", t => t.LeitoId)
                .ForeignKey("dbo.AssSolicitacaoExameItem", t => t.AssSolicitacaoExameItemId)
                .Index(t => t.Codigo, name: "IX_LauMovimentoItem_Codigo")
                .Index(t => t.LauMovimentoId)
                .Index(t => t.FatItemId)
                .Index(t => t.AssSolicitacaoExameItemId)
                .Index(t => t.LauMovItemStatusId)
                .Index(t => t.ConvenioId)
                .Index(t => t.LeitoId);

            CreateTable(
                "dbo.LauMovItemStatus",
                c => new
                {
                    Id = c.Long(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_LaudoMovimentoItemStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Codigo, name: "IX_LauMovItemStatus_Codigo");

            CreateTable(
                "dbo.LauModalidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsParecer = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Modalidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Codigo, name: "IX_LauModalidade_Codigo");

        }

        public override void Down()
        {
            DropForeignKey("dbo.LauMovimentoItem", "AssSolicitacaoExameItemId", "dbo.AssSolicitacaoExameItem");
            DropForeignKey("dbo.LauMovimentoItem", "LeitoId", "dbo.Leito");
            DropForeignKey("dbo.LauMovimentoItem", "LauMovItemStatusId", "dbo.LauMovItemStatus");
            DropForeignKey("dbo.LauMovimentoItem", "LauMovimentoId", "dbo.LauMovimento");
            DropForeignKey("dbo.LauMovimentoItem", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.LauMovimentoItem", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.LauMovimento", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.LauModalidade", "IX_LauModalidade_Codigo");
            DropIndex("dbo.LauMovItemStatus", "IX_LauMovItemStatus_Codigo");
            DropIndex("dbo.LauMovimentoItem", new[] { "LeitoId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "ConvenioId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "LauMovItemStatusId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "AssSolicitacaoExameItemId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "FatItemId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "LauMovimentoId" });
            DropIndex("dbo.LauMovimentoItem", "IX_LauMovimentoItem_Codigo");
            DropIndex("dbo.LauMovimento", new[] { "AtendimentoId" });
            DropIndex("dbo.LauMovimento", "IX_LauMovimento_Codigo");
            DropTable("dbo.LauModalidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Modalidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LauMovItemStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaudoMovimentoItemStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LauMovimentoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaudoMovimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LauMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LaudoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
