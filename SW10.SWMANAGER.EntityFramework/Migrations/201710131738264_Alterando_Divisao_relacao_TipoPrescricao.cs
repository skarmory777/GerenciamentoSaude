namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alterando_Divisao_relacao_TipoPrescricao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssDivisao", "AssTipoPrescricaoId", c => c.Long());
            CreateIndex("dbo.AssDivisao", "AssTipoPrescricaoId");
            AddForeignKey("dbo.AssDivisao", "AssTipoPrescricaoId", "dbo.AssTipoPrescricao", "Id");
            DropColumn("dbo.AssDivisao", "IsMedico");
            DropColumn("dbo.AssDivisao", "IsEnfermagem");
        }

        public override void Down()
        {
            AddColumn("dbo.AssDivisao", "IsEnfermagem", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsMedico", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AssDivisao", "AssTipoPrescricaoId", "dbo.AssTipoPrescricao");
            DropIndex("dbo.AssDivisao", new[] { "AssTipoPrescricaoId" });
            DropColumn("dbo.AssDivisao", "AssTipoPrescricaoId");
        }
    }
}
