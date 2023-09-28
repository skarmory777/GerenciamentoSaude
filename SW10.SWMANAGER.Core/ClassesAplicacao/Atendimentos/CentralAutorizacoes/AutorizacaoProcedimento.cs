using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes
{
    [Table("AteAutorizacaoProcedimento")]
    public class AutorizacaoProcedimento : CamposPadraoCRUD
    {
        public long? SolicitanteId { get; set; }
        public long? PacienteId { get; set; }
        public long? AtendimentoId { get; set; }
        public long ConvenioId { get; set; }

        [Index("Ate_Idx_DataSolicitacao")]
        public DateTime DataSolicitacao { get; set; }
        public string Observacao { get; set; }
        public string NumeroGuia { get; set; }

        [ForeignKey("SolicitanteId")]
        public Medico Solicitante { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        public bool IsProrrogacao { get; set; }

    }
}
