namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_Taxas1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatTaxaEmpresa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatTaxaId = c.Long(),
                    SisEmpresaId = c.Long(),
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
                    { "DynamicFilter_FaturamentoTaxaEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEmpresa", t => t.SisEmpresaId)
                .ForeignKey("dbo.FatTaxa", t => t.FatTaxaId)
                .Index(t => t.FatTaxaId)
                .Index(t => t.SisEmpresaId);

            CreateTable(
                "dbo.FatTaxaGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatTaxaId = c.Long(),
                    SisGrupoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoTaxaGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGrupo", t => t.SisGrupoId)
                .ForeignKey("dbo.FatTaxa", t => t.FatTaxaId)
                .Index(t => t.FatTaxaId)
                .Index(t => t.SisGrupoId);

            CreateTable(
                "dbo.FatTaxaLocal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatTaxaId = c.Long(),
                    SisUnidadeOrganizacionalId = c.Long(),
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
                    { "DynamicFilter_FaturamentoTaxaLocal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatTaxa", t => t.FatTaxaId)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.SisUnidadeOrganizacionalId)
                .Index(t => t.FatTaxaId)
                .Index(t => t.SisUnidadeOrganizacionalId);

            CreateTable(
                "dbo.FatTaxaTipoLeito",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatTaxaId = c.Long(),
                    TipoLeitoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoTaxaTipoLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatTaxa", t => t.FatTaxaId)
                .ForeignKey("dbo.TipoLeito", t => t.TipoLeitoId)
                .Index(t => t.FatTaxaId)
                .Index(t => t.TipoLeitoId);

            CreateTable(
                "dbo.FatTaxaTurno",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatTaxaId = c.Long(),
                    SisTurnoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoTaxaTurno_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatTaxa", t => t.FatTaxaId)
                .ForeignKey("dbo.SisTurno", t => t.SisTurnoId)
                .Index(t => t.FatTaxaId)
                .Index(t => t.SisTurnoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTaxaTurno", "SisTurnoId", "dbo.SisTurno");
            DropForeignKey("dbo.FatTaxaTurno", "FatTaxaId", "dbo.FatTaxa");
            DropForeignKey("dbo.FatTaxaTipoLeito", "TipoLeitoId", "dbo.TipoLeito");
            DropForeignKey("dbo.FatTaxaTipoLeito", "FatTaxaId", "dbo.FatTaxa");
            DropForeignKey("dbo.FatTaxaLocal", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.FatTaxaLocal", "FatTaxaId", "dbo.FatTaxa");
            DropForeignKey("dbo.FatTaxaGrupo", "FatTaxaId", "dbo.FatTaxa");
            DropForeignKey("dbo.FatTaxaGrupo", "SisGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatTaxaEmpresa", "FatTaxaId", "dbo.FatTaxa");
            DropForeignKey("dbo.FatTaxaEmpresa", "SisEmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.FatTaxaTurno", new[] { "SisTurnoId" });
            DropIndex("dbo.FatTaxaTurno", new[] { "FatTaxaId" });
            DropIndex("dbo.FatTaxaTipoLeito", new[] { "TipoLeitoId" });
            DropIndex("dbo.FatTaxaTipoLeito", new[] { "FatTaxaId" });
            DropIndex("dbo.FatTaxaLocal", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.FatTaxaLocal", new[] { "FatTaxaId" });
            DropIndex("dbo.FatTaxaGrupo", new[] { "SisGrupoId" });
            DropIndex("dbo.FatTaxaGrupo", new[] { "FatTaxaId" });
            DropIndex("dbo.FatTaxaEmpresa", new[] { "SisEmpresaId" });
            DropIndex("dbo.FatTaxaEmpresa", new[] { "FatTaxaId" });
            DropTable("dbo.FatTaxaTurno",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxaTurno_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTaxaTipoLeito",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxaTipoLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTaxaLocal",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxaLocal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTaxaGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxaGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTaxaEmpresa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxaEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
