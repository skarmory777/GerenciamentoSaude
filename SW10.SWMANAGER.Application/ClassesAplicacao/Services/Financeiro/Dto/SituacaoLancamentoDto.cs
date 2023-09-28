using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(SituacaoLancamento))]
    public class SituacaoLancamentoDto : CamposPadraoCRUDDto
    {
        public bool IsPermiteAlteracao { get; set; }
        public string CorLancamentoFundo { get; set; }
        public string CorLancamentoLetra { get; set; }
    }
}
