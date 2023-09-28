namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoFontImpressaoFormDinamico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormConfig", "FontSize", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisFormConfig", "FontSize");
        }
    }
}
