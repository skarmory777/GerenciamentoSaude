namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class AjustesCadastrosLaboratorio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LabColetaExameInformacao", "LabUnidadeId", "dbo.LabUnidade");
            DropForeignKey("dbo.LabInformacao", "UnidadeId", "dbo.LabUnidade");
            DropIndex("dbo.LabColetaExameInformacao", new[] { "LabUnidadeId" });
            DropTable("dbo.LabUnidade");

            CreateTable(
                "dbo.LabUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_UnidadeLaboratorio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AlterTableAnnotations(
                "dbo.LabTecnico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
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
                        "DynamicFilter_Tecnico_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AlterTableAnnotations(
                "dbo.LabFormatacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    NomeFormatacao = c.String(),
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
                        "DynamicFilter_Formatacao_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AlterTableAnnotations(
                "dbo.LabInformacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    CasaDecimal = c.Long(nullable: false),
                    Minimo = c.Double(nullable: false),
                    Maximo = c.Double(nullable: false),
                    MinimoExistente = c.Double(nullable: false),
                    MaximoExistente = c.Double(nullable: false),
                    Referencia = c.String(),
                    IsSoma100 = c.Boolean(nullable: false),
                    ObsAnormal = c.String(),
                    Islongerface = c.Boolean(nullable: false),
                    LongerfaceEnvio = c.String(),
                    LongerfaceRetorno = c.String(),
                    Dividelongerface = c.String(),
                    UnidadeId = c.Long(nullable: false),
                    TipoResultadoId = c.Long(),
                    EquipamentoId = c.Long(),
                    TabelaResultadoId = c.Long(),
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
                        "DynamicFilter_Informacao_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AlterTableAnnotations(
                "dbo.LabTabelaResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                        "DynamicFilter_TabelaResultado_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AlterTableAnnotations(
                "dbo.LabTipoResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                        "DynamicFilter_TipoResultado_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AlterTableAnnotations(
                "dbo.LabMetodo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                        "DynamicFilter_Metodo_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });

            AddColumn("dbo.LabTecnico", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTecnico", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTecnico", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabTecnico", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabTecnico", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabTecnico", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabTecnico", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabTecnico", "CreatorUserId", c => c.Long());
            AddColumn("dbo.LabFormatacao", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabFormatacao", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabFormatacao", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabFormatacao", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabFormatacao", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabFormatacao", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabFormatacao", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabFormatacao", "CreatorUserId", c => c.Long());
            AddColumn("dbo.LabInformacao", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabInformacao", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabInformacao", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabInformacao", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabInformacao", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabInformacao", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabInformacao", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabInformacao", "CreatorUserId", c => c.Long());
            AddColumn("dbo.LabTabelaResultado", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTabelaResultado", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTabelaResultado", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabTabelaResultado", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabTabelaResultado", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabTabelaResultado", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabTabelaResultado", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabTabelaResultado", "CreatorUserId", c => c.Long());
            AddColumn("dbo.LabTipoResultado", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTipoResultado", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabTipoResultado", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabTipoResultado", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabTipoResultado", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabTipoResultado", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabTipoResultado", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabTipoResultado", "CreatorUserId", c => c.Long());
            AddColumn("dbo.LabMetodo", "IsSistema", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabMetodo", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabMetodo", "DeleterUserId", c => c.Long());
            AddColumn("dbo.LabMetodo", "DeletionTime", c => c.DateTime());
            AddColumn("dbo.LabMetodo", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.LabMetodo", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.LabMetodo", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LabMetodo", "CreatorUserId", c => c.Long());
            //DropTable("dbo.LabUnidade");
            AddForeignKey("dbo.LabColetaExameInformacao", "LabUnidadeId", "dbo.LabUnidade", "Id");
            AddForeignKey("dbo.LabInformacao", "UnidadeId", "dbo.LabUnidade", "Id");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.LabUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            DropColumn("dbo.LabMetodo", "CreatorUserId");
            DropColumn("dbo.LabMetodo", "CreationTime");
            DropColumn("dbo.LabMetodo", "LastModifierUserId");
            DropColumn("dbo.LabMetodo", "LastModificationTime");
            DropColumn("dbo.LabMetodo", "DeletionTime");
            DropColumn("dbo.LabMetodo", "DeleterUserId");
            DropColumn("dbo.LabMetodo", "IsDeleted");
            DropColumn("dbo.LabMetodo", "IsSistema");
            DropColumn("dbo.LabTipoResultado", "CreatorUserId");
            DropColumn("dbo.LabTipoResultado", "CreationTime");
            DropColumn("dbo.LabTipoResultado", "LastModifierUserId");
            DropColumn("dbo.LabTipoResultado", "LastModificationTime");
            DropColumn("dbo.LabTipoResultado", "DeletionTime");
            DropColumn("dbo.LabTipoResultado", "DeleterUserId");
            DropColumn("dbo.LabTipoResultado", "IsDeleted");
            DropColumn("dbo.LabTipoResultado", "IsSistema");
            DropColumn("dbo.LabTabelaResultado", "CreatorUserId");
            DropColumn("dbo.LabTabelaResultado", "CreationTime");
            DropColumn("dbo.LabTabelaResultado", "LastModifierUserId");
            DropColumn("dbo.LabTabelaResultado", "LastModificationTime");
            DropColumn("dbo.LabTabelaResultado", "DeletionTime");
            DropColumn("dbo.LabTabelaResultado", "DeleterUserId");
            DropColumn("dbo.LabTabelaResultado", "IsDeleted");
            DropColumn("dbo.LabTabelaResultado", "IsSistema");
            DropColumn("dbo.LabInformacao", "CreatorUserId");
            DropColumn("dbo.LabInformacao", "CreationTime");
            DropColumn("dbo.LabInformacao", "LastModifierUserId");
            DropColumn("dbo.LabInformacao", "LastModificationTime");
            DropColumn("dbo.LabInformacao", "DeletionTime");
            DropColumn("dbo.LabInformacao", "DeleterUserId");
            DropColumn("dbo.LabInformacao", "IsDeleted");
            DropColumn("dbo.LabInformacao", "IsSistema");
            DropColumn("dbo.LabFormatacao", "CreatorUserId");
            DropColumn("dbo.LabFormatacao", "CreationTime");
            DropColumn("dbo.LabFormatacao", "LastModifierUserId");
            DropColumn("dbo.LabFormatacao", "LastModificationTime");
            DropColumn("dbo.LabFormatacao", "DeletionTime");
            DropColumn("dbo.LabFormatacao", "DeleterUserId");
            DropColumn("dbo.LabFormatacao", "IsDeleted");
            DropColumn("dbo.LabFormatacao", "IsSistema");
            DropColumn("dbo.LabTecnico", "CreatorUserId");
            DropColumn("dbo.LabTecnico", "CreationTime");
            DropColumn("dbo.LabTecnico", "LastModifierUserId");
            DropColumn("dbo.LabTecnico", "LastModificationTime");
            DropColumn("dbo.LabTecnico", "DeletionTime");
            DropColumn("dbo.LabTecnico", "DeleterUserId");
            DropColumn("dbo.LabTecnico", "IsDeleted");
            DropColumn("dbo.LabTecnico", "IsSistema");
            AlterTableAnnotations(
                "dbo.LabMetodo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                        "DynamicFilter_Metodo_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AlterTableAnnotations(
                "dbo.LabTipoResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                        "DynamicFilter_TipoResultado_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AlterTableAnnotations(
                "dbo.LabTabelaResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                        "DynamicFilter_TabelaResultado_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AlterTableAnnotations(
                "dbo.LabInformacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    CasaDecimal = c.Long(nullable: false),
                    Minimo = c.Double(nullable: false),
                    Maximo = c.Double(nullable: false),
                    MinimoExistente = c.Double(nullable: false),
                    MaximoExistente = c.Double(nullable: false),
                    Referencia = c.String(),
                    IsSoma100 = c.Boolean(nullable: false),
                    ObsAnormal = c.String(),
                    Islongerface = c.Boolean(nullable: false),
                    LongerfaceEnvio = c.String(),
                    LongerfaceRetorno = c.String(),
                    Dividelongerface = c.String(),
                    UnidadeId = c.Long(nullable: false),
                    TipoResultadoId = c.Long(),
                    EquipamentoId = c.Long(),
                    TabelaResultadoId = c.Long(),
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
                        "DynamicFilter_Informacao_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AlterTableAnnotations(
                "dbo.LabFormatacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    NomeFormatacao = c.String(),
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
                        "DynamicFilter_Formatacao_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            AlterTableAnnotations(
                "dbo.LabTecnico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
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
                        "DynamicFilter_Tecnico_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });

            DropTable("dbo.LabUnidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeLaboratorio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
