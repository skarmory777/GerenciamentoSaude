namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_TenantId_RegExameMov_MovItem : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SisMovutomaticoConvenioPlano", newName: "SisMovAutomaticoConvenioPlano");
            AddColumn("dbo.Pro_ReqExameMovItem", "TenantId", c => c.Long());
            AddColumn("dbo.Pro_ReqExameMov", "TenantId", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.Pro_ReqExameMov", "TenantId");
            DropColumn("dbo.Pro_ReqExameMovItem", "TenantId");
            RenameTable(name: "dbo.SisMovAutomaticoConvenioPlano", newName: "SisMovutomaticoConvenioPlano");
        }
    }
}
