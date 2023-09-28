namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class CriacaoFaturamentoCodigoDespesa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatCodigoDespesa",
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
                    { "DynamicFilter_FaturamentoCodigoDespesa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.FatGrupo", "FaturamentoCodigoDespesaId", c => c.Long());
            CreateIndex("dbo.FatGrupo", "FaturamentoCodigoDespesaId");
            AddForeignKey("dbo.FatGrupo", "FaturamentoCodigoDespesaId", "dbo.FatCodigoDespesa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatGrupo", "FaturamentoCodigoDespesaId", "dbo.FatCodigoDespesa");
            DropIndex("dbo.FatGrupo", new[] { "FaturamentoCodigoDespesaId" });
            DropColumn("dbo.FatGrupo", "FaturamentoCodigoDespesaId");
            DropTable("dbo.FatCodigoDespesa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoCodigoDespesa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
