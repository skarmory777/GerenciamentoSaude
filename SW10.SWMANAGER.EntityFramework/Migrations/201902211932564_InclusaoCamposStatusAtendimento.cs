namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposStatusAtendimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAtendimentoStatus",
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
                    { "DynamicFilter_AtendimentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AteAtendimento", "AteAtendimentoStatusId", c => c.Long());
            AddColumn("dbo.AteAtendimento", "IsPendenteExames", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimento", "IsPendenteMedicacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimento", "IsPendenteProcedimento", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimento", "IsAtendidoInternado", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimento", "IsAtendidoAlta", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimento", "IsAtendidoAguardandoInternacao", c => c.Boolean(nullable: false));
            CreateIndex("dbo.AteAtendimento", "AteAtendimentoStatusId");
            AddForeignKey("dbo.AteAtendimento", "AteAtendimentoStatusId", "dbo.AteAtendimentoStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "AteAtendimentoStatusId", "dbo.AteAtendimentoStatus");
            DropIndex("dbo.AteAtendimento", new[] { "AteAtendimentoStatusId" });
            DropColumn("dbo.AteAtendimento", "IsAtendidoAguardandoInternacao");
            DropColumn("dbo.AteAtendimento", "IsAtendidoAlta");
            DropColumn("dbo.AteAtendimento", "IsAtendidoInternado");
            DropColumn("dbo.AteAtendimento", "IsPendenteProcedimento");
            DropColumn("dbo.AteAtendimento", "IsPendenteMedicacao");
            DropColumn("dbo.AteAtendimento", "IsPendenteExames");
            DropColumn("dbo.AteAtendimento", "AteAtendimentoStatusId");
            DropTable("dbo.AteAtendimentoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
