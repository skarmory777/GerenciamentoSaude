using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto
{
    [AutoMap(typeof(FaturamentoSubGrupo))]
    public class FaturamentoSubGrupoDto : CamposPadraoCRUDDto
    {
        public virtual FaturamentoGrupoDto Grupo { get; set; }
        public long GrupoId { get; set; }

        public bool IsLaboratorio { get; set; }
        public bool IsLaudo { get; set; }

        public static FaturamentoSubGrupoDto Mapear(FaturamentoSubGrupo entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = MapearBase<FaturamentoSubGrupoDto>(entity);

            dto.GrupoId = entity.GrupoId;

            dto.IsLaboratorio = entity.IsLaboratorio;

            dto.IsLaudo = entity.IsLaudo;

            dto.Grupo = FaturamentoGrupoDto.Mapear(entity.Grupo);

            return dto;
        }

        public static FaturamentoSubGrupo Mapear(FaturamentoSubGrupoDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            var entity = MapearBase<FaturamentoSubGrupo>(dto);

            entity.GrupoId = dto.GrupoId;

            entity.IsLaboratorio = dto.IsLaboratorio;

            entity.IsLaudo = dto.IsLaudo;

            entity.Grupo = FaturamentoGrupoDto.Mapear(dto.Grupo);

            return entity;
        }
    }
}
