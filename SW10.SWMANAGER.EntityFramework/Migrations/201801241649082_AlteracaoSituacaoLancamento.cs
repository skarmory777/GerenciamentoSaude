namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoSituacaoLancamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinSituacaoLancamento", "CorLancamentoFundo", c => c.String());
            AddColumn("dbo.FinSituacaoLancamento", "CorLancamentoLetra", c => c.String());
            DropColumn("dbo.FinSituacaoLancamento", "CorLancamento");
        }

        public override void Down()
        {
            AddColumn("dbo.FinSituacaoLancamento", "CorLancamento", c => c.String());
            DropColumn("dbo.FinSituacaoLancamento", "CorLancamentoLetra");
            DropColumn("dbo.FinSituacaoLancamento", "CorLancamentoFundo");
        }
    }
}
