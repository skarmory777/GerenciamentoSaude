namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoFaturamentoKit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FaturamentoKitFaturamentoItems", "FaturamentoKit_Id", "dbo.FatKit");
            DropForeignKey("dbo.FaturamentoKitFaturamentoItems", "FaturamentoItem_Id", "dbo.FatItem");
            DropIndex("dbo.FaturamentoKitFaturamentoItems", new[] { "FaturamentoKit_Id" });
            DropIndex("dbo.FaturamentoKitFaturamentoItems", new[] { "FaturamentoItem_Id" });
            AlterColumn("dbo.FatKitItem", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.FaturamentoKitFaturamentoItems");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.FaturamentoKitFaturamentoItems",
                c => new
                {
                    FaturamentoKit_Id = c.Long(nullable: false),
                    FaturamentoItem_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.FaturamentoKit_Id, t.FaturamentoItem_Id });

            AlterColumn("dbo.FatKitItem", "Quantidade", c => c.Int(nullable: false));
            CreateIndex("dbo.FaturamentoKitFaturamentoItems", "FaturamentoItem_Id");
            CreateIndex("dbo.FaturamentoKitFaturamentoItems", "FaturamentoKit_Id");
            AddForeignKey("dbo.FaturamentoKitFaturamentoItems", "FaturamentoItem_Id", "dbo.FatItem", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FaturamentoKitFaturamentoItems", "FaturamentoKit_Id", "dbo.FatKit", "Id", cascadeDelete: true);
        }
    }
}
