namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Tabela_LabResultadoStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabResultadoStatus",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CorFonte = c.String(),
                    CorFundo = c.String(),
                    Sequencia = c.Int(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ResultadoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.LabResultadoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResultadoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
