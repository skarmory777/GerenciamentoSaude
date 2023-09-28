namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class reformulate_laboratorio_do_suprimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProdutoLaboratorio", "FornecedorId", "dbo.Fornecedor");
            RenameTable(name: "dbo.ProdutoLaboratorio", newName: "EstLaboratorio");
            DropIndex("dbo.EstLaboratorio", new[] { "FornecedorId" });
            RenameColumn(table: "dbo.LoteValidade", name: "ProdutoLaboratorioId", newName: "EstEstoqueLaboratorioId");
            RenameIndex(table: "dbo.LoteValidade", name: "IX_ProdutoLaboratorioId", newName: "IX_EstEstoqueLaboratorioId");
            AlterTableAnnotations(
                "dbo.EstLaboratorio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatBrasLaboratorioId = c.Long(),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_EstoqueLaboratorio_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    {
                        "DynamicFilter_ProdutoLaboratorio_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AddColumn("dbo.EstLaboratorio", "FatBrasLaboratorioId", c => c.Long());
            CreateIndex("dbo.EstLaboratorio", "FatBrasLaboratorioId");
            AddForeignKey("dbo.EstLaboratorio", "FatBrasLaboratorioId", "dbo.FatBrasLaboratorio", "Id");
            DropColumn("dbo.EstLaboratorio", "FornecedorId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstLaboratorio", "FornecedorId", c => c.Long(nullable: false));
            DropForeignKey("dbo.EstLaboratorio", "FatBrasLaboratorioId", "dbo.FatBrasLaboratorio");
            DropIndex("dbo.EstLaboratorio", new[] { "FatBrasLaboratorioId" });
            DropColumn("dbo.EstLaboratorio", "FatBrasLaboratorioId");
            AlterTableAnnotations(
                "dbo.EstLaboratorio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatBrasLaboratorioId = c.Long(),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_EstoqueLaboratorio_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    {
                        "DynamicFilter_ProdutoLaboratorio_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            RenameIndex(table: "dbo.LoteValidade", name: "IX_EstEstoqueLaboratorioId", newName: "IX_ProdutoLaboratorioId");
            RenameColumn(table: "dbo.LoteValidade", name: "EstEstoqueLaboratorioId", newName: "ProdutoLaboratorioId");
            CreateIndex("dbo.EstLaboratorio", "FornecedorId");
            RenameTable(name: "dbo.EstLaboratorio", newName: "ProdutoLaboratorio");
            AddForeignKey("dbo.ProdutoLaboratorio", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }
    }
}
