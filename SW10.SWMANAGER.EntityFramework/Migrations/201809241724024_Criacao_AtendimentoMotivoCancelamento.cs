namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criacao_AtendimentoMotivoCancelamento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteMotivoCancelamento",
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
                    { "DynamicFilter_AtendimentoMotivoCancelamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AteAtendimento", "AtendimentoMotivoCancelamentoId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "AtendimentoMotivoCancelamentoId");
            AddForeignKey("dbo.AteAtendimento", "AtendimentoMotivoCancelamentoId", "dbo.AteMotivoCancelamento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "AtendimentoMotivoCancelamentoId", "dbo.AteMotivoCancelamento");
            DropIndex("dbo.AteAtendimento", new[] { "AtendimentoMotivoCancelamentoId" });
            DropColumn("dbo.AteAtendimento", "AtendimentoMotivoCancelamentoId");
            DropTable("dbo.AteMotivoCancelamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoMotivoCancelamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
