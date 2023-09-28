using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("vwRptSaldoProduto")]
    public class VWRptSaldoProduto : Entity<long>
    {
        public long? EstoqueId { get; set; }
        public string Estoque { get; set; }
        public long? GrupoId { get; set; }
        public string Grupo { get; set; }
        public long? ProdutoId { get; set; }
        public string Produto { get; set; }
        public decimal QuantidadeAtual { get; set; }
        public decimal QuantidadeEntradaPendente { get; set; }
        public decimal QuantidadeSaidaPendente { get; set; }
        public decimal SaldoFuturo { get; set; }
        public string UnidadeReferencial { get; set; }
        public string UnidadeGerencial { get; set; }
    }
}
