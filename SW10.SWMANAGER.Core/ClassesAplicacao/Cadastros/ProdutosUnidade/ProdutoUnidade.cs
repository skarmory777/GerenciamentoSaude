using SW10.SWMANAGER.ClassesAplicacao;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosUnidade
{
    [Table("ProdutoUnidade")]
    public class ProdutoUnidade : CamposPadraoCRUD
    {
        [StringLength(10)]
        public string Nome { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(10)]
        public string Fator { get; set; }

    }
}
