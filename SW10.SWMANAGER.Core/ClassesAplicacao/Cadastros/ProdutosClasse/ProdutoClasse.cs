using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosSubClasse;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosClasse
{
    [Table("ProdutoClasse")]
    public class ProdutoClasse : CamposPadraoCRUD
    {
        public long? GrupoId { get; set; }

        [ForeignKey("GrupoId")]
        public ProdutoGrupo ProdutosGrupos { get; set; }

        public ICollection<ProdutoSubClasse> ProdutosSubClasses { get; set; }
    }
}
