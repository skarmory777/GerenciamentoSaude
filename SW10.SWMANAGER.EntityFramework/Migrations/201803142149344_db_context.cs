namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class db_context : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisBairro",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 120),
                    SisCidadeId = c.Long(nullable: false),
                    Capital = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Bairro_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisCidade", t => t.SisCidadeId, cascadeDelete: true)
                .Index(t => t.SisCidadeId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisBairro", "SisCidadeId", "dbo.SisCidade");
            DropIndex("dbo.SisBairro", new[] { "SisCidadeId" });
            DropTable("dbo.SisBairro",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Bairro_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
