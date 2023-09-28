namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoLoteValidade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio");
            DropIndex("dbo.LoteValidade", new[] { "EstEstoqueLaboratorioId" });
            AlterColumn("dbo.LoteValidade", "EstEstoqueLaboratorioId", c => c.Long());
            CreateIndex("dbo.LoteValidade", "EstEstoqueLaboratorioId");
            AddForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio");
            DropIndex("dbo.LoteValidade", new[] { "EstEstoqueLaboratorioId" });
            AlterColumn("dbo.LoteValidade", "EstEstoqueLaboratorioId", c => c.Long(nullable: false));
            CreateIndex("dbo.LoteValidade", "EstEstoqueLaboratorioId");
            AddForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio", "Id", cascadeDelete: true);
        }
    }
}
