namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_Tabela : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatTabelaPrecoItem", "TabelaPrecoId", "dbo.FatTabelaPreco");
            DropIndex("dbo.FatTabelaPrecoItem", new[] { "TabelaPrecoId" });
            AddColumn("dbo.FatTabela", "Codigo", c => c.String(maxLength: 10));
            AddColumn("dbo.FatTabela", "IsAtualizaBrasindice", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatTabela", "TabelaTissItemId", c => c.Long());
            DropColumn("dbo.FatTabelaPrecoItem", "TabelaPrecoId");
            DropTable("dbo.FatTabelaPreco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTabelaPreco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.FatTabelaPreco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 100),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoTabelaPreco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.FatTabelaPrecoItem", "TabelaPrecoId", c => c.Long(nullable: false));
            DropColumn("dbo.FatTabela", "TabelaTissItemId");
            DropColumn("dbo.FatTabela", "IsAtualizaBrasindice");
            DropColumn("dbo.FatTabela", "Codigo");
            CreateIndex("dbo.FatTabelaPrecoItem", "TabelaPrecoId");
            AddForeignKey("dbo.FatTabelaPrecoItem", "TabelaPrecoId", "dbo.FatTabelaPreco", "Id", cascadeDelete: true);
        }
    }
}
