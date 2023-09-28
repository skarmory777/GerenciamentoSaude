using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(Escolaridade))]
    public class EscolaridadeDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static EscolaridadeDto Mapear(Escolaridade input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<EscolaridadeDto>(input);
        }

        public static Escolaridade Mapear(EscolaridadeDto input)
        {
            if (input == null)
            {
                return null;
            }

            return MapearBase<Escolaridade>(input);
        }
    }
}
