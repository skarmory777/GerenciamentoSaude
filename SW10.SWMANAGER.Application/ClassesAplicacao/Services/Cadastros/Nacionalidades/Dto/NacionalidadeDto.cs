using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto
{
    [AutoMap(typeof(Nacionalidade))]
    public class NacionalidadeDto : CamposPadraoCRUDDto
    {
        public static NacionalidadeDto Mapear(Nacionalidade nacionalidade)
        {
            if (nacionalidade != null)
            {
                var nacionalidadeDto = MapearBase<NacionalidadeDto>(nacionalidade);

                nacionalidadeDto.Id = nacionalidade.Id;
                nacionalidadeDto.Codigo = nacionalidade.Codigo;
                nacionalidadeDto.Descricao = nacionalidade.Descricao;

                return nacionalidadeDto;
            }
            return null;
        }


        public static Nacionalidade Mapear(NacionalidadeDto nacionalidadeDto)
        {
            if (nacionalidadeDto != null)
            {
                var Nacionalidade = new Nacionalidade();

                Nacionalidade.Id = nacionalidadeDto.Id;
                Nacionalidade.Codigo = nacionalidadeDto.Codigo;
                Nacionalidade.Descricao = nacionalidadeDto.Descricao;

                return Nacionalidade;
            }
            return null;
        }


    }
}
