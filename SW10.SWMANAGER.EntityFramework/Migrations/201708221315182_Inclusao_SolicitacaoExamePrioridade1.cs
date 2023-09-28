namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_SolicitacaoExamePrioridade1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SolicitacaoExamePrioridades", newName: "AssSolicitacaoExamePrioridade");
            AlterColumn("dbo.AssSolicitacaoExamePrioridade", "Codigo", c => c.String(maxLength: 10));
            AlterColumn("dbo.AssSolicitacaoExamePrioridade", "Descricao", c => c.String(nullable: false, maxLength: 30));
        }

        public override void Down()
        {
            AlterColumn("dbo.AssSolicitacaoExamePrioridade", "Descricao", c => c.String());
            AlterColumn("dbo.AssSolicitacaoExamePrioridade", "Codigo", c => c.String());
            RenameTable(name: "dbo.AssSolicitacaoExamePrioridade", newName: "SolicitacaoExamePrioridades");
        }
    }
}
