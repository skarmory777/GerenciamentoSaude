namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoCamposGlosaEmFatEntregaConta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatEntregaConta", "ValorGlosaRecuperavel", c => c.Single());
            AddColumn("dbo.FatEntregaConta", "ValorGlosaRecuperavelTemp", c => c.Single());
            AddColumn("dbo.FatEntregaConta", "ValorGlosaIrrecuperavel", c => c.Single());
            AddColumn("dbo.FatEntregaConta", "ValorGlosaIrrecuperavelTemp", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FatEntregaConta", "ValorGlosaIrrecuperavelTemp");
            DropColumn("dbo.FatEntregaConta", "ValorGlosaIrrecuperavel");
            DropColumn("dbo.FatEntregaConta", "ValorGlosaRecuperavelTemp");
            DropColumn("dbo.FatEntregaConta", "ValorGlosaRecuperavel");
        }
    }
}
