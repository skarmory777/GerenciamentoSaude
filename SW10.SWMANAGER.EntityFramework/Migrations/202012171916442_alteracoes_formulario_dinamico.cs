namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracoes_formulario_dinamico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "Properties", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisFormColConfig", "Properties");
        }
    }
}
