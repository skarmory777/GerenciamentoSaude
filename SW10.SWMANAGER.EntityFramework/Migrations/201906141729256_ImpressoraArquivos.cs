namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class ImpressoraArquivos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisImpressoraArquivo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FilePath = c.String(),
                    PrinterName = c.String(),
                    NumberOfCopies = c.Long(nullable: false),
                    IsPrinted = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ImpressoraArquivo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.SisImpressoraArquivo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ImpressoraArquivo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
