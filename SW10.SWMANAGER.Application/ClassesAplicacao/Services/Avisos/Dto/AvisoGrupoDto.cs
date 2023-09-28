using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using SW10.SWMANAGER.Authorization.Roles.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Avisos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto
{
    public class AvisoGrupoDto : CamposPadraoCRUDDto
    {
        public long AvisoId { get; set; }
        public Aviso Aviso { get; set; }
        public int RoleId { get; set; }
        public RoleListDto Role { get; set; }

        public static AvisoGrupo Mapear(AvisoGrupoDto dto)
        {
            var entity = MapearBase<AvisoGrupo>(dto);
            entity.AvisoId = dto.AvisoId;
            entity.RoleId = dto.RoleId;
            return entity;
        }
        
        public static AvisoGrupoDto Mapear(AvisoGrupo entity)
        {
            var dto = MapearBase<AvisoGrupoDto>(entity);
            dto.AvisoId = entity.AvisoId;
            dto.RoleId = entity.RoleId;
            return dto;
        }

        public static ICollection<AvisoGrupo> MapearList(IEnumerable<AvisoGrupoDto> dtos)
        {
            return dtos.IsNullOrEmpty() ? null : dtos.Select(Mapear).ToList();
        }
        
        public static ICollection<AvisoGrupoDto> MapearList(IEnumerable<AvisoGrupo> entities)
        {
            return entities.IsNullOrEmpty() ? null : entities.Select(Mapear).ToList();
        }
    }
}