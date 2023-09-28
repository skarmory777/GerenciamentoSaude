namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposFaturamentoContaItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "FatItemCobradoId", c => c.Long());
            AddColumn("dbo.FatContaItem", "FatPacoteId", c => c.Long());
            CreateIndex("dbo.FatContaItem", "FatItemCobradoId");
            CreateIndex("dbo.FatContaItem", "FatPacoteId");
            AddForeignKey("dbo.FatContaItem", "FatItemCobradoId", "dbo.FatItem", "Id");
            AddForeignKey("dbo.FatContaItem", "FatPacoteId", "dbo.FatPacote", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatContaItem", "FatPacoteId", "dbo.FatPacote");
            DropForeignKey("dbo.FatContaItem", "FatItemCobradoId", "dbo.FatItem");
            DropIndex("dbo.FatContaItem", new[] { "FatPacoteId" });
            DropIndex("dbo.FatContaItem", new[] { "FatItemCobradoId" });
            DropColumn("dbo.FatContaItem", "FatPacoteId");
            DropColumn("dbo.FatContaItem", "FatItemCobradoId");
        }
    }
}
