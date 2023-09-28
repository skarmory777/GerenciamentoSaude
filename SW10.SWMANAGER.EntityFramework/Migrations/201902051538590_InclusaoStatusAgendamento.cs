namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class InclusaoStatusAgendamento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAgendamentoStatus",
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
                    { "DynamicFilter_AgendamentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AgendamentoConsulta", "StatusId", c => c.Long());
            CreateIndex("dbo.AgendamentoConsulta", "StatusId");
            AddForeignKey("dbo.AgendamentoConsulta", "StatusId", "dbo.AteAgendamentoStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AgendamentoConsulta", "StatusId", "dbo.AteAgendamentoStatus");
            DropIndex("dbo.AgendamentoConsulta", new[] { "StatusId" });
            DropColumn("dbo.AgendamentoConsulta", "StatusId");
            DropTable("dbo.AteAgendamentoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
