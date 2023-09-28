using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto
{
    [AutoMap(typeof(FaturamentoBrasApresentacao))]
    public class FaturamentoBrasApresentacaoDto : CamposPadraoCRUDDto
    {
        //public string Codigo { get; set; }

        //public string Descricao { get; set; }

        public float Quantidade { get; set; }

        public static FaturamentoBrasApresentacaoDto Mapear(FaturamentoBrasApresentacao faturamentoBrasApresentacao)
        {
            if (faturamentoBrasApresentacao == null)
            {
                return null;
            }

            var faturamentoBrasApresentacaoDto = MapearBase<FaturamentoBrasApresentacaoDto>(faturamentoBrasApresentacao);
            faturamentoBrasApresentacaoDto.Quantidade = faturamentoBrasApresentacao.Quantidade;

            return faturamentoBrasApresentacaoDto;
        }

        public static FaturamentoBrasApresentacao Mapear(FaturamentoBrasApresentacaoDto faturamentoBrasApresentacaoDto)
        {
            if (faturamentoBrasApresentacaoDto == null)
            {
                return null;
            }

            var faturamentoBrasApresentacao = MapearBase<FaturamentoBrasApresentacao>(faturamentoBrasApresentacaoDto);
            faturamentoBrasApresentacao.Quantidade = faturamentoBrasApresentacaoDto.Quantidade;

            return faturamentoBrasApresentacao;
        }

    }
}
