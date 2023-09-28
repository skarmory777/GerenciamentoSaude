namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_foreigh_comprarequisicao_estoque : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicao", "EstEstoqueId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicao", "EstEstoqueId");
            AddForeignKey("dbo.CmpRequisicao", "EstEstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicao", "EstEstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.CmpRequisicao", new[] { "EstEstoqueId" });
            DropColumn("dbo.CmpRequisicao", "EstEstoqueId");
        }
    }
}
