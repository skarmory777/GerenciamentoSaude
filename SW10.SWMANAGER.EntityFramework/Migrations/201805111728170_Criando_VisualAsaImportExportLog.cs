namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criando_VisualAsaImportExportLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisVisualAsaImportExportLog",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Tabela = c.String(),
                    IdAsa = c.Long(nullable: false),
                    IdSw = c.Long(nullable: false),
                    Operacao = c.String(),
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
                    { "DynamicFilter_VisualAsaImportExportLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.SisVisualAsaImportExportLog",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VisualAsaImportExportLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
