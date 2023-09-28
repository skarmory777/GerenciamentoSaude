namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class criacao_cbofamilia_cbotipo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisCboFamilia",
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
                    { "DynamicFilter_CboFamilia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.SisCboTipo",
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
                    { "DynamicFilter_CboTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.SisCbo", "SisCboFamiliaId", c => c.Long());
            AddColumn("dbo.SisCbo", "SisCboTipoId", c => c.Long());
            CreateIndex("dbo.SisCbo", "SisCboFamiliaId");
            CreateIndex("dbo.SisCbo", "SisCboTipoId");
            AddForeignKey("dbo.SisCbo", "SisCboFamiliaId", "dbo.SisCboFamilia", "Id");
            AddForeignKey("dbo.SisCbo", "SisCboTipoId", "dbo.SisCboTipo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisCbo", "SisCboTipoId", "dbo.SisCboTipo");
            DropForeignKey("dbo.SisCbo", "SisCboFamiliaId", "dbo.SisCboFamilia");
            DropIndex("dbo.SisCbo", new[] { "SisCboTipoId" });
            DropIndex("dbo.SisCbo", new[] { "SisCboFamiliaId" });
            DropColumn("dbo.SisCbo", "SisCboTipoId");
            DropColumn("dbo.SisCbo", "SisCboFamiliaId");
            DropTable("dbo.SisCboTipo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CboTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisCboFamilia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CboFamilia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
