using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class InventarioEstoque: CamposPadraoCRUDDto
    {
        public long InvItemId { get; set; }
        public DateTime DataInventario { get; set; }
        public string Status { get; set; }

        public long StatusInventarioId { get; set; }
        public long StatusInventarioItemId { get; set; }
        public string StatusInventarioItemDescricao { get; set; }
        public bool TemDivergencia { get; set; }
        public long ProdutoId { get; set; }
        public string ProdutoDescricao { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }
        public long? LoteValidadeId { get; set; }
        public decimal? QuantidadeContagem { get; set; }
        public long ItemId { get; set; }
        public string EstoqueDescricao { get; set; }
    }

    public class DashboardInventarioEstoque
    {
        public long TotalItems { get; set; }

        public long TotalContado { get; set; }

        public long TotalPrimeiraContagem { get; set; }

        public long TotalSegundaContagem { get; set; }

        public long TotalDivergente { get; set; }

        public long TotalPendente { get; set; }

        public long TotalFechado { get; set; }
    }
}
