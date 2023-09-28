using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoqueMovimentoItem")]
    public class EstoqueMovimentoItem : CamposPadraoCRUD
    {
        public long MovimentoId { get; set; }
        public long ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public string NumeroSerie { get; set; }
        public decimal CustoUnitario { get; set; }
        public long? ProdutoUnidadeId { get; set; }
        public decimal PerIPI { get; set; }

        public decimal ValorIPI { get; set; }

        public decimal PerICMS { get; set; }
        public decimal ValorICMS { get; set; }

        public long? MovimentoItemEstadoId { get; set; }

        [Column("PreMovimentoItemId")]
        public long? EstoquePreMovimentoItemId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("MovimentoId")]
        public EstoqueMovimento EstoqueMovimento { get; set; }

        [ForeignKey("ProdutoUnidadeId")]
        public ProdutoUnidade ProdutoUnidade { get; set; }

        [ForeignKey("EstoquePreMovimentoItemId")]
        public EstoquePreMovimentoItem EstoquePreMovimentoItem { get; set; }

        [ForeignKey("MovimentoItemEstadoId")]
        public EstoquePreMovimentoEstado MovimentoItemEstado { get; set; }

        public long? InventarioItemId { get; set; }

        [ForeignKey("InventarioItemId")]
        public InventarioItem InventarioItem { get; set; }

        public List<EstoqueMovimentoLoteValidade> EstoqueMovimentosLoteValidades { get; set; }

    }
}
