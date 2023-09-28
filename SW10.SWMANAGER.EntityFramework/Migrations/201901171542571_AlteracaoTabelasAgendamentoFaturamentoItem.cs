namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTabelasAgendamentoFaturamentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "IsAgendaMaterial", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAgendamentoMaterial", "Material", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAgendamentoMaterial", "Material");
            DropColumn("dbo.FatItem", "IsAgendaMaterial");
        }
    }
}
