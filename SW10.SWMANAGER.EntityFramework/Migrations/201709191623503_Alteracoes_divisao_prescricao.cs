namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_divisao_prescricao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssDivisao", "AssDivisaoId", "dbo.AssDivisao");
            DropIndex("dbo.AssDivisao", new[] { "AssDivisaoId" });
            DropColumn("dbo.AssDivisao", "AssDivisaoId");
        }

        public override void Down()
        {
            AddColumn("dbo.AssDivisao", "AssDivisaoId", c => c.Long());
            CreateIndex("dbo.AssDivisao", "AssDivisaoId");
            AddForeignKey("dbo.AssDivisao", "AssDivisaoId", "dbo.AssDivisao", "Id");
        }
    }
}
