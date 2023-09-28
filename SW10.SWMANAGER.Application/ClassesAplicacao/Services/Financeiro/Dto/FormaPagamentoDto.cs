using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(FormaPagamento))]
    public class FormaPagamentoDto : CamposPadraoCRUDDto
    {
        public int NumeroParcelas { get; set; }
        public decimal PercentualDesconto { get; set; }
        public int? DiasParcela1 { get; set; }
        public int? DiasParcela2 { get; set; }
        public int? DiasParcela3 { get; set; }
        public int? DiasParcela4 { get; set; }
        public int? DiasParcela5 { get; set; }
        public int? DiasParcela6 { get; set; }
        public int? DiasParcela7 { get; set; }
        public int? DiasParcela8 { get; set; }
    }
}
