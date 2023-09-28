namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_TaxaFaturamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatTaxa", "ConvenioId", c => c.Long());
            CreateIndex("dbo.FatTaxa", "ConvenioId");
            AddForeignKey("dbo.FatTaxa", "ConvenioId", "dbo.SisConvenio", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTaxa", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatTaxa", new[] { "ConvenioId" });
            DropColumn("dbo.FatTaxa", "ConvenioId");
        }
    }
}
