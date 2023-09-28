namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criacao_ExameStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabExameStatus",
                c => new
                {
                    Id = c.Long(nullable: false),
                    Cor = c.String(),
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
                    { "DynamicFilter_ExameStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.LabResultadoExame", "ExameStatusId", c => c.Long());
            CreateIndex("dbo.LabResultadoExame", "ExameStatusId");
            AddForeignKey("dbo.LabResultadoExame", "ExameStatusId", "dbo.LabExameStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "ExameStatusId", "dbo.LabExameStatus");
            DropIndex("dbo.LabResultadoExame", new[] { "ExameStatusId" });
            DropColumn("dbo.LabResultadoExame", "ExameStatusId");
            DropTable("dbo.LabExameStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ExameStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
