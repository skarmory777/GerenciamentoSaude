using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SolicitacaoAutorizacoes
{
    public class SolicitacaoAutorizacaoItemDto : CamposPadraoCRUDDto
    {
        public long? SolicitacaoAutorizacaoStatusId { get; set; }

        public SolicitacaoAutorizacaoStatusDto SolicitacaoAutorizacaoStatus { get; set; }
        public long? SolicitacaoAutorizacaoId { get; set; }

        public SolicitacaoAutorizacaoDto SolicitacaoAutorizacao { get; set; }

        public long? AtendimentoId { get; set; }

        public long? MedicoId { get; set; }
        
        public AtendimentoDto Atendimento { get; set; }

        public long? PrescricaoId { get; set; }

        public PrescricaoMedicaDto Prescricao { get; set; }
        
        public long? PrescricaoItemId { get; set; }
        
        public PrescricaoItemDto PrescricaoItem { get; set; }

        public MedicoDto Medico { get; set; }

        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public string ResultadoExames { get; set; }

        public string Indicacao { get; set; }
        
        public string Posologia { get; set; }
        
        public string Mensagem { get; set; }
    }


    public class SolicitacaoAutorizacaoListDto
    {
        public List<SolicitacaoAutorizacaoItemDto> SolicitacaoAutorizacoes { get; set; }
    }
    
    public class SolicitacaoAutorizacoesViewModel: SolicitacaoAutorizacaoListDto
    {
        public long? PrescricaoId { get; set; }
        public long AtendimentoId { get; set; }
    }
    
    
    public class ResultSolicitacaoAutorizacaoDto
    {
        public bool Successo { get; set; }
        public List<long> Ids { get; set; } = new List<long>();
    }
}
