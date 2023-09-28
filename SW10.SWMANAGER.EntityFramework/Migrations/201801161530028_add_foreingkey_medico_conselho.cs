namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_foreingkey_medico_conselho : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisMedico", "SisConselhoId", c => c.Long());
            CreateIndex("dbo.SisMedico", "SisConselhoId");
            AddForeignKey("dbo.SisMedico", "SisConselhoId", "dbo.SisConselho", "Id");
            DropColumn("dbo.SisMedico", "Conselho");
        }

        public override void Down()
        {
            AddColumn("dbo.SisMedico", "Conselho", c => c.String());
            DropForeignKey("dbo.SisMedico", "SisConselhoId", "dbo.SisConselho");
            DropIndex("dbo.SisMedico", new[] { "SisConselhoId" });
            DropColumn("dbo.SisMedico", "SisConselhoId");
        }
    }
}
