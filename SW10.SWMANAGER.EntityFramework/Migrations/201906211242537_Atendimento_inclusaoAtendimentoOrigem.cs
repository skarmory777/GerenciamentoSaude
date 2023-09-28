namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Atendimento_inclusaoAtendimentoOrigem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "AtendimentoOrigemId", c => c.Long());
            CreateIndex("dbo.AteAtendimento", "AtendimentoOrigemId");
            AddForeignKey("dbo.AteAtendimento", "AtendimentoOrigemId", "dbo.AteAtendimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAtendimento", "AtendimentoOrigemId", "dbo.AteAtendimento");
            DropIndex("dbo.AteAtendimento", new[] { "AtendimentoOrigemId" });
            DropColumn("dbo.AteAtendimento", "AtendimentoOrigemId");
        }
    }
}
