using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto
{
    [AutoMap(typeof(Pais))]
    public class PaisDto : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }

        public string Sigla { get; set; }


        public static PaisDto Mapear(Pais pais)
        {
            if (pais == null)
            {
                return null;
            }

            var paisDto = MapearBase<PaisDto>(pais);
            paisDto.Nome = pais.Nome;
            paisDto.Sigla = pais.Sigla;

            return paisDto;
        }

        public static Pais Mapear(PaisDto paisDto)
        {
            if (paisDto == null)
            {
                return null;
            }

            var pais = MapearBase<Pais>(paisDto);
            pais.Nome = pais.Nome;
            pais.Sigla = pais.Sigla;

            return pais;
        }

    }
}
