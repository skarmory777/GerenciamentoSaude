namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Ajustando_campos_nullable_Pro_ReqExameMov_e_MovItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pro_ReqExameMovItem", "IdSW", c => c.Long());
            AddColumn("dbo.Pro_ReqExameMov", "IdSW", c => c.Long());
            AlterColumn("dbo.Pro_ReqExameMovItem", "IdAutorizacao", c => c.Int());
            AlterColumn("dbo.Pro_ReqExameMovItem", "DataAutorizacao", c => c.DateTime());
            AlterColumn("dbo.Pro_ReqExameMov", "DataAutorizacao", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.Pro_ReqExameMov", "DataAutorizacao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pro_ReqExameMovItem", "DataAutorizacao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pro_ReqExameMovItem", "IdAutorizacao", c => c.Int(nullable: false));
            DropColumn("dbo.Pro_ReqExameMov", "IdSW");
            DropColumn("dbo.Pro_ReqExameMovItem", "IdSW");
        }
    }
}
