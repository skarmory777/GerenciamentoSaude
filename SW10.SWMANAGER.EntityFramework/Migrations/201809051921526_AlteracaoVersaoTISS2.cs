namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AlteracaoVersaoTISS2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisVersaoTiss",
                c => new
                {
                    Id = c.Long(nullable: false),
                    DataInicio = c.DateTime(nullable: false),
                    DataFim = c.DateTime(nullable: false),
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
                    { "DynamicFilter_VersaoTiss_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.SisConvenio", "VersaoTissId", c => c.Long());
            AddColumn("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId", c => c.Long(nullable: false));
            CreateIndex("dbo.SisConvenio", "VersaoTissId");
            CreateIndex("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId");
            AddForeignKey("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId", "dbo.SisVersaoTiss", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SisConvenio", "VersaoTissId", "dbo.SisVersaoTiss", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisConvenio", "VersaoTissId", "dbo.SisVersaoTiss");
            DropForeignKey("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId", "dbo.SisVersaoTiss");
            DropIndex("dbo.SisTabelaDominioVersaoTiss", new[] { "VersaoTissId" });
            DropIndex("dbo.SisConvenio", new[] { "VersaoTissId" });
            DropColumn("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId");
            DropColumn("dbo.SisConvenio", "VersaoTissId");
            DropTable("dbo.SisVersaoTiss",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VersaoTiss_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
