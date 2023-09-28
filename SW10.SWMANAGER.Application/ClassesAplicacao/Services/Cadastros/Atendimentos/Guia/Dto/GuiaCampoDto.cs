using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto
{
    [AutoMap(typeof(GuiaCampo))]
    public class GuiaCampoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public float CoordenadaX { get; set; }

        public float CoordenadaY { get; set; }

        public bool IsConjunto { get; set; }

        public bool IsSubItem { get; set; }

        public long? ConjuntoId { get; set; }

        public int? MaximoElementos { get; set; }

        public GuiaCampoDto[] SubConjuntos { get; set; }

        public static GuiaCampoDto Mapear(GuiaCampo entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<GuiaCampoDto>(entity);
            dto.CoordenadaX = entity.CoordenadaX;
            dto.CoordenadaY = entity.CoordenadaY;
            dto.IsConjunto = entity.IsConjunto;
            dto.IsSubItem = entity.IsSubItem;
            dto.ConjuntoId = entity.ConjuntoId;

            return dto;
        }

        public static GuiaCampo Mapear(CriarOuEditarGuiaCampo dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<GuiaCampo>(dto);
            entity.CoordenadaX = dto.CoordenadaX;
            entity.CoordenadaY = dto.CoordenadaY;
            entity.IsConjunto = dto.IsConjunto;
            entity.IsSubItem = dto.IsSubItem;
            entity.ConjuntoId = dto.ConjuntoId;
            entity.MaximoElementos = dto.MaximoElementos == null ? 0 : dto.MaximoElementos.Value;

            return entity;
        }

        public static List<GuiaCampoDto> Mapear(List<GuiaCampo> entityList)
        {
            var dtoList = new List<GuiaCampoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }

        public static GuiaCampo Mapear(GuiaCampoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<GuiaCampo>(dto);
            entity.CoordenadaX = dto.CoordenadaX;
            entity.CoordenadaY = dto.CoordenadaY;
            entity.IsConjunto = dto.IsConjunto;
            entity.IsSubItem = dto.IsSubItem;
            entity.ConjuntoId = dto.ConjuntoId;
            entity.MaximoElementos = dto.MaximoElementos == null ? 0 : dto.MaximoElementos.Value;

            return entity;
        }
    }
}