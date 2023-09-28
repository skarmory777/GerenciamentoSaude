using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLocalizacao
{
    [Table("ProdutoLocalizacao")]
    public class ProdutoLocalizacao : CamposPadraoCRUD
    {
        [StringLength(10)]
        public string Sigla { get; set; }

    }
}
