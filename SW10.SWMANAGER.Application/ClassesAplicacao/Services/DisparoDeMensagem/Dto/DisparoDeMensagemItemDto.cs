namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto
{
    using Abp.Collections.Extensions;
    using SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DisparoDeMensagemItemDto : CamposPadraoCRUDDto
    {
        public string Origem { get; set; }

        public long? OrigemId { get; set; }

        public long? PessoaId { get; set; }

        public SisPessoaDto Pessoa { get; set; }

        public long? DisparoDeMensagemId { get; set; }

        public DisparoDeMensagemDto DisparoDeMensagem { get; set; }

        public long DisparoDeMensagemItemTipoId { get; set; }

        public DisparoDeMensagemItemTipoDto DisparoDeMensagemItemTipo { get; set; }

        public DateTime DataProgramada { get; set; }

        public DateTime? DataInicioDisparo { get; set; }

        public DateTime? DataFinalDisparo { get; set; }

        public DateTime? DataRecebimento { get; set; }

        public string Mensagem { get; set; }

        public string Titulo { get; set; }

        public string Valor { get; set; }

        public static ICollection<DisparoDeMensagemItem> MapearLista(ICollection<DisparoDeMensagemItemDto> dto)
        {
            if (dto.IsNullOrEmpty())
            {
                return null;
            }

            return dto.Select(x => Mapear(x)).ToList();
        }

        public static ICollection<DisparoDeMensagemItemDto> MapearListaEntity(ICollection<DisparoDeMensagemItem> input)
        {
            if (input.IsNullOrEmpty())
            {
                return null;
            }

            return input.Select(x => MapearEntity(x)).ToList();
        }

        public static DisparoDeMensagemItem Mapear(DisparoDeMensagemItemDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var model = MapearBase<DisparoDeMensagemItem>(dto);
            model.DisparoDeMensagemId = dto.DisparoDeMensagemId;

            model.Origem = dto.Origem;
            model.OrigemId = dto.OrigemId;
            model.DataProgramada = dto.DataProgramada;
            model.DataInicioDisparo = dto.DataInicioDisparo;
            model.DataFinalDisparo = dto.DataFinalDisparo;
            model.DataRecebimento = dto.DataRecebimento;
            model.Mensagem = dto.Mensagem;
            model.Titulo = dto.Titulo;
            model.Valor = dto.Valor;

            model.PessoaId = dto.PessoaId;
            if (dto.Pessoa != null)
            {
                model.Pessoa = SisPessoaDto.Mapear(dto.Pessoa);
            }

            model.DisparoDeMensagemItemTipoId = dto.DisparoDeMensagemItemTipoId;

            if (dto.DisparoDeMensagemItemTipo != null)
            {
                model.DisparoDeMensagemItemTipo = MapearBase<DisparoDeMensagemItemTipo>(dto.DisparoDeMensagemItemTipo);
            }

            return model;
        }

        public static DisparoDeMensagemItemDto MapearEntity(DisparoDeMensagemItem dto)
        {
            if (dto == null)
            {
                return null;
            }

            var model = MapearBase<DisparoDeMensagemItemDto>(dto);
            model.DisparoDeMensagemId = dto.DisparoDeMensagemId;

            model.Origem = dto.Origem;
            model.OrigemId = dto.OrigemId;
            model.DataProgramada = dto.DataProgramada;
            model.DataInicioDisparo = dto.DataInicioDisparo;
            model.DataFinalDisparo = dto.DataFinalDisparo;
            model.DataRecebimento = dto.DataRecebimento;
            model.Mensagem = dto.Mensagem;
            model.Titulo = dto.Titulo;
            model.Valor = dto.Valor;

            model.PessoaId = dto.PessoaId;
            if (dto.Pessoa != null)
            {
                model.Pessoa = SisPessoaDto.Mapear(dto.Pessoa);
            }

            model.DisparoDeMensagemItemTipoId = dto.DisparoDeMensagemItemTipoId;

            if (dto.DisparoDeMensagemItemTipo != null)
            {
                model.DisparoDeMensagemItemTipo = MapearBase<DisparoDeMensagemItemTipoDto>(dto.DisparoDeMensagemItemTipo);
            }

            return model;
        }
    }
}
