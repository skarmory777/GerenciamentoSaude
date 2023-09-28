using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("EstKit")]
    public class EstoqueKit : CamposPadraoCRUD
    {
        public long? ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        public List<EstoqueKitItem> Itens { get; set; }
    }
}
