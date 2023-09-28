using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(TipoOperacao))]
    public class TipoOperacaoDto : CamposPadraoCRUDDto
    {
        public TipoOperacaoDto() { }

        public TipoOperacaoDto(TipoOperacao tipoOperacao)
        {

        }

        //public string Descricao { get; set; }

        public static TipoOperacaoDto Mapear(TipoOperacao tipoOperacao)
        {
            var tipoOperacaoDto = new TipoOperacaoDto();

            tipoOperacaoDto.Id = tipoOperacao.Id;
            tipoOperacaoDto.Codigo = tipoOperacao.Codigo;
            tipoOperacaoDto.Descricao = tipoOperacao.Descricao;

            return tipoOperacaoDto;
        }

        public static TipoOperacao Mapear(TipoOperacaoDto tipoOperacaoDto)
        {
            var tipoOperacao = new TipoOperacao();

            tipoOperacao.Id = tipoOperacaoDto.Id;
            tipoOperacao.Codigo = tipoOperacaoDto.Codigo;
            tipoOperacao.Descricao = tipoOperacaoDto.Descricao;

            return tipoOperacao;
        }
    }
}
