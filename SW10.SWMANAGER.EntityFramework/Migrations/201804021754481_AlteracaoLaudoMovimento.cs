namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoLaudoMovimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimento", "FaturamentocontaItemId", c => c.Long());
            CreateIndex("dbo.LauMovimento", "FaturamentocontaItemId");
            AddForeignKey("dbo.LauMovimento", "FaturamentocontaItemId", "dbo.FatContaItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LauMovimento", "FaturamentocontaItemId", "dbo.FatContaItem");
            DropIndex("dbo.LauMovimento", new[] { "FaturamentocontaItemId" });
            DropColumn("dbo.LauMovimento", "FaturamentocontaItemId");
        }
    }
}
