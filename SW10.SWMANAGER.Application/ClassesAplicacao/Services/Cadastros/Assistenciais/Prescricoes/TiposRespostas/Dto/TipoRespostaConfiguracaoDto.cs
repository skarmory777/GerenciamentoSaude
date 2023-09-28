using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto
{
    [AutoMap(typeof(TipoRespostaConfiguracao))]
    public class TipoRespostaConfiguracaoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
        public ICollection<TipoRespostaConfiguracaoElementoHtmlDto> ElementosHtml { get; set; }

        public static TipoRespostaConfiguracaoDto Mapear(TipoRespostaConfiguracao entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<TipoRespostaConfiguracaoDto>(entity);
            dto.ElementosHtml = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(entity.ElementosHtml.ToList());

            return dto;
        }

        public static List<TipoRespostaConfiguracaoDto> Mapear(List<TipoRespostaConfiguracao> entityList)
        {
            var dtoList = new List<TipoRespostaConfiguracaoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }

        public static TipoRespostaConfiguracao Mapear(TipoRespostaConfiguracaoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<TipoRespostaConfiguracao>(dto);
            entity.ElementosHtml = TipoRespostaConfiguracaoElementoHtmlDto.Mapear(dto.ElementosHtml.ToList());

            return entity;
        }
    }
}
