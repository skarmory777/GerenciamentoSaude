namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inserindo_Relacionamento_Atendimento_em_AutorizacaoProcedimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAutorizacaoProcedimento", "AtendimentoId", c => c.Long());
            CreateIndex("dbo.AteAutorizacaoProcedimento", "AtendimentoId");
            AddForeignKey("dbo.AteAutorizacaoProcedimento", "AtendimentoId", "dbo.AteAtendimento", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "AtendimentoId" });
            DropColumn("dbo.AteAutorizacaoProcedimento", "AtendimentoId");
        }
    }
}
