using SW10.SWMANAGER.ClassesAplicacao;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosEstoque
{
    [Table("ProdutoEstoque")]
    public class ProdutoEstoque : CamposPadraoCRUD
    {
        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(12)]
        public string Tipo { get; set; }

        [StringLength(20)]
        public string ProcessoCota { get; set; }

        public bool IsCodigoDeBarra { get; set; }

        public bool IsCustoMedio { get; set; }

        public bool IsUtilizaCota { get; set; }

        public bool IsChecarSaldo{ get; set; }

        public bool IsCalcularConsumoDemanda { get; set; }

    }
}
