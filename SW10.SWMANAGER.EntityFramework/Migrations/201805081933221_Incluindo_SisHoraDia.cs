namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Incluindo_SisHoraDia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisHoraDia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_HoraDia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AssPrescricaoItemHora", "SisHoraDiaId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoItemHora", "SisHoraDiaId");
            AddForeignKey("dbo.AssPrescricaoItemHora", "SisHoraDiaId", "dbo.SisHoraDia", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoItemHora", "SisHoraDiaId", "dbo.SisHoraDia");
            DropIndex("dbo.AssPrescricaoItemHora", new[] { "SisHoraDiaId" });
            DropColumn("dbo.AssPrescricaoItemHora", "SisHoraDiaId");
            DropTable("dbo.SisHoraDia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_HoraDia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
