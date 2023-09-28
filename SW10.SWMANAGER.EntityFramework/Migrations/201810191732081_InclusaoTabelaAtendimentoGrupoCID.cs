namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class InclusaoTabelaAtendimentoGrupoCID : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAtendimentoGrupoCID",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AtendimentoId = c.Long(nullable: false),
                    GrupoCIDId = c.Long(nullable: false),
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
                    { "DynamicFilter_AtendimentoGrupoCID_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.GrupoCID", t => t.GrupoCIDId, cascadeDelete: true)
                .Index(t => t.AtendimentoId)
                .Index(t => t.GrupoCIDId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimentoGrupoCID", "GrupoCIDId", "dbo.GrupoCID");
            DropForeignKey("dbo.AteAtendimentoGrupoCID", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AteAtendimentoGrupoCID", new[] { "GrupoCIDId" });
            DropIndex("dbo.AteAtendimentoGrupoCID", new[] { "AtendimentoId" });
            DropTable("dbo.AteAtendimentoGrupoCID",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoGrupoCID_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
