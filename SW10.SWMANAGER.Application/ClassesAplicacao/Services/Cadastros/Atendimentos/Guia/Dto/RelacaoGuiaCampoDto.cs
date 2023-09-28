using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto
{
    [AutoMap(typeof(RelacaoGuiaCampo))]
    public class RelacaoGuiaCampoDto : CamposPadraoCRUDDto
    {
        public float CoordenadaX { get; set; }

        public float CoordenadaY { get; set; }

        public long GuiaId { get; set; }
        [ForeignKey("GuiaId")]
        public virtual GuiaDto Guia { get; set; }

        public long GuiaCampoId { get; set; }
        [ForeignKey("GuiaCampoId")]
        public virtual GuiaCampoDto GuiaCampo { get; set; }

        public static RelacaoGuiaCampo Mapear(CriarOuEditarRelacaoGuiaCampo dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<RelacaoGuiaCampo>(dto);
            entity.CoordenadaX = dto.CoordenadaX;
            entity.CoordenadaY = dto.CoordenadaY;
            entity.GuiaId = dto.GuiaId;
            entity.GuiaCampoId = dto.GuiaCampoId;

            return entity;
        }

        public static CriarOuEditarRelacaoGuiaCampo Mapear(RelacaoGuiaCampo entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<CriarOuEditarRelacaoGuiaCampo>(entity);
            dto.CoordenadaX = entity.CoordenadaX;
            dto.CoordenadaY = entity.CoordenadaY;
            dto.GuiaId = entity.GuiaId;
            dto.GuiaCampoId = entity.GuiaCampoId;

            return dto;
        }
    }
}
