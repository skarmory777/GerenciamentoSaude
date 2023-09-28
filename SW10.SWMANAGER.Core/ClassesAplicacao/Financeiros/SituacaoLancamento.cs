using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinSituacaoLancamento")]
    public class SituacaoLancamento : CamposPadraoCRUD
    {
        public bool IsPermiteAlteracao { get; set; }
        public string CorLancamentoFundo { get; set; }
        public string CorLancamentoLetra { get; set; }
    }
}
