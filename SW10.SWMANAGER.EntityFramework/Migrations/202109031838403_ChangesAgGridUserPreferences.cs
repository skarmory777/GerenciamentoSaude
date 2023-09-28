namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesAgGridUserPreferences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AgGridUserPreferences", "ColumnState", c => c.String());
            AddColumn("dbo.AgGridUserPreferences", "ColumnGroupState", c => c.String());
            AddColumn("dbo.AgGridUserPreferences", "SortModel", c => c.String());
            AddColumn("dbo.AgGridUserPreferences", "FilterModel", c => c.String());
            DropColumn("dbo.AgGridUserPreferences", "Data");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AgGridUserPreferences", "Data", c => c.String());
            DropColumn("dbo.AgGridUserPreferences", "FilterModel");
            DropColumn("dbo.AgGridUserPreferences", "SortModel");
            DropColumn("dbo.AgGridUserPreferences", "ColumnGroupState");
            DropColumn("dbo.AgGridUserPreferences", "ColumnState");
        }
    }
}
