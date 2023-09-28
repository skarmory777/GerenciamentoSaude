namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ranca_cbosus_de_especialidade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SisEspecialidade", "SisCboSusId", "dbo.SisCbo");
            DropIndex("dbo.SisEspecialidade", new[] { "SisCboSusId" });
            DropColumn("dbo.SisEspecialidade", "SisCboSusId");
        }

        public override void Down()
        {
            AddColumn("dbo.SisEspecialidade", "SisCboSusId", c => c.Long());
            CreateIndex("dbo.SisEspecialidade", "SisCboSusId");
            AddForeignKey("dbo.SisEspecialidade", "SisCboSusId", "dbo.SisCbo", "Id");
        }
    }
}
