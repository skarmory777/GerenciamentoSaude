using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto
{
    [AutoMap(typeof(TipoRespostaConfiguracaoElementoHtml))]
    public class TipoRespostaConfiguracaoElementoHtmlDto : CamposPadraoCRUDDto
    {
        public long ElementoHtmlId { get; set; }

        public ElementoHtmlDto ElementoHtml { get; set; }

        public string Rotulo { get; set; }

        public string RotuloPosElemento { get; set; }

        public long TipoRespostaConfiguracaoId { get; set; }

        public TipoRespostaConfiguracaoDto TipoRespostaConfiguracao { get; set; }

        public static TipoRespostaConfiguracaoElementoHtmlDto Mapear(TipoRespostaConfiguracaoElementoHtml entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<TipoRespostaConfiguracaoElementoHtmlDto>(entity);
            dto.ElementoHtmlId = entity.ElementoHtmlId;
            dto.ElementoHtml = ElementoHtmlDto.Mapear(entity.ElementoHtml);
            dto.Rotulo = entity.Rotulo;
            dto.RotuloPosElemento = entity.RotuloPosElemento;
            dto.TipoRespostaConfiguracaoId = entity.TipoRespostaConfiguracaoId;
            dto.TipoRespostaConfiguracao = TipoRespostaConfiguracaoDto.Mapear(entity.TipoRespostaConfiguracao);

            return dto;
        }

        public static List<TipoRespostaConfiguracaoElementoHtmlDto> Mapear(List<TipoRespostaConfiguracaoElementoHtml> entityList)
        {
            var dtoList = new List<TipoRespostaConfiguracaoElementoHtmlDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }

        public static TipoRespostaConfiguracaoElementoHtml Mapear(TipoRespostaConfiguracaoElementoHtmlDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<TipoRespostaConfiguracaoElementoHtml>(dto);
            entity.ElementoHtmlId = dto.ElementoHtmlId;
            entity.ElementoHtml = ElementoHtmlDto.Mapear(dto.ElementoHtml);
            entity.Rotulo = dto.Rotulo;
            entity.RotuloPosElemento = dto.RotuloPosElemento;
            entity.TipoRespostaConfiguracaoId = dto.TipoRespostaConfiguracaoId;
            entity.TipoRespostaConfiguracao = TipoRespostaConfiguracaoDto.Mapear(dto.TipoRespostaConfiguracao);

            return entity;
        }

        public static List<TipoRespostaConfiguracaoElementoHtml> Mapear(List<TipoRespostaConfiguracaoElementoHtmlDto> dtoList)
        {
            var entityList = new List<TipoRespostaConfiguracaoElementoHtml>();

            if (dtoList == null) return null;

            foreach (var item in dtoList)
            {
                var newItemDto = Mapear(item);
                entityList.Add(newItemDto);
            }

            return entityList;
        }
    }
}
