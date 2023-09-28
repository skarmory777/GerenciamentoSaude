namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AlteracaoVersaoTISS : DbMigration
    {
        public override void Up()
        {
            // DropForeignKey("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId", "dbo.VersaoTiss");
            DropForeignKey(dependentTable: "dbo.SisTabelaDominioVersaoTiss", name: "FK_dbo.TabelaDominioVersaoTiss_dbo.VersaoTiss_VersaoTissId");

            DropForeignKey("dbo.SisConvenio", "VersaoTissId", "dbo.SisVersaoTiss");
            DropIndex("dbo.SisConvenio", new[] { "VersaoTissId" });
            DropIndex("dbo.SisTabelaDominioVersaoTiss", new[] { "VersaoTissId" });
            DropColumn("dbo.SisConvenio", "VersaoTissId");
            DropColumn("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId");
            DropTable("dbo.SisVersaoTiss",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VersaoTiss_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.SisVersaoTiss",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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

            AddColumn("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId", c => c.Long(nullable: false));
            AddColumn("dbo.SisConvenio", "VersaoTissId", c => c.Long());
            CreateIndex("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId");
            CreateIndex("dbo.SisConvenio", "VersaoTissId");
            AddForeignKey("dbo.SisConvenio", "VersaoTissId", "dbo.SisVersaoTiss", "Id");
            AddForeignKey("dbo.SisTabelaDominioVersaoTiss", "VersaoTissId", "dbo.SisVersaoTiss", "Id", cascadeDelete: true);
        }
    }
}
