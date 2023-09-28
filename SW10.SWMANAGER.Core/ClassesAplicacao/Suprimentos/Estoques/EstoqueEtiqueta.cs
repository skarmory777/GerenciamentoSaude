using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("EstEtiqueta")]
    public class EstoqueEtiqueta : CamposPadraoCRUD
    {
        public long? ProdutoId { get; set; }
        public long? LoteValidadeId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("LoteValidadeId")]
        public LoteValidade LoteValidade { get; set; }

        public long? UnidadeProdutoId { get; set; }

        [ForeignKey("UnidadeProdutoId")]
        public Unidade UnidadeProduto { get; set; }

        public long? EstoqueKitId { get; set; }

        [ForeignKey("EstoqueKitId")]
        public EstoqueKit EstoqueKit { get; set; }



    }
}
