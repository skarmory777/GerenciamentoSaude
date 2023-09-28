namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracoesTipoLocalChamada : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteTipoLocalChamada", "TipoLocalChamadaProximoId", c => c.Long());
            AddColumn("dbo.AtSenhaMov", "DataHoraInicial", c => c.DateTime());
            AddColumn("dbo.AtSenhaMov", "DataHoraFinal", c => c.DateTime());
            CreateIndex("dbo.AteTipoLocalChamada", "TipoLocalChamadaProximoId");
            AddForeignKey("dbo.AteTipoLocalChamada", "TipoLocalChamadaProximoId", "dbo.AteTipoLocalChamada", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteTipoLocalChamada", "TipoLocalChamadaProximoId", "dbo.AteTipoLocalChamada");
            DropIndex("dbo.AteTipoLocalChamada", new[] { "TipoLocalChamadaProximoId" });
            DropColumn("dbo.AtSenhaMov", "DataHoraFinal");
            DropColumn("dbo.AtSenhaMov", "DataHoraInicial");
            DropColumn("dbo.AteTipoLocalChamada", "TipoLocalChamadaProximoId");
        }
    }
}
