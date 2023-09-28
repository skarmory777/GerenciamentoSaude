namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AlteracaoConvenio_DadosContato : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteFormaAutorizacao",
                c => new
                {
                    Id = c.Long(nullable: false),
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
                    { "DynamicFilter_FormaAutorizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.SisConvenio", "FormaAutorizacaoId", c => c.Long());
            AddColumn("dbo.SisConvenio", "DadosContato", c => c.String());
            CreateIndex("dbo.SisConvenio", "FormaAutorizacaoId");
            AddForeignKey("dbo.SisConvenio", "FormaAutorizacaoId", "dbo.AteFormaAutorizacao", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisConvenio", "FormaAutorizacaoId", "dbo.AteFormaAutorizacao");
            DropIndex("dbo.SisConvenio", new[] { "FormaAutorizacaoId" });
            DropColumn("dbo.SisConvenio", "DadosContato");
            DropColumn("dbo.SisConvenio", "FormaAutorizacaoId");
            DropTable("dbo.AteFormaAutorizacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormaAutorizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
