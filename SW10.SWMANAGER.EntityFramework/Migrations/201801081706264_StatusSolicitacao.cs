namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class StatusSolicitacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteStatusSolicitacaoProcedimento",
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
                    { "DynamicFilter_StatusSolicitacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeSolicitada", c => c.Int(nullable: false));
            AddColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeAutorizada", c => c.Int(nullable: false));
            AddColumn("dbo.AteAutorizacaoProcedimentoItem", "StatusId", c => c.Long());
            AddColumn("dbo.AteAutorizacaoProcedimentoItem", "Observacao", c => c.String());
            CreateIndex("dbo.AteAutorizacaoProcedimentoItem", "StatusId");
            AddForeignKey("dbo.AteAutorizacaoProcedimentoItem", "StatusId", "dbo.AteStatusSolicitacaoProcedimento", "Id");
            DropColumn("dbo.AteAutorizacaoProcedimentoItem", "Quantidade");
            DropColumn("dbo.AteAutorizacaoProcedimentoItem", "Status");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAutorizacaoProcedimentoItem", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.AteAutorizacaoProcedimentoItem", "Quantidade", c => c.Int(nullable: false));
            DropForeignKey("dbo.AteAutorizacaoProcedimentoItem", "StatusId", "dbo.AteStatusSolicitacaoProcedimento");
            DropIndex("dbo.AteAutorizacaoProcedimentoItem", new[] { "StatusId" });
            DropColumn("dbo.AteAutorizacaoProcedimentoItem", "Observacao");
            DropColumn("dbo.AteAutorizacaoProcedimentoItem", "StatusId");
            DropColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeAutorizada");
            DropColumn("dbo.AteAutorizacaoProcedimentoItem", "QuantidadeSolicitada");
            DropTable("dbo.AteStatusSolicitacaoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_StatusSolicitacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
