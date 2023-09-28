namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_campos_PrescricaoItem_Divisao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssDivisao", "IsMedicamento", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItem", "EstEstoqueId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoItem", "EstEstoqueId");
            AddForeignKey("dbo.AssPrescricaoItem", "EstEstoqueId", "dbo.Est_Estoque", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssPrescricaoItem", "EstEstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.AssPrescricaoItem", new[] { "EstEstoqueId" });
            DropColumn("dbo.AssPrescricaoItem", "EstEstoqueId");
            DropColumn("dbo.AssDivisao", "IsMedicamento");
        }
    }
}
