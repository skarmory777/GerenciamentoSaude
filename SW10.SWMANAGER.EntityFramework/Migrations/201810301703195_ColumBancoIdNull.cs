namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ColumBancoIdNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinAgencia", "BancoId", "dbo.FinBanco");
            DropIndex("dbo.FinAgencia", new[] { "BancoId" });
            AlterColumn("dbo.FinAgencia", "BancoId", c => c.Long());
            CreateIndex("dbo.FinAgencia", "BancoId");
            AddForeignKey("dbo.FinAgencia", "BancoId", "dbo.FinBanco", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinAgencia", "BancoId", "dbo.FinBanco");
            DropIndex("dbo.FinAgencia", new[] { "BancoId" });
            AlterColumn("dbo.FinAgencia", "BancoId", c => c.Long(nullable: false));
            CreateIndex("dbo.FinAgencia", "BancoId");
            AddForeignKey("dbo.FinAgencia", "BancoId", "dbo.FinBanco", "Id", cascadeDelete: true);
        }
    }
}
