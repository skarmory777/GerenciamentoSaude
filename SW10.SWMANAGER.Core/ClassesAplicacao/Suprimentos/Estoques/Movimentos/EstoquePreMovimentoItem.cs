using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoquePreMovimentoItem")]
    public class EstoquePreMovimentoItem : CamposPadraoCRUD
    {
        public long PreMovimentoId { get; set; }
        public long ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public string NumeroSerie { get; set; }
        public decimal CustoUnitario { get; set; }
        public string CodigoBarra { get; set; }
        public long? ProdutoUnidadeId { get; set; }
        public decimal PerIPI { get; set; }

        public decimal ValorIPI { get; set; }

        public decimal ValorICMS { get; set; }
        public decimal PerICMS { get; set; }

        public long? PreMovimentoItemEstadoId { get; set; }

        public long? EstoqueKitItemId { get; set; }

        [ForeignKey("EstoqueKitItemId")]
        public EstoqueKitItem EstoqueKitItem { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("PreMovimentoId")]
        public EstoquePreMovimento EstoquePreMovimento { get; set; }

        [ForeignKey("ProdutoUnidadeId")]
        public Unidade ProdutoUnidade { get; set; }

        [ForeignKey("PreMovimentoItemEstadoId")]
        public EstoquePreMovimentoEstado PreMovimentoItemEstado { get; set; }

        public long? InventarioItemId { get; set; }

        [ForeignKey("InventarioItemId")]
        public InventarioItem InventarioItem { get; set; }

        public List<EstoquePreMovimentoLoteValidade> EstoquePreMovimentosLoteValidades { get; set; }
    }
}
