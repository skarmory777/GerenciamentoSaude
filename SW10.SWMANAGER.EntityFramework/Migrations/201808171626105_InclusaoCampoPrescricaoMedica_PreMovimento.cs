namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoPrescricaoMedica_PreMovimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "SisPrescricaoMedicaId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "SisPrescricaoMedicaId");
            AddForeignKey("dbo.EstoquePreMovimento", "SisPrescricaoMedicaId", "dbo.AssPrescricaoMedica", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "SisPrescricaoMedicaId", "dbo.AssPrescricaoMedica");
            DropIndex("dbo.EstoquePreMovimento", new[] { "SisPrescricaoMedicaId" });
            DropColumn("dbo.EstoquePreMovimento", "SisPrescricaoMedicaId");
        }
    }
}
