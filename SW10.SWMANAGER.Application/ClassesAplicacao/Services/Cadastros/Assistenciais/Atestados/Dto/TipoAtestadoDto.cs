using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto
{
    [AutoMap(typeof(TipoAtestado))]
    public class TipoAtestadoDto : CamposPadraoCRUDDto
    {
        public static TipoAtestado Mapear(TipoAtestadoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<TipoAtestado>(dto);

            return entity;
        }

        public static TipoAtestadoDto Mapear(TipoAtestado entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<TipoAtestadoDto>(entity);

            return dto;
        }

        public static List<TipoAtestadoDto> Mapear(List<TipoAtestado> entityList)
        {
            var dtoList = new List<TipoAtestadoDto>();

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
