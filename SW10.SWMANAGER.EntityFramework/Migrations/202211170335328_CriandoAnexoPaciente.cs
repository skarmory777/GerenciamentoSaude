namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoAnexoPaciente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisPaciente", "AnexoListaId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisPaciente", "AnexoListaId");
        }
    }
}
