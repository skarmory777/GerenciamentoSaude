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
    public class AgendamentoConsultaDto : CamposPadraoCRUDDto
    {
        public long? AgendamentoConsultaMedicoDisponibilidadeId { get; set; }

        public long? MedicoId { get; set; }

        public long? MedicoEspecialidadeId { get; set; }

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

        public long? StatusId { get; set; }

        public AgendamentoStatusDto AgendamentoStatus { get; set; }


        public static AgendamentoConsultaDto Mapear(AgendamentoConsulta entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AgendamentoConsultaDto
            {
                Id = entity.Id,
                ImportaId = entity.ImportaId,
                Codigo = entity.Codigo,
                Descricao = entity.Descricao,
                CreationTime = entity.CreationTime,
                CreatorUserId = entity.CreatorUserId,
                DeleterUserId = entity.DeleterUserId,
                DeletionTime = entity.DeletionTime,
                IsDeleted = entity.IsDeleted,
                IsSistema = entity.IsSistema,
                LastModificationTime = entity.LastModificationTime,
                LastModifierUserId = entity.LastModifierUserId,

                DataAgendamento = entity.DataAgendamento,
                HoraAgendamento = entity.HoraAgendamento,
                Notas = entity.Notas,
                NomeReservante = entity.NomeReservante,
                DataNascimentoReservante = entity.DataNascimentoReservante,
                MedicoId = entity.MedicoId,
                Medico = MedicoDto.Mapear(entity.Medico),
                MedicoEspecialidade = MedicoEspecialidadeDto.Mapear(entity.MedicoEspecialidade),
                MedicoEspecialidadeId = entity.MedicoEspecialidadeId,
                PacienteId = entity.PacienteId,
                Paciente = PacienteDto.Mapear(entity.Paciente),
                Plano = PlanoDto.Mapear(entity.Plano),
                PlanoId = entity.PlanoId,
                AgendamentoConsultaMedicoDisponibilidade =
                                  AgendamentoConsultaMedicoDisponibilidadeDto.Mapear(
                                      entity.AgendamentoConsultaMedicoDisponibilidade),
                AgendamentoConsultaMedicoDisponibilidadeId =
                                  entity.AgendamentoConsultaMedicoDisponibilidadeId,
                StatusId = entity.StatusId,
                AgendamentoStatus = AgendamentoStatusDto.Mapear(entity.AgendamentoStatus),
                ConvenioId = entity.ConvenioId,
                Convenio = ConvenioDto.Mapear(entity.Convenio),
                ConvenioReservante = entity.ConvenioReservante,
                PlanoReservante = entity.PlanoReservante,
                TelefoneReservante = entity.TelefoneReservante,

            };
        }
    }
}
