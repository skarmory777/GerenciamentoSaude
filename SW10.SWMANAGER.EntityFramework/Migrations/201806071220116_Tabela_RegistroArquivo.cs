namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Tabela_RegistroArquivo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisRegistroArquivo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    RegistroId = c.Long(nullable: false),
                    Tabela = c.String(),
                    Campo = c.String(),
                    Arquivo = c.Binary(),
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
                    { "DynamicFilter_RegistroArquivo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.SisRegistroArquivo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RegistroArquivo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
