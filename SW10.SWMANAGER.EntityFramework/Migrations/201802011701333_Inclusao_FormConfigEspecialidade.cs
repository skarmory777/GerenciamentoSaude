namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_FormConfigEspecialidade : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisFormConfigEspecialidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisFormConfigId = c.Long(),
                    SisEspecialidadeId = c.Long(),
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
                    { "DynamicFilter_FormConfigEspecialidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisEspecialidade", t => t.SisEspecialidadeId)
                .ForeignKey("dbo.SisFormConfig", t => t.SisFormConfigId)
                .Index(t => t.SisFormConfigId)
                .Index(t => t.SisEspecialidadeId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisFormConfigEspecialidade", "SisFormConfigId", "dbo.SisFormConfig");
            DropForeignKey("dbo.SisFormConfigEspecialidade", "SisEspecialidadeId", "dbo.SisEspecialidade");
            DropIndex("dbo.SisFormConfigEspecialidade", new[] { "SisEspecialidadeId" });
            DropIndex("dbo.SisFormConfigEspecialidade", new[] { "SisFormConfigId" });
            DropTable("dbo.SisFormConfigEspecialidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfigEspecialidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
