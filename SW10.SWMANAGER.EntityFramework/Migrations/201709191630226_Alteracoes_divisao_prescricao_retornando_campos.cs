namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_divisao_prescricao_retornando_campos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssDivisao", "IsDivisaoPrincipal", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "AssDivisaoId", c => c.Long());
            CreateIndex("dbo.AssDivisao", "AssDivisaoId");
            AddForeignKey("dbo.AssDivisao", "AssDivisaoId", "dbo.AssDivisao", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssDivisao", "AssDivisaoId", "dbo.AssDivisao");
            DropIndex("dbo.AssDivisao", new[] { "AssDivisaoId" });
            DropColumn("dbo.AssDivisao", "AssDivisaoId");
            DropColumn("dbo.AssDivisao", "IsDivisaoPrincipal");
        }
    }
}
