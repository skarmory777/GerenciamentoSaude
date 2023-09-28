using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class ListagemInventarioEstoqueContagem : CamposPadraoCRUDDto
    {
        public long Id { get; set; }
        public string ProdutoDescricao { get; set; }
        public string Lote { get; set; }

        public long StatusInventarioItemId { get; set; }
        public string StatusInventarioItemDescricao { get; set; }
        public DateTime? Validade { get; set; }
       // public long? LoteValidadeId { get; set; }
        //public decimal? QuantidadeEstoque { get; set; }
        public decimal? QuantidadeContagem { get; set; }
        public long ItemId { get; set; }
        public long GridId { get; set; }
        public long? ProdutoId { get; set; }
    }
}
