namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoTabelaFatEntregaContaRecebida : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatEntregaContaRecebida",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FinQuitacaoId = c.Long(),
                        FatEntregaContaId = c.Long(),
                        ValorRecebido = c.Single(nullable: false),
                        ValorGlosaRecuperavel = c.Single(),
                        ValorGlosaIrrecuperavel = c.Single(),
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
                    { "DynamicFilter_FaturamentoEntregaContaRecebida_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatEntregaConta", t => t.FatEntregaContaId)
                .ForeignKey("dbo.FinQuitacao", t => t.FinQuitacaoId)
                .Index(t => t.FinQuitacaoId)
                .Index(t => t.FatEntregaContaId);
            
            AddColumn("dbo.FinQuitacao", "TransferenciaIdentificador", c => c.Guid());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FatEntregaContaRecebida", "FinQuitacaoId", "dbo.FinQuitacao");
            DropForeignKey("dbo.FatEntregaContaRecebida", "FatEntregaContaId", "dbo.FatEntregaConta");
            DropIndex("dbo.FatEntregaContaRecebida", new[] { "FatEntregaContaId" });
            DropIndex("dbo.FatEntregaContaRecebida", new[] { "FinQuitacaoId" });
            DropColumn("dbo.FinQuitacao", "TransferenciaIdentificador");
            DropTable("dbo.FatEntregaContaRecebida",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoEntregaContaRecebida_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
