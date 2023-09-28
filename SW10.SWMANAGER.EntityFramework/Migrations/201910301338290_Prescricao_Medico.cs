namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prescricao_Medico : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssPrescricaoMedica", "SisPrestadorId", "dbo.SisPrestador");
            DropIndex("dbo.AssPrescricaoMedica", new[] { "SisPrestadorId" });
            AddColumn("dbo.AssPrescricaoMedica", "SisMedicoId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoMedica", "SisMedicoId");
            AddForeignKey("dbo.AssPrescricaoMedica", "SisMedicoId", "dbo.SisMedico", "Id");
            DropColumn("dbo.AssPrescricaoMedica", "SisPrestadorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssPrescricaoMedica", "SisPrestadorId", c => c.Long());
            DropForeignKey("dbo.AssPrescricaoMedica", "SisMedicoId", "dbo.SisMedico");
            DropIndex("dbo.AssPrescricaoMedica", new[] { "SisMedicoId" });
            DropColumn("dbo.AssPrescricaoMedica", "SisMedicoId");
            CreateIndex("dbo.AssPrescricaoMedica", "SisPrestadorId");
            AddForeignKey("dbo.AssPrescricaoMedica", "SisPrestadorId", "dbo.SisPrestador", "Id");
        }
    }
}
