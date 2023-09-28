namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Lab_FormataItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabFormataItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FormataId = c.Long(),
                    ItemResultadoId = c.Long(),
                    Ordem = c.Int(nullable: false),
                    OrdemRegistro = c.Int(),
                    Formula = c.String(),
                    IsBI = c.Boolean(nullable: false),
                    IsRefExame = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FormataItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabFormata", t => t.FormataId)
                .ForeignKey("dbo.LabItemResultado", t => t.ItemResultadoId)
                .Index(t => t.FormataId)
                .Index(t => t.ItemResultadoId);

            CreateTable(
                "dbo.LabItemResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CasaDecimal = c.Int(),
                    ErroMinimo = c.Double(nullable: false),
                    ErroMaximo = c.Double(nullable: false),
                    AlteradoMinimo = c.Double(nullable: false),
                    AlteradoMaximo = c.Double(nullable: false),
                    AceitavelMinimo = c.Double(nullable: false),
                    AceitavelMaximo = c.Double(nullable: false),
                    Referencia = c.String(),
                    Formula = c.String(),
                    Normal = c.Double(nullable: false),
                    TamFixo = c.Int(),
                    ObsAnormal = c.String(),
                    ErroMinimoFeminino = c.Double(nullable: false),
                    AlteradoMinimoFeminino = c.Double(nullable: false),
                    NormalFeminino = c.Double(nullable: false),
                    AceitavelMaximoFeminino = c.Double(nullable: false),
                    ErroMaximoFeminino = c.Double(nullable: false),
                    Interface = c.String(),
                    InterfaceEnvio = c.String(),
                    Equipamento = c.String(),
                    DivideInter = c.Double(nullable: false),
                    IsAntibiotico = c.Boolean(nullable: false),
                    IsBacteria = c.Boolean(nullable: false),
                    IsInteiro = c.Boolean(nullable: false),
                    IsObrigatorio = c.Boolean(nullable: false),
                    IsMultiValor = c.Boolean(nullable: false),
                    IsSoma100 = c.Boolean(nullable: false),
                    ParteInteira = c.Boolean(nullable: false),
                    IsInterface = c.Boolean(nullable: false),
                    IsTamFixo = c.Boolean(nullable: false),
                    TipoResultadoId = c.Long(),
                    LaboratorioUnidadeId = c.Long(),
                    TabelaId = c.Long(),
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
                    { "DynamicFilter_ItemResultado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabUnidade", t => t.LaboratorioUnidadeId)
                .ForeignKey("dbo.LabTabela", t => t.TabelaId)
                .ForeignKey("dbo.LabTipoResultado", t => t.TipoResultadoId)
                .Index(t => t.TipoResultadoId)
                .Index(t => t.LaboratorioUnidadeId)
                .Index(t => t.TabelaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.LabFormataItem", "ItemResultadoId", "dbo.LabItemResultado");
            DropForeignKey("dbo.LabItemResultado", "TipoResultadoId", "dbo.LabTipoResultado");
            DropForeignKey("dbo.LabItemResultado", "TabelaId", "dbo.LabTabela");
            DropForeignKey("dbo.LabItemResultado", "LaboratorioUnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabFormataItem", "FormataId", "dbo.LabFormata");
            DropIndex("dbo.LabItemResultado", new[] { "TabelaId" });
            DropIndex("dbo.LabItemResultado", new[] { "LaboratorioUnidadeId" });
            DropIndex("dbo.LabItemResultado", new[] { "TipoResultadoId" });
            DropIndex("dbo.LabFormataItem", new[] { "ItemResultadoId" });
            DropIndex("dbo.LabFormataItem", new[] { "FormataId" });
            DropTable("dbo.LabItemResultado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ItemResultado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabFormataItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormataItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
