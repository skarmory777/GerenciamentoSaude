using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosClasse;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosGrupo
{
    [Table("ProdutoGrupo")]
    public class ProdutoGrupo : CamposPadraoCRUD
    {
        public ICollection<ProdutoClasse> ProdutosClasses { get; set; }
    }
}
