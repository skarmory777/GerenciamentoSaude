namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ConvenioConfiguracaoResumoConta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConvenioConfiguracaoResumoContas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsAgrupaItens = c.Boolean(nullable: false),
                        IsAgrupaUnidadeOrganizacional = c.Boolean(nullable: false),
                        IsImprimeCID = c.Boolean(nullable: false),
                        IsImprimeCPFMedico = c.Boolean(nullable: false),
                        IsImprimeObservacaoItens = c.Boolean(nullable: false),
                        IsImprimeTratamento = c.Boolean(nullable: false),
                        IsImprimeTratamentos = c.Boolean(nullable: false),
                        IsImprimeDataHoraImpressao = c.Boolean(nullable: false),
                        IsExibeDescontoDoCaixa = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ConvenioConfiguracaoResumoConta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SisConvenio", "IsAgrupaResumoConta", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "IsAgrupaItensResumoConta", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "ConfiguracaoResumoContaInternacaoId", c => c.Long());
            AddColumn("dbo.SisConvenio", "ConfiguracaoResumoContaEmergenciaId", c => c.Long());
            CreateIndex("dbo.SisConvenio", "ConfiguracaoResumoContaInternacaoId");
            CreateIndex("dbo.SisConvenio", "ConfiguracaoResumoContaEmergenciaId");
            AddForeignKey("dbo.SisConvenio", "ConfiguracaoResumoContaEmergenciaId", "dbo.ConvenioConfiguracaoResumoContas", "Id");
            AddForeignKey("dbo.SisConvenio", "ConfiguracaoResumoContaInternacaoId", "dbo.ConvenioConfiguracaoResumoContas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SisConvenio", "ConfiguracaoResumoContaInternacaoId", "dbo.ConvenioConfiguracaoResumoContas");
            DropForeignKey("dbo.SisConvenio", "ConfiguracaoResumoContaEmergenciaId", "dbo.ConvenioConfiguracaoResumoContas");
            DropIndex("dbo.SisConvenio", new[] { "ConfiguracaoResumoContaEmergenciaId" });
            DropIndex("dbo.SisConvenio", new[] { "ConfiguracaoResumoContaInternacaoId" });
            DropColumn("dbo.SisConvenio", "ConfiguracaoResumoContaEmergenciaId");
            DropColumn("dbo.SisConvenio", "ConfiguracaoResumoContaInternacaoId");
            DropColumn("dbo.SisConvenio", "IsAgrupaItensResumoConta");
            DropColumn("dbo.SisConvenio", "IsAgrupaResumoConta");
            DropTable("dbo.ConvenioConfiguracaoResumoContas",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConvenioConfiguracaoResumoConta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
