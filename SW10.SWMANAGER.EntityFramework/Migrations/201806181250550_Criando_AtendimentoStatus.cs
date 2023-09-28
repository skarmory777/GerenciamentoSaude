namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Criando_AtendimentoStatus : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AteAtendimento", name: "AtendimentoStatusId", newName: "AteAtendimentoStatusId");
            CreateTable(
                "dbo.AteAtendimentoStatus",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_AtendimentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.AteAtendimento", "AteAtendimentoStatusId");
            AddForeignKey("dbo.AteAtendimento", "AteAtendimentoStatusId", "dbo.AteAtendimentoStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "AteAtendimentoStatusId", "dbo.AteAtendimentoStatus");
            DropIndex("dbo.AteAtendimento", new[] { "AteAtendimentoStatusId" });
            DropTable("dbo.AteAtendimentoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameColumn(table: "dbo.AteAtendimento", name: "AteAtendimentoStatusId", newName: "AtendimentoStatusId");
        }
    }
}
