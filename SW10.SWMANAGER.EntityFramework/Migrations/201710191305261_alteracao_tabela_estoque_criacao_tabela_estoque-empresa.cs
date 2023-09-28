namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class alteracao_tabela_estoque_criacao_tabela_estoqueempresa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Est_Estoque", "SetorId", c => c.Long());
            AddColumn("dbo.Est_Estoque", "TipoEstoque", c => c.Long(nullable: false));
            AddColumn("dbo.Est_Estoque", "TipoCusto", c => c.Long(nullable: false));
            AddColumn("dbo.Est_Estoque", "IsImpressaoAutomatica", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Estoque", "DevolucaoProdutos", c => c.Long(nullable: false));
            AddColumn("dbo.Est_Estoque", "CaminhoImpressoraSolicitacaoProduto", c => c.String());
            AddColumn("dbo.Est_Estoque", "CaminhoImpressoraCodigoBarra", c => c.String());
            AddColumn("dbo.Est_Estoque", "CaminhoImpressoraEtiquetaPaciente", c => c.String());
            AddColumn("dbo.Est_Estoque", "IsSaidaPaciente", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Estoque", "IsSaidaSetor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Estoque", "IsDevolucaoPaciente", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Estoque", "IsDevolucaoSetor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Est_Estoque", "IsTransferenciaEstoques", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Est_Estoque", "SetorId");
            AddForeignKey("dbo.Est_Estoque", "SetorId", "dbo.UnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Est_Estoque", "SetorId", "dbo.UnidadeOrganizacional");
            DropIndex("dbo.Est_Estoque", new[] { "SetorId" });
            DropColumn("dbo.Est_Estoque", "IsTransferenciaEstoques");
            DropColumn("dbo.Est_Estoque", "IsDevolucaoSetor");
            DropColumn("dbo.Est_Estoque", "IsDevolucaoPaciente");
            DropColumn("dbo.Est_Estoque", "IsSaidaSetor");
            DropColumn("dbo.Est_Estoque", "IsSaidaPaciente");
            DropColumn("dbo.Est_Estoque", "CaminhoImpressoraEtiquetaPaciente");
            DropColumn("dbo.Est_Estoque", "CaminhoImpressoraCodigoBarra");
            DropColumn("dbo.Est_Estoque", "CaminhoImpressoraSolicitacaoProduto");
            DropColumn("dbo.Est_Estoque", "DevolucaoProdutos");
            DropColumn("dbo.Est_Estoque", "IsImpressaoAutomatica");
            DropColumn("dbo.Est_Estoque", "TipoCusto");
            DropColumn("dbo.Est_Estoque", "TipoEstoque");
            DropColumn("dbo.Est_Estoque", "SetorId");
        }
    }
}
