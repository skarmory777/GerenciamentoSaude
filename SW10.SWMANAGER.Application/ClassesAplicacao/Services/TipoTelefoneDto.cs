using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(TipoTelefone))]
    public class TipoTelefoneDto : CamposPadraoCRUDDto
    {

        public static TipoTelefoneDto Mapear(TipoTelefone input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<TipoTelefoneDto>(input);
        }

        public static TipoTelefone Mapear(TipoTelefoneDto input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<TipoTelefone>(input);
        }
    }
}
