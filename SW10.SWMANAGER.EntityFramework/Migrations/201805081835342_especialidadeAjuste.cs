namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class especialidadeAjuste : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisMedicoEspecialidade", "SisMedicoId", "dbo.SisMedico");
            DropIndex("dbo.SisMedicoEspecialidade", new[] { "SisMedicoId" });
            AlterColumn("dbo.SisMedicoEspecialidade", "SisMedicoId", c => c.Long());
            CreateIndex("dbo.SisMedicoEspecialidade", "SisMedicoId");
            AddForeignKey("dbo.SisMedicoEspecialidade", "SisMedicoId", "dbo.SisMedico", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisMedicoEspecialidade", "SisMedicoId", "dbo.SisMedico");
            DropIndex("dbo.SisMedicoEspecialidade", new[] { "SisMedicoId" });
            AlterColumn("dbo.SisMedicoEspecialidade", "SisMedicoId", c => c.Long(nullable: false));
            CreateIndex("dbo.SisMedicoEspecialidade", "SisMedicoId");
            AddForeignKey("dbo.SisMedicoEspecialidade", "SisMedicoId", "dbo.SisMedico", "Id", cascadeDelete: true);
        }
    }
}
