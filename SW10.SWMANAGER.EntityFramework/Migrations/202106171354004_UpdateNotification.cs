namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNotification : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisAvisos", "DataProgramada", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SisAvisos", "DataProgramada", c => c.DateTime(nullable: false));
        }
    }
}
