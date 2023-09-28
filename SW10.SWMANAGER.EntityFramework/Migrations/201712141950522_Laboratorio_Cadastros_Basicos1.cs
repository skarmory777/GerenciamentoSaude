namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Laboratorio_Cadastros_Basicos1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Coleta", "TecnicoId", "dbo.LabTecnico");
            DropForeignKey("dbo.LabInformacao", "EquipamentoId", "dbo.LabEquipamento");
            DropForeignKey("dbo.LabEquipamento", "Informacao_Id1", "dbo.LabInformacao");
            DropForeignKey("dbo.LabInformacao", "UnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabInformacao", "TipoResultadoId", "dbo.LabTipoResultado");
            DropForeignKey("dbo.LabColetaExameInformacao", "LabUnidadeId", "dbo.LabUnidade");
            DropIndex("dbo.LabColetaExameInformacao", new[] { "LabUnidadeId" });
            DropIndex("dbo.Coleta", new[] { "TecnicoId" });
            DropIndex("dbo.LabInformacao", new[] { "UnidadeId" });
            DropIndex("dbo.LabInformacao", new[] { "TipoResultadoId" });
            DropIndex("dbo.LabInformacao", new[] { "EquipamentoId" });
            DropIndex("dbo.LabEquipamento", new[] { "Informacao_Id1" });
            RenameColumn(table: "dbo.LabInformacao", name: "TabelaResultadoId", newName: "TabelaResultado_Id");
            RenameIndex(table: "dbo.LabInformacao", name: "IX_TabelaResultadoId", newName: "IX_TabelaResultado_Id");
            CreateTable(
                "dbo.LabSetor",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    OrdemSetor = c.Int(nullable: false),
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
                    { "DynamicFilter_Setor_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AlterTableAnnotations(
                "dbo.LabUnidade",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_LaboratorioUnidade_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    {
                        "DynamicFilter_UnidadeLaboratorio_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AddColumn("dbo.LabTecnico", "RegConselho", c => c.String());
            AddColumn("dbo.LabMaterial", "Ordem", c => c.Int(nullable: false));
            AddColumn("dbo.LabEquipamento", "TipoLayout", c => c.Int(nullable: false));
            AlterColumn("dbo.LabMaterial", "Descricao", c => c.String());
            AlterColumn("dbo.LabKitExame", "Descricao", c => c.String());
            DropColumn("dbo.LabTecnico", "Nome");
            DropColumn("dbo.LabInformacao", "UnidadeId");
            DropColumn("dbo.LabInformacao", "TipoResultadoId");
            DropColumn("dbo.LabInformacao", "EquipamentoId");
            DropColumn("dbo.LabEquipamento", "Informacao_Id1");
            DropColumn("dbo.LabKitExame", "IsAtivo");
        }

        public override void Down()
        {
            AddColumn("dbo.LabKitExame", "IsAtivo", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabEquipamento", "Informacao_Id1", c => c.Long());
            AddColumn("dbo.LabInformacao", "EquipamentoId", c => c.Long());
            AddColumn("dbo.LabInformacao", "TipoResultadoId", c => c.Long());
            AddColumn("dbo.LabInformacao", "UnidadeId", c => c.Long(nullable: false));
            AddColumn("dbo.LabTecnico", "Nome", c => c.String());
            AlterColumn("dbo.LabKitExame", "Descricao", c => c.String(maxLength: 50));
            AlterColumn("dbo.LabMaterial", "Descricao", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.LabEquipamento", "TipoLayout");
            DropColumn("dbo.LabMaterial", "Ordem");
            DropColumn("dbo.LabTecnico", "RegConselho");
            AlterTableAnnotations(
                "dbo.LabUnidade",
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_LaboratorioUnidade_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    {
                        "DynamicFilter_UnidadeLaboratorio_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            DropTable("dbo.LabSetor",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Setor_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameIndex(table: "dbo.LabInformacao", name: "IX_TabelaResultado_Id", newName: "IX_TabelaResultadoId");
            RenameColumn(table: "dbo.LabInformacao", name: "TabelaResultado_Id", newName: "TabelaResultadoId");
            CreateIndex("dbo.LabEquipamento", "Informacao_Id1");
            CreateIndex("dbo.LabInformacao", "EquipamentoId");
            CreateIndex("dbo.LabInformacao", "TipoResultadoId");
            CreateIndex("dbo.LabInformacao", "UnidadeId");
            CreateIndex("dbo.Coleta", "TecnicoId");
            CreateIndex("dbo.LabColetaExameInformacao", "LabUnidadeId");
            AddForeignKey("dbo.LabColetaExameInformacao", "LabUnidadeId", "dbo.LabUnidade", "Id");
            AddForeignKey("dbo.LabInformacao", "TipoResultadoId", "dbo.LabTipoResultado", "Id");
            AddForeignKey("dbo.LabInformacao", "UnidadeId", "dbo.LabUnidade", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LabEquipamento", "Informacao_Id1", "dbo.LabInformacao", "Id");
            AddForeignKey("dbo.LabInformacao", "EquipamentoId", "dbo.LabEquipamento", "Id");
            AddForeignKey("dbo.Coleta", "TecnicoId", "dbo.LabTecnico", "Id");
        }
    }
}
