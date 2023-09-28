using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(Sexo))]
    public class SexoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static SexoDto Mapear(Sexo sexo)
        {
            if (sexo == null)
            {
                return null;
            }

            return MapearBase<SexoDto>(sexo);
        }

        public static Sexo Mapear(SexoDto sexoDto)
        {
            if (sexoDto == null)
            {
                return null;
            }

            return MapearBase<Sexo>(sexoDto);
        }
    }
}
