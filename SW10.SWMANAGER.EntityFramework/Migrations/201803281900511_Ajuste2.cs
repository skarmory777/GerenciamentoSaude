namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Ajuste2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisMedico", "ReligiaoId", c => c.Long());
            AddColumn("dbo.SisPaciente", "ReligiaoId", c => c.Long());
            CreateIndex("dbo.SisMedico", "ReligiaoId");
            CreateIndex("dbo.SisPaciente", "ReligiaoId");
            AddForeignKey("dbo.SisMedico", "ReligiaoId", "dbo.SisReligiao", "Id");
            AddForeignKey("dbo.SisPaciente", "ReligiaoId", "dbo.SisReligiao", "Id");
            DropColumn("dbo.SisMedico", "Religiao");
            DropColumn("dbo.SisPaciente", "Religiao");
        }

        public override void Down()
        {
            AddColumn("dbo.SisPaciente", "Religiao", c => c.Int());
            AddColumn("dbo.SisMedico", "Religiao", c => c.Int());
            DropForeignKey("dbo.SisPaciente", "ReligiaoId", "dbo.SisReligiao");
            DropForeignKey("dbo.SisMedico", "ReligiaoId", "dbo.SisReligiao");
            DropIndex("dbo.SisPaciente", new[] { "ReligiaoId" });
            DropIndex("dbo.SisMedico", new[] { "ReligiaoId" });
            DropColumn("dbo.SisPaciente", "ReligiaoId");
            DropColumn("dbo.SisMedico", "ReligiaoId");
        }
    }
}
