namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoDocumentoConvenio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "IsCaixa", c => c.Boolean(nullable: false));
            AddColumn("dbo.FinDocumento", "FatContaId", c => c.Long());
            CreateIndex("dbo.FinDocumento", "FatContaId");
            AddForeignKey("dbo.FinDocumento", "FatContaId", "dbo.FatConta", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinDocumento", "FatContaId", "dbo.FatConta");
            DropIndex("dbo.FinDocumento", new[] { "FatContaId" });
            DropColumn("dbo.FinDocumento", "FatContaId");
            DropColumn("dbo.SisConvenio", "IsCaixa");
        }
    }
}
