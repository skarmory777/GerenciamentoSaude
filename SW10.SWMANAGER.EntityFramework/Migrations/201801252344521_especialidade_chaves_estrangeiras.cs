namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class especialidade_chaves_estrangeiras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisEspecialidade", "SisCboId", c => c.Long());
            AddColumn("dbo.SisEspecialidade", "SisCboSusId", c => c.Long());
            CreateIndex("dbo.SisEspecialidade", "SisCboId");
            CreateIndex("dbo.SisEspecialidade", "SisCboSusId");
            AddForeignKey("dbo.SisEspecialidade", "SisCboId", "dbo.SisCbo", "Id");
            AddForeignKey("dbo.SisEspecialidade", "SisCboSusId", "dbo.SisCbo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisEspecialidade", "SisCboSusId", "dbo.SisCbo");
            DropForeignKey("dbo.SisEspecialidade", "SisCboId", "dbo.SisCbo");
            DropIndex("dbo.SisEspecialidade", new[] { "SisCboSusId" });
            DropIndex("dbo.SisEspecialidade", new[] { "SisCboId" });
            DropColumn("dbo.SisEspecialidade", "SisCboSusId");
            DropColumn("dbo.SisEspecialidade", "SisCboId");
        }
    }
}
