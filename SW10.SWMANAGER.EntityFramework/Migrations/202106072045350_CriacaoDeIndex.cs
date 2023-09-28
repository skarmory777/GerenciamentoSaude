namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriacaoDeIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AteAtendimento", "DataRegistro", name: "Ate_Idx_DataRegistro");
            CreateIndex("dbo.AteAtendimento", "DataAlta", name: "Ate_Idx_DataAlta");
            CreateIndex("dbo.AteAtendimento", "DataPrevistaAlta", name: "Ate_Idx_DataPrevistaAlta");
            CreateIndex("dbo.AteAtendimentoLeitoMov", "DataInicial", name: "Ate_Idx_DataInicial");
            CreateIndex("dbo.AteAtendimentoLeitoMov", "DataFinal", name: "Ate_Idx_DataFinal");
            CreateIndex("dbo.AteAtendimentoLeitoMov", "DataInclusao", name: "Ate_Idx_DataInclusao");
            CreateIndex("dbo.AssAtendimentoMovimento", "DataInicio", name: "Ate_Idx_DataInicio");
            CreateIndex("dbo.AssAtendimentoMovimento", "DataFinal", name: "Ate_Idx_DataFinal");
            CreateIndex("dbo.AteBalancoHidricos", "DataBalancoHidrico", name: "Ate_Idx_DataBalancoHidrico");
            CreateIndex("dbo.LoteValidade", "Validade", name: "Est_Idx_Validade");
            CreateIndex("dbo.EstoqueMovimento", "Movimento", name: "Est_Idx_Movimento");
            CreateIndex("dbo.EstoqueMovimento", "Emissao", name: "Est_Idx_Emissao");
            CreateIndex("dbo.EstoquePreMovimento", "Movimento", name: "Est_Idx_Movimento");
            CreateIndex("dbo.EstoquePreMovimento", "Emissao", name: "Est_Idx_Emissao");
            CreateIndex("dbo.EstInventario", "DataInventario", name: "Est_Idx_DataInventario");
            CreateIndex("dbo.AssPrescricaoMedica", "DataPrescricao", name: "Ate_Idx_DataPrescricao");
            CreateIndex("dbo.AssSolicitacaoExameItem", "DataValidade", name: "Ate_Idx_DataValidade");
            CreateIndex("dbo.AssSolicitacaoExame", "DataSolicitacao", name: "Ate_Idx_DataSolicitacao");
            CreateIndex("dbo.AteSenha", "DataHora", name: "Ate_Idx_DataHora");
            CreateIndex("dbo.AteSenhaMov", "DataHora", name: "Ate_Idx_DataHora");
            CreateIndex("dbo.AteSenhaMov", "DataHoraInicial", name: "Ate_Idx_DataHoraInicial");
            CreateIndex("dbo.AteSenhaMov", "DataHoraFinal", name: "Ate_Idx_DataHoraFinal");
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "DataSolicitacao", name: "Ate_Idx_DataSolicitacao");
            CreateIndex("dbo.AssSolicitacaoAntimicrobianos", "DataMaximaTempoProvavel", name: "Ate_Idx_DataMaximaTempoProvavel");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", "Ate_Idx_DataMaximaTempoProvavel");
            DropIndex("dbo.AssSolicitacaoAntimicrobianos", "Ate_Idx_DataSolicitacao");
            DropIndex("dbo.AteSenhaMov", "Ate_Idx_DataHoraFinal");
            DropIndex("dbo.AteSenhaMov", "Ate_Idx_DataHoraInicial");
            DropIndex("dbo.AteSenhaMov", "Ate_Idx_DataHora");
            DropIndex("dbo.AteSenha", "Ate_Idx_DataHora");
            DropIndex("dbo.AssSolicitacaoExame", "Ate_Idx_DataSolicitacao");
            DropIndex("dbo.AssSolicitacaoExameItem", "Ate_Idx_DataValidade");
            DropIndex("dbo.AssPrescricaoMedica", "Ate_Idx_DataPrescricao");
            DropIndex("dbo.EstInventario", "Est_Idx_DataInventario");
            DropIndex("dbo.EstoquePreMovimento", "Est_Idx_Emissao");
            DropIndex("dbo.EstoquePreMovimento", "Est_Idx_Movimento");
            DropIndex("dbo.EstoqueMovimento", "Est_Idx_Emissao");
            DropIndex("dbo.EstoqueMovimento", "Est_Idx_Movimento");
            DropIndex("dbo.LoteValidade", "Est_Idx_Validade");
            DropIndex("dbo.AteBalancoHidricos", "Ate_Idx_DataBalancoHidrico");
            DropIndex("dbo.AssAtendimentoMovimento", "Ate_Idx_DataFinal");
            DropIndex("dbo.AssAtendimentoMovimento", "Ate_Idx_DataInicio");
            DropIndex("dbo.AteAtendimentoLeitoMov", "Ate_Idx_DataInclusao");
            DropIndex("dbo.AteAtendimentoLeitoMov", "Ate_Idx_DataFinal");
            DropIndex("dbo.AteAtendimentoLeitoMov", "Ate_Idx_DataInicial");
            DropIndex("dbo.AteAtendimento", "Ate_Idx_DataPrevistaAlta");
            DropIndex("dbo.AteAtendimento", "Ate_Idx_DataAlta");
            DropIndex("dbo.AteAtendimento", "Ate_Idx_DataRegistro");
        }
    }
}
