using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class ListagemInventarioEstoque 
    {
        public long Id { get; set; }
        public long ProdutoId { get; set; }
        public string ProdutoDescricao { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }

        public string InventarioItemStatus { get; set; }

        public long? LoteValidadeId { get; set; }
        public decimal? QuantidadeContagem { get; set; }
        public decimal? QuantidadeEstoque { get; set; }

    }
}
