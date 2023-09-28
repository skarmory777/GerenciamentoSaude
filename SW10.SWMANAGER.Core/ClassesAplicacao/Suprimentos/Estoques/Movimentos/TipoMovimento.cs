using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{

    [Table("EstTipoMovimento")]
    public class TipoMovimento : CamposPadraoCRUD
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public override long Id { get; set; }
        public bool IsEntrada { get; set; }
        public bool IsOrdemCompra { get; set; }
        public bool IsPessoa { get; set; }
        public bool IsOrdemCompraObrigatoria { get; set; }
        public bool IsFiscal { get; set; }
        public bool IsFrete { get; set; }
        public bool IsFinanceiro { get; set; }
    }
}
