using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto
{
    [AutoMap(typeof(GuiaCampo))]
    public class CriarOuEditarGuiaCampo : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public float CoordenadaX { get; set; }

        public float CoordenadaY { get; set; }

        public bool IsConjunto { get; set; }

        public bool IsSubItem { get; set; }

        public long? ConjuntoId { get; set; }

        public int? MaximoElementos { get; set; }

        public GuiaCampoDto[] SubConjuntos { get; set; }

        public static CriarOuEditarGuiaCampo Mapear(GuiaCampo entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CriarOuEditarGuiaCampo>(entity);
            dto.CoordenadaX = entity.CoordenadaX;
            dto.CoordenadaY = entity.CoordenadaY;
            dto.IsConjunto = entity.IsConjunto;
            dto.IsSubItem = entity.IsSubItem;
            dto.ConjuntoId = entity.ConjuntoId;

            return dto;
        }
    }
}