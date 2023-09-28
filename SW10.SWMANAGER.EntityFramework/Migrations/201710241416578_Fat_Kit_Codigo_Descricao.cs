namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_Kit_Codigo_Descricao : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FaturamentoKitFaturamentoItems", newName: "FaturamentoItemFaturamentoKits");
            DropPrimaryKey("dbo.FaturamentoItemFaturamentoKits");
            AddColumn("dbo.FatContaKit", "TerceirizadoId", c => c.Long());
            AddColumn("dbo.FatContaKit", "FatKitId", c => c.Long());
            AddColumn("dbo.FatKit", "Descricao", c => c.String(maxLength: 255));
            AddColumn("dbo.Terceirizado", "Descricao", c => c.String(maxLength: 255));
            AddPrimaryKey("dbo.FaturamentoItemFaturamentoKits", new[] { "FaturamentoItem_Id", "FaturamentoKit_Id" });
            CreateIndex("dbo.FatContaKit", "TerceirizadoId");
            CreateIndex("dbo.FatContaKit", "FatKitId");
            AddForeignKey("dbo.FatContaKit", "FatKitId", "dbo.FatKit", "Id");
            AddForeignKey("dbo.FatContaKit", "TerceirizadoId", "dbo.Terceirizado", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatContaKit", "TerceirizadoId", "dbo.Terceirizado");
            DropForeignKey("dbo.FatContaKit", "FatKitId", "dbo.FatKit");
            DropIndex("dbo.FatContaKit", new[] { "FatKitId" });
            DropIndex("dbo.FatContaKit", new[] { "TerceirizadoId" });
            DropPrimaryKey("dbo.FaturamentoItemFaturamentoKits");
            DropColumn("dbo.Terceirizado", "Descricao");
            DropColumn("dbo.FatKit", "Descricao");
            DropColumn("dbo.FatContaKit", "FatKitId");
            DropColumn("dbo.FatContaKit", "TerceirizadoId");
            AddPrimaryKey("dbo.FaturamentoItemFaturamentoKits", new[] { "FaturamentoKit_Id", "FaturamentoItem_Id" });
            RenameTable(name: "dbo.FaturamentoItemFaturamentoKits", newName: "FaturamentoKitFaturamentoItems");
        }
    }
}
