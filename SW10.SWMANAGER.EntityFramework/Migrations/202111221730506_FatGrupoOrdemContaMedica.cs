namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FatGrupoOrdemContaMedica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatGrupo", "OrdemAmbulatorio", c => c.Long());
            AddColumn("dbo.FatGrupo", "OrdemInternacao", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FatGrupo", "OrdemInternacao");
            DropColumn("dbo.FatGrupo", "OrdemAmbulatorio");
        }
    }
}
