namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class SolicitacaoAntimicrobianos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssSolicitacaoAntimicrobianos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AteAtendimentoId = c.Long(),
                        SisMedicoId = c.Long(),
                        AssPrescricaoItemId = c.Long(),
                        DataSolicitacao = c.DateTime(nullable: false),
                        DataMaximaTempoProvavel = c.DateTime(nullable: false),
                        TempoProvavelUso = c.Int(nullable: false),
                        TipoInfeccao = c.String(),
                        TipoCultura = c.String(),
                        OutrasIndicacoes = c.String(),
                        OutrosResultados = c.String(),
                        IsSistema = c.Boolean(nullable: false),
                        Codigo = c.String(maxLength: 10),
                        Descricao = c.String(),
                        ImportaId = c.Int(),
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
                    { "DynamicFilter_SolicitacaoAntimicrobiano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AteAtendimentoId)
                .ForeignKey("dbo.SisMedico", t => t.SisMedicoId)
                .ForeignKey("dbo.AssPrescricaoItem", t => t.AssPrescricaoItemId)
                .Index(t => t.AteAtendimentoId)
                .Index(t => t.SisMedicoId)
                .Index(t => t.AssPrescricaoItemId);
            
            CreateTable(
                "dbo.AssSolicitacaoAntimicrobianosCulturas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SolicitacaoAntimicrobianoId = c.Long(nullable: false),
                        TipoId = c.Long(nullable: false),
                        DataCultura = c.DateTime(nullable: false),
                        OutrosResultados = c.String(),
                        IsSistema = c.Boolean(nullable: false),
                        Codigo = c.String(maxLength: 10),
                        Descricao = c.String(),
                        ImportaId = c.Int(),
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
                    { "DynamicFilter_SolicitacaoAntimicrobianosCulturas_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssSolicitacaoAntimicrobianos", t => t.SolicitacaoAntimicrobianoId, cascadeDelete: true)
                .ForeignKey("dbo.AssTipoSolicitacaoAntimicrobianosCultura", t => t.TipoId, cascadeDelete: true)
                .Index(t => t.SolicitacaoAntimicrobianoId)
                .Index(t => t.TipoId);
            
            CreateTable(
                "dbo.AssSolicitacaoAntimicrobianosResultados",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TipoSolicitacaoAntimicrobianosResultadoId = c.Long(nullable: false),
                        Valor = c.Boolean(nullable: false),
                        CulturaId = c.Long(nullable: false),
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
                    { "DynamicFilter_SolicitacaoAntimicrobianosResultados_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssSolicitacaoAntimicrobianosCulturas", t => t.CulturaId, cascadeDelete: true)
                .ForeignKey("dbo.AssTipoSolicitacaoAntimicrobianosResultados", t => t.TipoSolicitacaoAntimicrobianosResultadoId, cascadeDelete: true)
                .Index(t => t.TipoSolicitacaoAntimicrobianosResultadoId)
                .Index(t => t.CulturaId);
            
            CreateTable(
                "dbo.AssTipoSolicitacaoAntimicrobianosResultados",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsSistema = c.Boolean(nullable: false),
                        Codigo = c.String(maxLength: 10),
                        Descricao = c.String(),
                        ImportaId = c.Int(),
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
                    { "DynamicFilter_TipoSolicitacaoAntimicrobianosResultado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssTipoSolicitacaoAntimicrobianosCultura",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsSistema = c.Boolean(nullable: false),
                        Codigo = c.String(maxLength: 10),
                        Descricao = c.String(),
                        ImportaId = c.Int(),
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
                    { "DynamicFilter_TipoSolicitacaoAntimicrobianosCultura_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssSolicitacaoAntimicrobianosIndicacoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TipoSolicitacaoAntimicrobianosIndicacaoId = c.Long(nullable: false),
                        SolicitacaoAntimicrobianoId = c.Long(nullable: false),
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
                    { "DynamicFilter_SolicitacaoAntimicrobianosIndicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssSolicitacaoAntimicrobianos", t => t.SolicitacaoAntimicrobianoId, cascadeDelete: true)
                .ForeignKey("dbo.AssTipoSolicitacaoAntimicrobianosIndicacoes", t => t.TipoSolicitacaoAntimicrobianosIndicacaoId, cascadeDelete: true)
                .Index(t => t.TipoSolicitacaoAntimicrobianosIndicacaoId)
                .Index(t => t.SolicitacaoAntimicrobianoId);
            
            CreateTable(
                "dbo.AssTipoSolicitacaoAntimicrobianosIndicacoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsSistema = c.Boolean(nullable: false),
                        Codigo = c.String(maxLength: 10),
                        Descricao = c.String(),
                        ImportaId = c.Int(),
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
                    { "DynamicFilter_TipoSolicitacaoAntimicrobianosIndicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianosIndicacoes", "TipoSolicitacaoAntimicrobianosIndicacaoId", "dbo.AssTipoSolicitacaoAntimicrobianosIndicacoes");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianosIndicacoes", "SolicitacaoAntimicrobianoId", "dbo.AssSolicitacaoAntimicrobianos");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianosCulturas", "TipoId", "dbo.AssTipoSolicitacaoAntimicrobianosCultura");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianosResultados", "TipoSolicitacaoAntimicrobianosResultadoId", "dbo.AssTipoSolicitacaoAntimicrobianosResultados");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianosResultados", "CulturaId", "dbo.AssSolicitacaoAntimicrobianosCulturas");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianosCulturas", "SolicitacaoAntimicrobianoId", "dbo.AssSolicitacaoAntimicrobianos");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AssPrescricaoItemId", "dbo.AssPrescricaoItem");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.AssSolicitacaoAntimicrobianos", "AteAtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AssSolicitacaoAntimicrobianosIndicacoes", new[] { "SolicitacaoAntimicrobianoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianosIndicacoes", new[] { "TipoSolicitacaoAntimicrobianosIndicacaoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianosResultados", new[] { "CulturaId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianosResultados", new[] { "TipoSolicitacaoAntimicrobianosResultadoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianosCulturas", new[] { "TipoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianosCulturas", new[] { "SolicitacaoAntimicrobianoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "AssPrescricaoItemId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "SisMedicoId" });
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", new[] { "AteAtendimentoId" });
            DropTable("dbo.AssTipoSolicitacaoAntimicrobianosIndicacoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoSolicitacaoAntimicrobianosIndicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssSolicitacaoAntimicrobianosIndicacoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoAntimicrobianosIndicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssTipoSolicitacaoAntimicrobianosCultura",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoSolicitacaoAntimicrobianosCultura_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssTipoSolicitacaoAntimicrobianosResultados",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoSolicitacaoAntimicrobianosResultado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssSolicitacaoAntimicrobianosResultados",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoAntimicrobianosResultados_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssSolicitacaoAntimicrobianosCulturas",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoAntimicrobianosCulturas_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssSolicitacaoAntimicrobianos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoAntimicrobiano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
