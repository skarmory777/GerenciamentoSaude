using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto
{
    [AutoMap(typeof(TipoControle))]
    public class TipoControleDto : CamposPadraoCRUDDto, IDescricao
    {
        public static TipoControle Mapear(TipoControleDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<TipoControle>(dto);

            return entity;
        }

        public static TipoControleDto Mapear(TipoControle entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<TipoControleDto>(entity);

            return dto;
        }

        public static List<TipoControleDto> Mapear(List<TipoControle> entityList)
        {
            var dtoList = new List<TipoControleDto>();

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
