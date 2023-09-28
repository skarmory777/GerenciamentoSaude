namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_ContaItemStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "FatContaStatusId", c => c.Long());
            CreateIndex("dbo.FatContaItem", "FatContaStatusId");
            AddForeignKey("dbo.FatContaItem", "FatContaStatusId", "dbo.FatContaStatus", "Id");
            DropColumn("dbo.FatContaItem", "StatusEntrega");
        }

        public override void Down()
        {
            AddColumn("dbo.FatContaItem", "StatusEntrega", c => c.String());
            DropForeignKey("dbo.FatContaItem", "FatContaStatusId", "dbo.FatContaStatus");
            DropIndex("dbo.FatContaItem", new[] { "FatContaStatusId" });
            DropColumn("dbo.FatContaItem", "FatContaStatusId");
        }
    }
}
