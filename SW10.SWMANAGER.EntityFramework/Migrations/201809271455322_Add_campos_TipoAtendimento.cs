namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Add_campos_TipoAtendimento : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AteTipoAtendimento", newName: "AteAtendimentoTipo");
            RenameColumn(table: "dbo.AteAtendimentoTipo", name: "TabelaItemTissId", newName: "SisTabelaItemTissId");
            RenameIndex(table: "dbo.AteAtendimentoTipo", name: "IX_TabelaItemTissId", newName: "IX_SisTabelaItemTissId");
            AddColumn("dbo.AteAtendimentoTipo", "IsInternacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimentoTipo", "IsAmbulatorioEmergencia", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimentoTipo", "IsAmbulatorioEmergencia");
            DropColumn("dbo.AteAtendimentoTipo", "IsInternacao");
            RenameIndex(table: "dbo.AteAtendimentoTipo", name: "IX_SisTabelaItemTissId", newName: "IX_TabelaItemTissId");
            RenameColumn(table: "dbo.AteAtendimentoTipo", name: "SisTabelaItemTissId", newName: "TabelaItemTissId");
            RenameTable(name: "dbo.AteAtendimentoTipo", newName: "AteTipoAtendimento");
        }
    }
}
