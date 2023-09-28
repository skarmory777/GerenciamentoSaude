namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_SolicitacaoExamePrioridade : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SolicitacaoExamePrioridades",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
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
                    { "DynamicFilter_SolicitacaoExamePrioridade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.SolicitacaoExamePrioridades",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoExamePrioridade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
