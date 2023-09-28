namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inserindo_relacionameto_com_FatItem_e_AssPrescricaoItem_e_FinContaAdministrativa_em_Produto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Produto", "IsFaturamentoItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Produto", "IsPrescricaoItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Produto", "FatItemId", c => c.Long());
            AddColumn("dbo.Est_Produto", "FinContaAdministrativaId", c => c.Long());
            CreateIndex("dbo.Est_Produto", "FatItemId");
            CreateIndex("dbo.Est_Produto", "FinContaAdministrativaId");
            AddForeignKey("dbo.Est_Produto", "FinContaAdministrativaId", "dbo.FinContaAdministrativa", "Id");
            AddForeignKey("dbo.Est_Produto", "FatItemId", "dbo.FatItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Est_Produto", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.Est_Produto", "FinContaAdministrativaId", "dbo.FinContaAdministrativa");
            DropIndex("dbo.Est_Produto", new[] { "FinContaAdministrativaId" });
            DropIndex("dbo.Est_Produto", new[] { "FatItemId" });
            DropColumn("dbo.Est_Produto", "FinContaAdministrativaId");
            DropColumn("dbo.Est_Produto", "FatItemId");
            DropColumn("dbo.Est_Produto", "IsPrescricaoItem");
            DropColumn("dbo.Est_Produto", "IsFaturamentoItem");
        }
    }
}
