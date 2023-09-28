namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto
{
    using System;
    using System.Collections.Generic;
    using SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem;

    public class DisparoDeMensagemDto : CamposPadraoCRUDDto
    {
        public DateTime DataProgramada { get; set; }

        public DateTime? DataInicioDisparo { get; set; }

        public DateTime? DataFinalDisparo { get; set; }

        public bool DisparoAtivo { get; set; }

        public string Mensagem { get; set; }

        public string Titulo { get; set; }

        public long Total { get; set; }

        public long TotalEnviado { get; set; }

        public long TotalRecebido { get; set; }

        public ICollection<DisparoDeMensagemItemDto> DisparoDeMensagemItems { get; set; }


        public static DisparoDeMensagem Mapear(DisparoDeMensagemDto dto)
        {
            var model = MapearBase<DisparoDeMensagem>(dto);

            model.DataProgramada = dto.DataProgramada;
            model.DataInicioDisparo = dto.DataInicioDisparo;
            model.DataFinalDisparo = dto.DataFinalDisparo;
            model.Mensagem = dto.Mensagem;
            model.Titulo = dto.Titulo;
            model.Total = dto.Total;
            model.TotalEnviado = dto.TotalEnviado;
            model.TotalRecebido = dto.TotalRecebido;
            model.DisparoDeMensagemItems = DisparoDeMensagemItemDto.MapearLista(dto.DisparoDeMensagemItems);

            return model;
        }

        public static DisparoDeMensagemDto MapearEntity(DisparoDeMensagem dto)
        {
            var model = MapearBase<DisparoDeMensagemDto>(dto);

            model.DataProgramada = dto.DataProgramada;
            model.DataInicioDisparo = dto.DataInicioDisparo;
            model.DataFinalDisparo = dto.DataFinalDisparo;
            model.Mensagem = dto.Mensagem;
            model.Titulo = dto.Titulo;
            model.Total = dto.Total;
            model.TotalEnviado = dto.TotalEnviado;
            model.TotalRecebido = dto.TotalRecebido;
            model.DisparoDeMensagemItems = DisparoDeMensagemItemDto.MapearListaEntity(dto.DisparoDeMensagemItems);

            return model;
        }
    }
}
