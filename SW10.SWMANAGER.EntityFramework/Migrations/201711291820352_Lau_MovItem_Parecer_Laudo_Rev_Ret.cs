namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Lau_MovItem_Parecer_Laudo_Rev_Ret : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimentoItem", "Parecer", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "UsuarioParecerId", c => c.Long());
            AddColumn("dbo.LauMovimentoItem", "ParecerData", c => c.DateTime());
            AddColumn("dbo.LauMovimentoItem", "Laudo", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "UsuarioLaudoId", c => c.Long());
            AddColumn("dbo.LauMovimentoItem", "LaudoData", c => c.DateTime());
            AddColumn("dbo.LauMovimentoItem", "ConcordanciaLaudo", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "JustificativaConcoLaudo", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "Revisao", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "UsuarioRevisaoId", c => c.Long());
            AddColumn("dbo.LauMovimentoItem", "RevisaoData", c => c.DateTime());
            AddColumn("dbo.LauMovimentoItem", "Retificacao", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "UsuarioRetificacaoId", c => c.Long());
            AddColumn("dbo.LauMovimentoItem", "RetificacaoData", c => c.DateTime());
            AddColumn("dbo.LauMovimentoItem", "Status", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.LauMovimentoItem", "Status");
            DropColumn("dbo.LauMovimentoItem", "RetificacaoData");
            DropColumn("dbo.LauMovimentoItem", "UsuarioRetificacaoId");
            DropColumn("dbo.LauMovimentoItem", "Retificacao");
            DropColumn("dbo.LauMovimentoItem", "RevisaoData");
            DropColumn("dbo.LauMovimentoItem", "UsuarioRevisaoId");
            DropColumn("dbo.LauMovimentoItem", "Revisao");
            DropColumn("dbo.LauMovimentoItem", "JustificativaConcoLaudo");
            DropColumn("dbo.LauMovimentoItem", "ConcordanciaLaudo");
            DropColumn("dbo.LauMovimentoItem", "LaudoData");
            DropColumn("dbo.LauMovimentoItem", "UsuarioLaudoId");
            DropColumn("dbo.LauMovimentoItem", "Laudo");
            DropColumn("dbo.LauMovimentoItem", "ParecerData");
            DropColumn("dbo.LauMovimentoItem", "UsuarioParecerId");
            DropColumn("dbo.LauMovimentoItem", "Parecer");
        }
    }
}
