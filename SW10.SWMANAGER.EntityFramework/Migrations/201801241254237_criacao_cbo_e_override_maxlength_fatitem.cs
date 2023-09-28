namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class criacao_cbo_e_override_maxlength_fatitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisCbo",
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
                    { "DynamicFilter_Cbo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AlterColumn("dbo.FatItemTabela", "Codigo", c => c.String(maxLength: 20));
        }

        public override void Down()
        {
            AlterColumn("dbo.FatItemTabela", "Codigo", c => c.String(maxLength: 10));
            DropTable("dbo.SisCbo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cbo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
