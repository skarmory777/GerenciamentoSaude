using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias.Dto
{
    [AutoMap(typeof(HoraDia))]
    public class HoraDiaDto : CamposPadraoCRUDDto
    {

        public static IEnumerable<HoraDiaDto> Mapear(List<HoraDia> input)
        {
            foreach (var item in input)
            {
                yield return MapearBase<HoraDiaDto>(item);
            }
        }
    }
}
