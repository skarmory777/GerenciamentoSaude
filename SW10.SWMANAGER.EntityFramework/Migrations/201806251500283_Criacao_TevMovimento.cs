namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criacao_TevMovimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssTevMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Data = c.DateTime(nullable: false),
                    Risco = c.Int(nullable: false),
                    Observacao = c.String(),
                    AteAtendimentoId = c.Long(),
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
                    { "DynamicFilter_TevMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AteAtendimentoId)
                .Index(t => t.AteAtendimentoId);

            AddColumn("dbo.AteAtendimento", "IsControlaTev", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssTevMovimento", "AteAtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AssTevMovimento", new[] { "AteAtendimentoId" });
            DropColumn("dbo.AteAtendimento", "IsControlaTev");
            DropTable("dbo.AssTevMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TevMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
