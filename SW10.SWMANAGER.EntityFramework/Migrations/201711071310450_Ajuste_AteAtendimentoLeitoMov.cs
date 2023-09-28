namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Ajuste_AteAtendimentoLeitoMov : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAtendimentoLeitoMov",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataInicial = c.DateTime(),
                    DataFinal = c.DateTime(),
                    DataInclusao = c.DateTime(),
                    UserId = c.Long(),
                    AteAtendimentoId = c.Long(),
                    AteLeitoId = c.Long(),
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
                    { "DynamicFilter_AtendimentoLeitoMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AteAtendimentoId)
                .ForeignKey("dbo.AteLeito", t => t.AteLeitoId)
                .ForeignKey("dbo.AbpUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AteAtendimentoId)
                .Index(t => t.AteLeitoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimentoLeitoMov", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AteAtendimentoLeitoMov", "AteLeitoId", "dbo.AteLeito");
            DropForeignKey("dbo.AteAtendimentoLeitoMov", "AteAtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AteAtendimentoLeitoMov", new[] { "AteLeitoId" });
            DropIndex("dbo.AteAtendimentoLeitoMov", new[] { "AteAtendimentoId" });
            DropIndex("dbo.AteAtendimentoLeitoMov", new[] { "UserId" });
            DropTable("dbo.AteAtendimentoLeitoMov",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoLeitoMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
