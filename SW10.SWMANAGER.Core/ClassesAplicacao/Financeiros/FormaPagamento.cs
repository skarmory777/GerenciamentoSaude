using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinFormaPagamento")]
    public class FormaPagamento : CamposPadraoCRUD
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
        public long? CodigoBionexo { get; set; }
    }
}
