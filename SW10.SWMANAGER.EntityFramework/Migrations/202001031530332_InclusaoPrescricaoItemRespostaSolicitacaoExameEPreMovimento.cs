namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InclusaoPrescricaoItemRespostaSolicitacaoExameEPreMovimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "PrescricaoItemRespostaId", c => c.Long());
            AddColumn("dbo.AssSolicitacaoExameItem", "PrescricaoItemRespostaId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "PrescricaoItemRespostaId");
            CreateIndex("dbo.AssSolicitacaoExameItem", "PrescricaoItemRespostaId");
            AddForeignKey("dbo.EstoquePreMovimento", "PrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta", "Id");
            AddForeignKey("dbo.AssSolicitacaoExameItem", "PrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoExameItem", "PrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta");
            DropForeignKey("dbo.EstoquePreMovimento", "PrescricaoItemRespostaId", "dbo.AssPrescricaoItemResposta");
            DropIndex("dbo.AssSolicitacaoExameItem", new[] { "PrescricaoItemRespostaId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "PrescricaoItemRespostaId" });
            DropColumn("dbo.AssSolicitacaoExameItem", "PrescricaoItemRespostaId");
            DropColumn("dbo.EstoquePreMovimento", "PrescricaoItemRespostaId");
        }
    }
}
