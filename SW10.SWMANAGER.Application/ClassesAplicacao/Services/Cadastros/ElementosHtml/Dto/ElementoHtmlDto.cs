using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto
{
    [AutoMap(typeof(ElementoHtml))]
    public class ElementoHtmlDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public long? ElementoHtmlTipoId { get; set; }

        public int Tamanho { get; set; }

        public bool IsRequerido { get; set; }

        public bool IsDesativado { get; set; }

        public virtual ElementoHtmlTipoDto ElementoHtmlTipo { get; set; }

        public static ElementoHtmlDto Mapear(ElementoHtml entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<ElementoHtmlDto>(entity);
            dto.ElementoHtmlTipoId = entity.ElementoHtmlTipoId;
            dto.Tamanho = entity.Tamanho;
            dto.IsRequerido = entity.IsRequerido;
            dto.IsDesativado = entity.IsDesativado;
            dto.ElementoHtmlTipo = ElementoHtmlTipoDto.Mapear(entity.ElementoHtmlTipo);

            return dto;
        }

        public static ElementoHtml Mapear(ElementoHtmlDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<ElementoHtml>(dto);
            entity.ElementoHtmlTipoId = dto.ElementoHtmlTipoId;
            entity.Tamanho = dto.Tamanho;
            entity.IsRequerido = dto.IsRequerido;
            entity.IsDesativado = dto.IsDesativado;
            entity.ElementoHtmlTipo = ElementoHtmlTipoDto.Mapear(dto.ElementoHtmlTipo);

            return entity;
        }
    }
}
