using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto
{
    [AutoMap(typeof(AgendamentoConsultaMedicoDisponibilidade))]
    public class AgendamentoConsultaMedicoDisponibilidadeDto : CamposPadraoCRUDDto
    {
        public long MedicoId { get; set; }

        public long MedicoEspecialidadeId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime HoraInicio { get; set; }

        public DateTime HoraFim { get; set; }

        public long IntervaloId { get; set; }

        public bool Domingo { get; set; }

        public bool Segunda { get; set; }

        public bool Terca { get; set; }

        public bool Quarta { get; set; }

        public bool Quinta { get; set; }

        public bool Sexta { get; set; }

        public bool Sabado { get; set; }

        //[ForeignKey("MedicoId")]
        public virtual MedicoDto Medico { get; set; }

        //[ForeignKey("MedicoEspecialidadeId")]
        public virtual MedicoEspecialidadeDto MedicoEspecialidade { get; set; }

        //[ForeignKey("IntervaloId")]
        public virtual IntervaloDto Intervalo { get; set; }

        public static AgendamentoConsultaMedicoDisponibilidadeDto Mapear(
            AgendamentoConsultaMedicoDisponibilidade entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AgendamentoConsultaMedicoDisponibilidadeDto
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

                MedicoId = entity.MedicoId,
                MedicoEspecialidadeId = entity.MedicoEspecialidadeId,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim,
                HoraInicio = entity.HoraInicio,
                HoraFim = entity.HoraFim,
                IntervaloId = entity.IntervaloId,
                Domingo = entity.Domingo,
                Segunda = entity.Segunda,
                Terca = entity.Terca,
                Quarta = entity.Quarta,
                Quinta = entity.Quinta,
                Sexta = entity.Sexta,
                Sabado = entity.Sabado,
                Intervalo = IntervaloDto.Mapear(entity.Intervalo),
                Medico = MedicoDto.Mapear(entity.Medico),
                MedicoEspecialidade = MedicoEspecialidadeDto.Mapear(entity.MedicoEspecialidade),
            };
        }

        public static List<AgendamentoConsultaMedicoDisponibilidadeDto> Mapear(List<AgendamentoConsultaMedicoDisponibilidade> entityList)
        {
            var dtoList = new List<AgendamentoConsultaMedicoDisponibilidadeDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }
    }
}
