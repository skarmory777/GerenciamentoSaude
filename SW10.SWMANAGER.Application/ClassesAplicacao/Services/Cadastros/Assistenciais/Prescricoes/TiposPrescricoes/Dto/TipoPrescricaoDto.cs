using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto
{
    [AutoMap(typeof(TipoPrescricao))]
    public class TipoPrescricaoDto : CamposPadraoCRUDDto
    {

        public static TipoPrescricao Mapear(TipoPrescricaoDto input)
        {
            if (input == null)
            {
                return null;
            }
            return MapearBase<TipoPrescricao>(input);
        }

        public static TipoPrescricaoDto Mapear(TipoPrescricao input)
        {
            if (input == null)
            {
                return null;
            }
            return MapearBase<TipoPrescricaoDto>(input);
        }

        public static List<TipoPrescricaoDto> Mapear(List<TipoPrescricao> entityList)
        {
            var dtoList = new List<TipoPrescricaoDto>();

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
