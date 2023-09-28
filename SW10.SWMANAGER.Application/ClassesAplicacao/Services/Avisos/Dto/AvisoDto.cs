using System;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Avisos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto
{
    public class AvisoDto : CamposPadraoCRUDDto
    {
        public string Titulo { get; set; }

        public string Mensagem { get; set; }

        public DateTime?  DataProgramada { get; set; }

        public DateTime? DataInicioDisparo { get; set; }

        public DateTime? DataFinalDisparo { get; set; }

        public bool Bloquear { get; set; }

        public bool DisparoAtivo { get; set; }

        public long TotalEnviado { get; set; }

        public ICollection<AvisoGrupoDto> Grupos { get; set; }

        public static Aviso Mapear(AvisoDto dto)
        {
            var model = MapearBase<Aviso>(dto);
            model.Titulo = dto.Titulo;
            model.Mensagem = dto.Mensagem;
            model.Bloquear = dto.Bloquear;
            model.DisparoAtivo = dto.DisparoAtivo;
            model.TotalEnviado = dto.TotalEnviado;
            model.Grupos = AvisoGrupoDto.MapearList(dto.Grupos);
            model.DataProgramada = dto.DataProgramada;
            model.DataInicioDisparo = dto.DataInicioDisparo;
            model.DataFinalDisparo = dto.DataFinalDisparo;
            return model;
        }
        
        public static AvisoDto Mapear(Aviso model)
        {
            var dto = MapearBase<AvisoDto>(model);
            dto.Titulo = model.Titulo;
            dto.Mensagem = model.Mensagem;
            dto.Bloquear = model.Bloquear;
            dto.DisparoAtivo = model.DisparoAtivo;
            dto.TotalEnviado = model.TotalEnviado;
            dto.Grupos = AvisoGrupoDto.MapearList(model.Grupos);
            dto.DataProgramada = model.DataProgramada;
            dto.DataInicioDisparo = model.DataInicioDisparo;
            dto.DataFinalDisparo = model.DataFinalDisparo;
            return dto;
        }
    }
}