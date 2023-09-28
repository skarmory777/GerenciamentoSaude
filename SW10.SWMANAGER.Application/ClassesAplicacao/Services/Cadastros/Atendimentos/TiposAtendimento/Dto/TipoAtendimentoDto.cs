using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto
{
    [AutoMap(typeof(TipoAtendimento))]
    public class TipoAtendimentoDto : CamposPadraoCRUDDto
    {
        public bool IsInternacao { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }

        public long? TabelaItemTissId { get; set; }
        public TabelaDominioDto TabelaDominio { get; set; }

        public static TipoAtendimentoDto Mapear(TipoAtendimento tipoAtendimento)
        {
            if (tipoAtendimento == null)
            {
                return null;
            }
            TipoAtendimentoDto tipoAtendimentoDto = MapearBase<TipoAtendimentoDto>(tipoAtendimento);

            tipoAtendimentoDto.Id = tipoAtendimento.Id;
            tipoAtendimentoDto.Codigo = tipoAtendimento.Codigo;
            tipoAtendimentoDto.Descricao = tipoAtendimento.Descricao;
            tipoAtendimentoDto.TabelaItemTissId = tipoAtendimento.TabelaItemTissId;

            if (tipoAtendimento.TabelaDominio != null)
            {
                tipoAtendimentoDto.TabelaDominio = TabelaDominioDto.Mapear(tipoAtendimento.TabelaDominio);
            }

            return tipoAtendimentoDto;
        }

        public static TipoAtendimento Mapear(TipoAtendimentoDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            var entity = MapearBase<TipoAtendimento>(dto);

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;
            entity.TabelaItemTissId = dto.TabelaItemTissId;

            if (dto.TabelaDominio != null)
            {
                entity.TabelaDominio = TabelaDominioDto.Mapear(dto.TabelaDominio);
            }

            return entity;
        }

        public static List<TipoAtendimentoDto> Mapear(List<TipoAtendimento> entityList)
        {
            var dtoList = new List<TipoAtendimentoDto>();

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
