using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros
{
    [AutoMap(typeof(TipoFrete))]
    public class TipoFreteDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }


        public static TipoFreteDto Maprear(TipoFrete tipoFrete)
        {
            var tipoFreteDto = new TipoFreteDto();

            tipoFreteDto.Id = tipoFrete.Id;
            tipoFreteDto.Codigo = tipoFrete.Codigo;
            tipoFreteDto.Descricao = tipoFrete.Descricao;

            return tipoFreteDto;
        }

    }
}
