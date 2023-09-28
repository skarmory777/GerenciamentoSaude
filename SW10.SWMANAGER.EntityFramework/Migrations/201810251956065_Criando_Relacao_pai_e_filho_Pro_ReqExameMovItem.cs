namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Criando_Relacao_pai_e_filho_Pro_ReqExameMovItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pro_ReqExameMovItem", "Pro_ReqExameMovId", c => c.Long());
            CreateIndex("dbo.Pro_ReqExameMovItem", "Pro_ReqExameMovId");
            AddForeignKey("dbo.Pro_ReqExameMovItem", "Pro_ReqExameMovId", "dbo.Pro_ReqExameMov", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Pro_ReqExameMovItem", "Pro_ReqExameMovId", "dbo.Pro_ReqExameMov");
            DropIndex("dbo.Pro_ReqExameMovItem", new[] { "Pro_ReqExameMovId" });
            DropColumn("dbo.Pro_ReqExameMovItem", "Pro_ReqExameMovId");
        }
    }
}
