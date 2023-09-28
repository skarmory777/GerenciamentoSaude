using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("ProdutoSaldo")]
    public class ProdutoSaldo : CamposPadraoCRUD
    {
        public long EstoqueId { get; set; }
        public long ProdutoId { get; set; }
        public long? LoteValidadeId { get; set; }
        public long? EmprestimoId { get; set; }
        public long? ConsignadoId { get; set; }
        public long? ValeId { get; set; }
        public decimal QuantidadeAtual { get; set; }
        public decimal QuantidadeEntradaPendente { get; set; }
        public decimal QuantidadeSaidaPendente { get; set; }

        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("LoteValidadeId")]
        public LoteValidade LoteValidade { get; set; }

        [ForeignKey("EmprestimoId")]
        public Fornecedor Emprestimo { get; set; }

        [ForeignKey("ConsignadoId")]
        public Fornecedor Consignado { get; set; }

        [ForeignKey("ValeId")]
        public Fornecedor Vale { get; set; }

    }
}
