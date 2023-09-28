namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class RemocaoTipoGrupoCentroCusto_CRUD_Duplicado : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GrupoCentroCusto", "TipoGrupoCentroCustosId", "dbo.TipoGrupoCentroCusto");
            DropIndex("dbo.GrupoCentroCusto", new[] { "TipoGrupoCentroCustosId" });
            DropColumn("dbo.GrupoCentroCusto", "TipoGrupoCentroCustosId");
            DropTable("dbo.TipoGrupoCentroCusto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoGrupoCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.TipoGrupoCentroCusto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 75),
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
                    { "DynamicFilter_TipoGrupoCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.GrupoCentroCusto", "TipoGrupoCentroCustosId", c => c.Long());
            CreateIndex("dbo.GrupoCentroCusto", "TipoGrupoCentroCustosId");
            AddForeignKey("dbo.GrupoCentroCusto", "TipoGrupoCentroCustosId", "dbo.TipoGrupoCentroCusto", "Id");
        }
    }
}
