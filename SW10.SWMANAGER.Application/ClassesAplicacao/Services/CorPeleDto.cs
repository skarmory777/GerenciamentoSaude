using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(CorPele))]
    public class CorPeleDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static CorPeleDto Mapear(CorPele input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<CorPeleDto>(input);
        }

        public static CorPele Mapear(CorPeleDto input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<CorPele>(input);
        }
    }
}
