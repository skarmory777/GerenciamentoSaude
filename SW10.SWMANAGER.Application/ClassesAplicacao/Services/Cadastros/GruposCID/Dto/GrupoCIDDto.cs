using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto
{
    [AutoMap(typeof(GrupoCID))]
    public class GrupoCIDDto : CamposPadraoCRUDDto
    {
        public static GrupoCIDDto Mapear(GrupoCID grupoCID)
        {
            return MapearBase<GrupoCIDDto>(grupoCID);
        }

        public static GrupoCID Mapear(GrupoCIDDto grupoCIDDto)
        {
            return MapearBase<GrupoCID>(grupoCIDDto);
        }
    }
}
