namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Imagem_laudo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LauMovItemStatus", newName: "LauMovimentoStatus");
            DropForeignKey("dbo.LauMovimentoItem", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.LauMovimentoItem", "LauMovItemStatusId", "dbo.LauMovItemStatus");
            DropForeignKey("dbo.LauMovimentoItem", "LeitoId", "dbo.Leito");
            DropIndex("dbo.LauMovimentoItem", "IX_LauMovimentoItem_Codigo");
            DropIndex("dbo.LauMovimentoItem", new[] { "LauMovItemStatusId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "ConvenioId" });
            DropIndex("dbo.LauMovimentoItem", new[] { "LeitoId" });
            RenameIndex(table: "dbo.LauMovimentoStatus", name: "IX_LauMovItemStatus_Codigo", newName: "IX_LauMovimentoStatus_Codigo");
            AlterTableAnnotations(
                "dbo.LauMovimentoStatus",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_LaudoMovimentoItemStatus_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    {
                        "DynamicFilter_LaudoMovimentoStatus_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AddColumn("dbo.LauMovimento", "LauMovimentoStatusId", c => c.Long(nullable: false));
            AddColumn("dbo.LauMovimento", "ConvenioId", c => c.Long());
            AddColumn("dbo.LauMovimento", "LeitoId", c => c.Long());
            AddColumn("dbo.LauMovimento", "IsContraste", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimento", "QtdeConstraste", c => c.String());
            AddColumn("dbo.LauMovimento", "Obs", c => c.String());
            AlterColumn("dbo.LauMovimentoItem", "TecnicoId", c => c.Long());
            CreateIndex("dbo.LauMovimento", "LauMovimentoStatusId");
            CreateIndex("dbo.LauMovimento", "ConvenioId");
            CreateIndex("dbo.LauMovimento", "LeitoId");
            AddForeignKey("dbo.LauMovimento", "ConvenioId", "dbo.Convenio", "Id");
            AddForeignKey("dbo.LauMovimento", "LauMovimentoStatusId", "dbo.LauMovimentoStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LauMovimento", "LeitoId", "dbo.Leito", "Id");
            DropColumn("dbo.LauMovimentoItem", "LauMovItemStatusId");
            DropColumn("dbo.LauMovimentoItem", "ConvenioId");
            DropColumn("dbo.LauMovimentoItem", "LeitoId");
            DropColumn("dbo.LauMovimentoItem", "IsContraste");
            DropColumn("dbo.LauMovimentoItem", "QtdeConstraste");
            DropColumn("dbo.LauMovimentoItem", "Obs");
        }

        public override void Down()
        {
            AddColumn("dbo.LauMovimentoItem", "Obs", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "QtdeConstraste", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "IsContraste", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "LeitoId", c => c.Long());
            AddColumn("dbo.LauMovimentoItem", "ConvenioId", c => c.Long());
            AddColumn("dbo.LauMovimentoItem", "LauMovItemStatusId", c => c.Long(nullable: false));
            DropForeignKey("dbo.LauMovimento", "LeitoId", "dbo.Leito");
            DropForeignKey("dbo.LauMovimento", "LauMovimentoStatusId", "dbo.LauMovimentoStatus");
            DropForeignKey("dbo.LauMovimento", "ConvenioId", "dbo.Convenio");
            DropIndex("dbo.LauMovimento", new[] { "LeitoId" });
            DropIndex("dbo.LauMovimento", new[] { "ConvenioId" });
            DropIndex("dbo.LauMovimento", new[] { "LauMovimentoStatusId" });
            AlterColumn("dbo.LauMovimentoItem", "TecnicoId", c => c.Long(nullable: false));
            DropColumn("dbo.LauMovimento", "Obs");
            DropColumn("dbo.LauMovimento", "QtdeConstraste");
            DropColumn("dbo.LauMovimento", "IsContraste");
            DropColumn("dbo.LauMovimento", "LeitoId");
            DropColumn("dbo.LauMovimento", "ConvenioId");
            DropColumn("dbo.LauMovimento", "LauMovimentoStatusId");
            AlterTableAnnotations(
                "dbo.LauMovimentoStatus",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_LaudoMovimentoItemStatus_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    {
                        "DynamicFilter_LaudoMovimentoStatus_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            RenameIndex(table: "dbo.LauMovimentoStatus", name: "IX_LauMovimentoStatus_Codigo", newName: "IX_LauMovItemStatus_Codigo");
            CreateIndex("dbo.LauMovimentoItem", "LeitoId");
            CreateIndex("dbo.LauMovimentoItem", "ConvenioId");
            CreateIndex("dbo.LauMovimentoItem", "LauMovItemStatusId");
            CreateIndex("dbo.LauMovimentoItem", "Codigo", name: "IX_LauMovimentoItem_Codigo");
            AddForeignKey("dbo.LauMovimentoItem", "LeitoId", "dbo.Leito", "Id");
            AddForeignKey("dbo.LauMovimentoItem", "LauMovItemStatusId", "dbo.LauMovItemStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LauMovimentoItem", "ConvenioId", "dbo.Convenio", "Id");
            RenameTable(name: "dbo.LauMovimentoStatus", newName: "LauMovItemStatus");
        }
    }
}
