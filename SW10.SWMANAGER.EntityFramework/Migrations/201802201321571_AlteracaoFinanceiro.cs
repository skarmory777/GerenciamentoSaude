namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoFinanceiro : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinDocumento", "QuantidadeParcelas", c => c.Int(nullable: false));
            AddColumn("dbo.FinQuitacao", "ChequeId", c => c.Long());
            CreateIndex("dbo.FinQuitacao", "ChequeId");
            AddForeignKey("dbo.FinQuitacao", "ChequeId", "dbo.FinCheque", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinQuitacao", "ChequeId", "dbo.FinCheque");
            DropIndex("dbo.FinQuitacao", new[] { "ChequeId" });
            DropColumn("dbo.FinQuitacao", "ChequeId");
            DropColumn("dbo.FinDocumento", "QuantidadeParcelas");
        }
    }
}
