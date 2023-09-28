using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(EstadoCivil))]
    public class EstadoCivilDto : CamposPadraoCRUDDto
    {

        public static EstadoCivilDto Mapear(EstadoCivil input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<EstadoCivilDto>(input);
        }

        public static EstadoCivil Mapear(EstadoCivilDto input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<EstadoCivil>(input);
        }
    }
}
