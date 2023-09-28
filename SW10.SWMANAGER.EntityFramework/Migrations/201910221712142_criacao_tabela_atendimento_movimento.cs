namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class criacao_tabela_atendimento_movimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssAtendimentoMovimento",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AtendimentoId = c.Long(nullable: false),
                        DataInicio = c.DateTime(),
                        DataFinal = c.DateTime(),
                        MedicoId = c.Long(),
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
                    { "DynamicFilter_AtendimentoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.SisMedico", t => t.MedicoId)
                .Index(t => t.AtendimentoId)
                .Index(t => t.MedicoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssAtendimentoMovimento", "MedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.AssAtendimentoMovimento", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AssAtendimentoMovimento", new[] { "MedicoId" });
            DropIndex("dbo.AssAtendimentoMovimento", new[] { "AtendimentoId" });
            DropTable("dbo.AssAtendimentoMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
