namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CamposDoFaturamento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisOcorrencias",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        TipoOcorrenciaId = c.Long(nullable: false),
                        SubTipoOcorrenciaId = c.Long(),
                        SourceModel = c.String(),
                        SourceId = c.Long(),
                        RelationModel = c.String(),
                        RelationId = c.Long(),
                        Texto = c.String(),
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
                    { "DynamicFilter_Ocorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisSubTipoOcorrencias", t => t.SubTipoOcorrenciaId)
                .ForeignKey("dbo.SisTipoOcorrencias", t => t.TipoOcorrenciaId, cascadeDelete: true)
                .Index(t => t.TipoOcorrenciaId)
                .Index(t => t.SubTipoOcorrenciaId);
            
            CreateTable(
                "dbo.SisSubTipoOcorrencias",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TipoOcorrenciaId = c.Long(nullable: false),
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
                    { "DynamicFilter_SubTipoOcorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisTipoOcorrencias", t => t.TipoOcorrenciaId, cascadeDelete: true)
                .Index(t => t.TipoOcorrenciaId);
            
            CreateTable(
                "dbo.SisTipoOcorrencias",
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
                    { "DynamicFilter_TipoOcorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FatConta", "OrigemTitular", c => c.Int(nullable: false));
            AddColumn("dbo.FatConta", "DataInicio", c => c.DateTime());
            DropColumn("dbo.FatConta", "DataIncio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FatConta", "DataIncio", c => c.DateTime());
            DropForeignKey("dbo.SisOcorrencias", "TipoOcorrenciaId", "dbo.SisTipoOcorrencias");
            DropForeignKey("dbo.SisOcorrencias", "SubTipoOcorrenciaId", "dbo.SisSubTipoOcorrencias");
            DropForeignKey("dbo.SisSubTipoOcorrencias", "TipoOcorrenciaId", "dbo.SisTipoOcorrencias");
            DropIndex("dbo.SisSubTipoOcorrencias", new[] { "TipoOcorrenciaId" });
            DropIndex("dbo.SisOcorrencias", new[] { "SubTipoOcorrenciaId" });
            DropIndex("dbo.SisOcorrencias", new[] { "TipoOcorrenciaId" });
            DropColumn("dbo.FatConta", "DataInicio");
            DropColumn("dbo.FatConta", "OrigemTitular");
            DropTable("dbo.SisTipoOcorrencias",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoOcorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisSubTipoOcorrencias",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubTipoOcorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisOcorrencias",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Ocorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
