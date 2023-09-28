namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Atualizacao_campos_PrescricaoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItem", "TotalDias", c => c.Long(nullable: false));
            AddColumn("dbo.AssPrescricaoItem", "Quantidade", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.AssPrescricaoItem", "DosePadrao");
            DropColumn("dbo.AssPrescricaoItem", "Duracao");
        }

        public override void Down()
        {
            AddColumn("dbo.AssPrescricaoItem", "Duracao", c => c.String());
            AddColumn("dbo.AssPrescricaoItem", "DosePadrao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AssPrescricaoItem", "Quantidade");
            DropColumn("dbo.AssPrescricaoItem", "TotalDias");
        }
    }
}
