namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AdicionandoFaturamentoPacote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatPacote",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Inicio = c.DateTime(nullable: false),
                    Final = c.DateTime(nullable: false),
                    FaturamentoItemId = c.Long(),
                    FaturamentoContaId = c.Long(),
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
                    { "DynamicFilter_FaturamentoPacote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatConta", t => t.FaturamentoContaId)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId)
                .Index(t => t.FaturamentoItemId)
                .Index(t => t.FaturamentoContaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatPacote", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatPacote", "FaturamentoContaId", "dbo.FatConta");
            DropIndex("dbo.FatPacote", new[] { "FaturamentoContaId" });
            DropIndex("dbo.FatPacote", new[] { "FaturamentoItemId" });
            DropTable("dbo.FatPacote",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoPacote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
