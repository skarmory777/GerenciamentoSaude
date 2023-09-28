using System;
using System.ComponentModel.DataAnnotations.Schema;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos
{
    [Table(("FatItemAtendimento"))]
    public class FaturamentoItemAtendimento : CamposPadraoCRUD
    {
        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long AtendimentoId { get; set; }
        
        public Atendimento Atendimento { get; set; }
        
        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }
        
        public Medico Medico { get; set; }
        
        [ForeignKey("Leito"), Column("AteLeitoId")]
        public long? LeitoId { get; set; }
        
        public Leito Leito { get; set; }
        
        [Index("Fat_Idx_Data")]
        public DateTime Data { get; set; }
        
        [ForeignKey("FaturamentoItem"), Column("FaturamentoItemId")]
        public long? FaturamentoItemId { get; set; }
        
        public FaturamentoItem FaturamentoItem { get; set; }
        
        public decimal? Quantidade { get; set; }
        
        public string Entidade { get; set; }
        
        public long EntidadeId { get; set; }
        
    }
}