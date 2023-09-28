namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_TevRisco_Relacionamento_TevMovimento_x_TevRisco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssTevRisco",
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
                    { "DynamicFilter_TevRisco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AssTevMovimento", "AssTevRiscoId", c => c.Long());
            CreateIndex("dbo.AssTevMovimento", "AssTevRiscoId");
            AddForeignKey("dbo.AssTevMovimento", "AssTevRiscoId", "dbo.AssTevRisco", "Id");
            DropColumn("dbo.AssTevMovimento", "Risco");
        }

        public override void Down()
        {
            AddColumn("dbo.AssTevMovimento", "Risco", c => c.Int(nullable: false));
            DropForeignKey("dbo.AssTevMovimento", "AssTevRiscoId", "dbo.AssTevRisco");
            DropIndex("dbo.AssTevMovimento", new[] { "AssTevRiscoId" });
            DropColumn("dbo.AssTevMovimento", "AssTevRiscoId");
            DropTable("dbo.AssTevRisco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TevRisco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
