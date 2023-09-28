using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    [Table("AgendamentoConsulta")]
    public class AgendamentoConsulta : CamposPadraoCRUD
    {
        public long? AgendamentoConsultaMedicoDisponibilidadeId { get; set; }

        public long? MedicoId { get; set; }

        public long? MedicoEspecialidadeId { get; set; }

        public long? PacienteId { get; set; }

        public long? ConvenioId { get; set; }

        public long? PlanoId { get; set; }

        [Index("Ate_Idx_DataAgendamento")]
        public DateTime DataAgendamento { get; set; }

        [Index("Ate_Idx_HoraAgendamento")]
        public DateTime HoraAgendamento { get; set; }

        public string Notas { get; set; }

        public string NomeReservante { get; set; }

        [Index("Ate_Idx_DataNascimentoReservante")]
        public DateTime? DataNascimentoReservante { get; set; }

        public string TelefoneReservante { get; set; }

        public long? ConvenioReservante { get; set; }

        public long? PlanoReservante { get; set; }

        [ForeignKey("MedicoId")]
        public Medico Medico { get; set; }

        [ForeignKey("MedicoEspecialidadeId")]
        public MedicoEspecialidade MedicoEspecialidade { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        [ForeignKey("PlanoId")]
        public Plano Plano { get; set; }

        [ForeignKey("AgendamentoConsultaMedicoDisponibilidadeId")]
        public AgendamentoConsultaMedicoDisponibilidade AgendamentoConsultaMedicoDisponibilidade { get; set; }

        public int QuantidadeHorarios { get; set; }

        [Index("Ate_Idx_IsConsulta")]
        public bool IsConsulta { get; set; }
        [Index("Ate_Idx_IsCirurgia")]
        public bool IsCirurgia { get; set; }

        [Index("Ate_Idx_IsExames")]
        public bool IsExames { get; set; }
        public string CPF { get; set; }

        public long? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public AgendamentoStatus AgendamentoStatus { get; set; }

        //public AgendamentoCirurgico AgendamentoCirurgico { get; set; }
    }
}
