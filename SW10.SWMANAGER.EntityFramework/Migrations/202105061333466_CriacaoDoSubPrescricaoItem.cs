namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriacaoDoSubPrescricaoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItem", "PrescricaoItemId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoItem", "PrescricaoItemId");
            AddForeignKey("dbo.AssPrescricaoItem", "PrescricaoItemId", "dbo.AssPrescricaoItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoItem", "PrescricaoItemId", "dbo.AssPrescricaoItem");
            DropIndex("dbo.AssPrescricaoItem", new[] { "PrescricaoItemId" });
            DropColumn("dbo.AssPrescricaoItem", "PrescricaoItemId");
        }
    }
}
