namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Marcus_EvolucaoMedica : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EvolucaoMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AtendimentoId = c.Long(nullable: false),
                    FormConfigId = c.Long(nullable: false),
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
                    { "DynamicFilter_EvolucaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId, cascadeDelete: false)
                .ForeignKey("dbo.FormConfig", t => t.FormConfigId, cascadeDelete: false)
                .Index(t => t.AtendimentoId)
                .Index(t => t.FormConfigId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EvolucaoMedica", "FormConfigId", "dbo.FormConfig");
            DropForeignKey("dbo.EvolucaoMedica", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.EvolucaoMedica", new[] { "FormConfigId" });
            DropIndex("dbo.EvolucaoMedica", new[] { "AtendimentoId" });
            DropTable("dbo.EvolucaoMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EvolucaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
