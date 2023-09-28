namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Laboratorio_Todas_tabelas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabTabela",
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
                    { "DynamicFilter_Tabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabExame",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsExameSimples = c.Boolean(nullable: false),
                    IsPeso = c.Boolean(nullable: false),
                    IsTesta100 = c.Boolean(nullable: false),
                    IsAltura = c.Boolean(nullable: false),
                    IsCor = c.Boolean(nullable: false),
                    IsMestruacao = c.Boolean(nullable: false),
                    IsNacionalidade = c.Boolean(nullable: false),
                    IsNaturalidade = c.Boolean(nullable: false),
                    IsImpReferencia = c.Boolean(nullable: false),
                    IsCultura = c.Boolean(nullable: false),
                    IsPendente = c.Boolean(nullable: false),
                    IsRepete = c.Boolean(nullable: false),
                    IsLibera = c.Boolean(nullable: false),
                    Mneumonico = c.String(),
                    OrdemImp = c.Int(),
                    Prazo = c.Int(),
                    Interpretacao = c.Binary(),
                    Extra1 = c.Binary(),
                    Extra2 = c.Binary(),
                    Referencia = c.String(),
                    QtdFatura = c.Int(),
                    MapaExame = c.String(),
                    OrdemResul = c.Int(),
                    OrdemResumo = c.Int(),
                    OrdemMapaResultado = c.Int(),
                    FaturamentoItemId = c.Long(),
                    EquipamentoId = c.Long(),
                    ExameIncluiId = c.Long(),
                    SetorId = c.Long(),
                    MaterialId = c.Long(),
                    MetodoId = c.Long(),
                    UnidadeId = c.Long(),
                    FormataId = c.Long(),
                    MapaId = c.Long(),
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
                    { "DynamicFilter_Exame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabEquipamento", t => t.EquipamentoId)
                .ForeignKey("dbo.LabExame", t => t.ExameIncluiId)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId)
                .ForeignKey("dbo.LabFormata", t => t.FormataId)
                .ForeignKey("dbo.LabMapa", t => t.MapaId)
                .ForeignKey("dbo.LabMaterial", t => t.MaterialId)
                .ForeignKey("dbo.LabMetodo", t => t.MetodoId)
                .ForeignKey("dbo.LabSetor", t => t.SetorId)
                .ForeignKey("dbo.LabUnidade", t => t.UnidadeId)
                .Index(t => t.FaturamentoItemId)
                .Index(t => t.EquipamentoId)
                .Index(t => t.ExameIncluiId)
                .Index(t => t.SetorId)
                .Index(t => t.MaterialId)
                .Index(t => t.MetodoId)
                .Index(t => t.UnidadeId)
                .Index(t => t.FormataId)
                .Index(t => t.MapaId);

            CreateTable(
                "dbo.LabFormata",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Formatacao = c.String(),
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
                    { "DynamicFilter_Formata_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabMapa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Cabec1 = c.String(),
                    Cabec2 = c.String(),
                    Cabec3 = c.String(),
                    Cabec4 = c.String(),
                    Cabec5 = c.String(),
                    Cabec6 = c.String(),
                    Cabec7 = c.String(),
                    Cabec8 = c.String(),
                    Cabec9 = c.String(),
                    Cabec10 = c.String(),
                    Cabec11 = c.String(),
                    Cabec12 = c.String(),
                    Cabec13 = c.String(),
                    Cabec14 = c.String(),
                    Cabec15 = c.String(),
                    Cabec16 = c.String(),
                    Cabec17 = c.String(),
                    Cabec18 = c.String(),
                    Cabec19 = c.String(),
                    Cabec20 = c.String(),
                    Exame1ID = c.Int(),
                    Exame2ID = c.Int(),
                    Exame3ID = c.Int(),
                    Exame4ID = c.Int(),
                    Exame5ID = c.Int(),
                    Exame6ID = c.Int(),
                    Exame7ID = c.Int(),
                    Exame8ID = c.Int(),
                    Exame9ID = c.Int(),
                    Exame10ID = c.Int(),
                    Exame11ID = c.Int(),
                    Exame12ID = c.Int(),
                    Exame13ID = c.Int(),
                    Exame14ID = c.Int(),
                    Exame15ID = c.Int(),
                    Exame16ID = c.Int(),
                    Exame17ID = c.Int(),
                    Exame18ID = c.Int(),
                    Exame19ID = c.Int(),
                    Exame20ID = c.Int(),
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
                    { "DynamicFilter_Mapa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LabKit",
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
                    { "DynamicFilter_Kit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.LabTabelaResultado", "IsAlterado", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTabelaResultado", "TabelaId", c => c.Long());
            AddColumn("dbo.LabKitExame", "KitId", c => c.Long());
            AddColumn("dbo.LabKitExame", "ExameId", c => c.Long());
            AddColumn("dbo.LabKitExame", "IsLiberaKit", c => c.Boolean(nullable: false));
            CreateIndex("dbo.LabTabelaResultado", "TabelaId");
            CreateIndex("dbo.LabKitExame", "KitId");
            CreateIndex("dbo.LabKitExame", "ExameId");
            AddForeignKey("dbo.LabTabelaResultado", "TabelaId", "dbo.LabTabela", "Id");
            AddForeignKey("dbo.LabKitExame", "ExameId", "dbo.LabExame", "Id");
            AddForeignKey("dbo.LabKitExame", "KitId", "dbo.LabKit", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabKitExame", "KitId", "dbo.LabKit");
            DropForeignKey("dbo.LabKitExame", "ExameId", "dbo.LabExame");
            DropForeignKey("dbo.LabExame", "UnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabExame", "SetorId", "dbo.LabSetor");
            DropForeignKey("dbo.LabExame", "MetodoId", "dbo.LabMetodo");
            DropForeignKey("dbo.LabExame", "MaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.LabExame", "MapaId", "dbo.LabMapa");
            DropForeignKey("dbo.LabExame", "FormataId", "dbo.LabFormata");
            DropForeignKey("dbo.LabExame", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.LabExame", "ExameIncluiId", "dbo.LabExame");
            DropForeignKey("dbo.LabExame", "EquipamentoId", "dbo.LabEquipamento");
            DropForeignKey("dbo.LabTabelaResultado", "TabelaId", "dbo.LabTabela");
            DropIndex("dbo.LabExame", new[] { "MapaId" });
            DropIndex("dbo.LabExame", new[] { "FormataId" });
            DropIndex("dbo.LabExame", new[] { "UnidadeId" });
            DropIndex("dbo.LabExame", new[] { "MetodoId" });
            DropIndex("dbo.LabExame", new[] { "MaterialId" });
            DropIndex("dbo.LabExame", new[] { "SetorId" });
            DropIndex("dbo.LabExame", new[] { "ExameIncluiId" });
            DropIndex("dbo.LabExame", new[] { "EquipamentoId" });
            DropIndex("dbo.LabExame", new[] { "FaturamentoItemId" });
            DropIndex("dbo.LabKitExame", new[] { "ExameId" });
            DropIndex("dbo.LabKitExame", new[] { "KitId" });
            DropIndex("dbo.LabTabelaResultado", new[] { "TabelaId" });
            DropColumn("dbo.LabKitExame", "IsLiberaKit");
            DropColumn("dbo.LabKitExame", "ExameId");
            DropColumn("dbo.LabKitExame", "KitId");
            DropColumn("dbo.LabTabelaResultado", "TabelaId");
            DropColumn("dbo.LabTabelaResultado", "IsAlterado");
            DropTable("dbo.LabKit",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Kit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabMapa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Mapa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabFormata",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Formata_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabExame",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Exame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabTabela",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
