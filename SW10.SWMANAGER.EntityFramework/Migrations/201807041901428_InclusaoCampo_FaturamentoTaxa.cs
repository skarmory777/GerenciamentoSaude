namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampo_FaturamentoTaxa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatTaxa", "TurnoId", c => c.Long());
            CreateIndex("dbo.FatTaxa", "TurnoId");
            AddForeignKey("dbo.FatTaxa", "TurnoId", "dbo.SisTurno", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTaxa", "TurnoId", "dbo.SisTurno");
            DropIndex("dbo.FatTaxa", new[] { "TurnoId" });
            DropColumn("dbo.FatTaxa", "TurnoId");
        }
    }
}
