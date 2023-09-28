using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    [AutoMap(typeof(AgendamentoConsulta))]
    public class CriarOuEditarAgendamentoConsulta : CamposPadraoCRUDDto
    {
        public long? AgendamentoConsultaMedicoDisponibilidadeId { get; set; }

        public long MedicoId { get; set; }

        public long MedicoEspecialidadeId { get; set; }

        public long? PacienteId { get; set; }

        public long? ConvenioId { get; set; }

        public long? PlanoId { get; set; }

        public DateTime DataAgendamento { get; set; }

        public DateTime HoraAgendamento { get; set; }

        public string Notas { get; set; }

        public string NomeReservante { get; set; }

        public DateTime? DataNascimentoReservante { get; set; }

        public string TelefoneReservante { get; set; }

        public long? ConvenioReservante { get; set; }

        public long? PlanoReservante { get; set; }

        //[ForeignKey("MedicoId")]
        public virtual MedicoDto Medico { get; set; }

        //[ForeignKey("MedicoEspecialidadeId")]
        public virtual MedicoEspecialidadeDto MedicoEspecialidade { get; set; }

        //[ForeignKey("PacienteId")]
        public virtual PacienteDto Paciente { get; set; }

        //[ForeignKey("ConvenioId")]
        public virtual ConvenioDto Convenio { get; set; }

        //[ForeignKey("PlanoId")]
        public virtual PlanoDto Plano { get; set; }

        //[ForeignKey("AgendamentoConsultaMedicoDisponibilidadeId")]
        public virtual AgendamentoConsultaMedicoDisponibilidadeDto AgendamentoConsultaMedicoDisponibilidade { get; set; }

        public int QuantidadeHorarios { get; set; }

    }
}
