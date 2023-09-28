using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto
{
    [AutoMap(typeof(TipoLogradouro))]
    public class TipoLogradouroDto : CamposPadraoCRUDDto
    {
        public string Abreviacao { get; set; }

        public string Descricao { get; set; }


        public static TipoLogradouroDto Mapear(TipoLogradouro TipoLogradouro)
        {
            if (TipoLogradouro == null)
            {
                return null;
            }

            var TipoLogradouroDto = MapearBase<TipoLogradouroDto>(TipoLogradouro);
            TipoLogradouroDto.Abreviacao = TipoLogradouro.Abreviacao;

            return TipoLogradouroDto;
        }

        public static TipoLogradouro Mapear(TipoLogradouroDto TipoLogradouroDto)
        {
            if (TipoLogradouroDto == null)
            {
                return null;
            }

            var TipoLogradouro = MapearBase<TipoLogradouro>(TipoLogradouroDto);
            TipoLogradouro.Abreviacao = TipoLogradouroDto.Abreviacao;

            return TipoLogradouro;
        }


    }
}
