namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_ModeloPrescricao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssModeloPrescricaoMedica",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PrescricaoMedicaId = c.Long(nullable: false),
                        AtendimentoId = c.Long(nullable: false),
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
                    { "DynamicFilter_ModeloPrescricao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.AssPrescricaoMedica", t => t.PrescricaoMedicaId, cascadeDelete: true)
                .Index(t => t.PrescricaoMedicaId)
                .Index(t => t.AtendimentoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssModeloPrescricaoMedica", "PrescricaoMedicaId", "dbo.AssPrescricaoMedica");
            DropForeignKey("dbo.AssModeloPrescricaoMedica", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AssModeloPrescricaoMedica", new[] { "AtendimentoId" });
            DropIndex("dbo.AssModeloPrescricaoMedica", new[] { "PrescricaoMedicaId" });
            DropTable("dbo.AssModeloPrescricaoMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ModeloPrescricao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
