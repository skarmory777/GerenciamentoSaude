using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto
{
    [AutoMap(typeof(Religiao))]
    public class ReligiaoDto : CamposPadraoCRUDDto
    {

        public static ReligiaoDto Mapear(Religiao entity)
        {
            return MapearBase<ReligiaoDto>(entity);
        }

        public static Religiao Mapear(ReligiaoDto dto)
        {
            return MapearBase<Religiao>(dto);
        }
    }
}
