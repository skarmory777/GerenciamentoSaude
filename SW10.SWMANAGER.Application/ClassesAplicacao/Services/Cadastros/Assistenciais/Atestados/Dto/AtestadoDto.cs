using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto
{
    [AutoMap(typeof(Atestado))]
    public class AtestadoDto : CamposPadraoCRUDDto
    {
        public DateTime DataAtendimento { get; set; }
        public string Conteudo { get; set; }
        public long? MedicoId { get; set; }
        public long? PacienteId { get; set; }
        public long? TipoAtestadoId { get; set; }
        public long? ModeloAtestadoId { get; set; }
        public virtual MedicoDto Medico { get; set; }
        public virtual PacienteDto Paciente { get; set; }
        public virtual TipoAtestadoDto TipoAtestado { get; set; }
        public virtual ModeloAtestadoDto ModeloAtestado { get; set; }

        public static AtestadoDto Mapear(Atestado entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<AtestadoDto>(entity);
            dto.DataAtendimento = entity.DataAtendimento;
            dto.Conteudo = entity.Conteudo;
            dto.MedicoId = entity.MedicoId;
            dto.PacienteId = entity.PacienteId;
            dto.TipoAtestadoId = entity.TipoAtestadoId;
            dto.Medico = MedicoDto.Mapear(entity.Medico);
            dto.Paciente = PacienteDto.Mapear(entity.Paciente);
            dto.TipoAtestado = MapearBase<TipoAtestadoDto>(entity.TipoAtestado);
            dto.ModeloAtestado = ModeloAtestadoDto.Mapear(entity.ModeloAtestado);

            return dto;
        }

        public static Atestado Mapear(AtestadoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<Atestado>(dto);
            entity.DataAtendimento = dto.DataAtendimento;
            entity.Conteudo = dto.Conteudo;
            entity.MedicoId = dto.MedicoId;
            entity.PacienteId = dto.PacienteId;
            entity.TipoAtestadoId = dto.TipoAtestadoId;
            entity.Medico = MedicoDto.Mapear(dto.Medico);
            entity.Paciente = PacienteDto.Mapear(dto.Paciente);
            entity.TipoAtestado = MapearBase<TipoAtestado>(dto.TipoAtestado);
            entity.ModeloAtestado = ModeloAtestadoDto.Mapear(dto.ModeloAtestado);

            return entity;
        }

        public static List<AtestadoDto> Mapear(List<Atestado> entityList)
        {
            var dtoList = new List<AtestadoDto>();

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
