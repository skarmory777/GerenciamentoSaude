using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto
{
    [AutoMap(typeof(ElementoHtmlTipo))]
    public class ElementoHtmlTipoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string HtmlHelper { get; set; }

        public static ElementoHtmlTipoDto Mapear(ElementoHtmlTipo entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<ElementoHtmlTipoDto>(entity);
            dto.HtmlHelper = entity.HtmlHelper;
            
            return dto;
        }

        public static ElementoHtmlTipo Mapear(ElementoHtmlTipoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<ElementoHtmlTipo>(dto);
            entity.HtmlHelper = dto.HtmlHelper;

            return entity;
        }
    }
}
