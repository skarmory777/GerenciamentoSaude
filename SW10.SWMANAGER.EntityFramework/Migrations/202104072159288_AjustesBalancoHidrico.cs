namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesBalancoHidrico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteBalancoHidricos", "DesConferidoManha", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteBalancoHidricos", "DesConferidoManhaUserId", c => c.Long());
            AddColumn("dbo.AteBalancoHidricos", "DtDesConferidoManha", c => c.DateTime());
            AddColumn("dbo.AteBalancoHidricos", "DesConferidoNoite", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteBalancoHidricos", "DesConferidoNoiteUserId", c => c.Long());
            AddColumn("dbo.AteBalancoHidricos", "DtDesConferidoNoite", c => c.DateTime());
            AddColumn("dbo.AteBalancoHidricos", "DesConferidoTotal", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteBalancoHidricos", "DesConferidoTotalUserId", c => c.Long());
            AddColumn("dbo.AteBalancoHidricos", "DtDesConferidoTotal", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteBalancoHidricos", "DtDesConferidoTotal");
            DropColumn("dbo.AteBalancoHidricos", "DesConferidoTotalUserId");
            DropColumn("dbo.AteBalancoHidricos", "DesConferidoTotal");
            DropColumn("dbo.AteBalancoHidricos", "DtDesConferidoNoite");
            DropColumn("dbo.AteBalancoHidricos", "DesConferidoNoiteUserId");
            DropColumn("dbo.AteBalancoHidricos", "DesConferidoNoite");
            DropColumn("dbo.AteBalancoHidricos", "DtDesConferidoManha");
            DropColumn("dbo.AteBalancoHidricos", "DesConferidoManhaUserId");
            DropColumn("dbo.AteBalancoHidricos", "DesConferidoManha");
        }
    }
}
