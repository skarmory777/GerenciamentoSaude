namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class George_BaixaVale : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstMovimentoBaixa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MovimentoBaixaPrincipalId = c.Long(nullable: false),
                    MovimentoBaixaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_EstMovimentoBaixa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoqueMovimento", t => t.MovimentoBaixaId, cascadeDelete: false)
                .ForeignKey("dbo.EstoqueMovimento", t => t.MovimentoBaixaPrincipalId, cascadeDelete: false)
                .Index(t => t.MovimentoBaixaPrincipalId)
                .Index(t => t.MovimentoBaixaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstMovimentoBaixa", "MovimentoBaixaPrincipalId", "dbo.EstoqueMovimento");
            DropForeignKey("dbo.EstMovimentoBaixa", "MovimentoBaixaId", "dbo.EstoqueMovimento");
            DropIndex("dbo.EstMovimentoBaixa", new[] { "MovimentoBaixaId" });
            DropIndex("dbo.EstMovimentoBaixa", new[] { "MovimentoBaixaPrincipalId" });
            DropTable("dbo.EstMovimentoBaixa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstMovimentoBaixa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
