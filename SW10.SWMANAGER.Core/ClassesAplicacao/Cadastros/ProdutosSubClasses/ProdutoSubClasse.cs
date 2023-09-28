using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosClasse;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosSubClasse
{
    [Table("ProdutoSubClasse")]
    public class ProdutoSubClasse : CamposPadraoCRUD
    {
        public long? GrupoClasseId { get; set; }

        [ForeignKey("GrupoClasseId")]
        public ProdutoClasse ProdutosClasses { get; set; }

    }
}
