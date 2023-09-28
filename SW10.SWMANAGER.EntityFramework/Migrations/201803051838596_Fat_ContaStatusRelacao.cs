namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_ContaStatusRelacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatConta", "FatContaStatusId", c => c.Long());
            CreateIndex("dbo.FatConta", "FatContaStatusId");
            AddForeignKey("dbo.FatConta", "FatContaStatusId", "dbo.FatContaStatus", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatConta", "FatContaStatusId", "dbo.FatContaStatus");
            DropIndex("dbo.FatConta", new[] { "FatContaStatusId" });
            DropColumn("dbo.FatConta", "FatContaStatusId");
        }
    }
}
