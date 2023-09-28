using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("vwRptEstMovDetalhado")]
    public class VWRptEstoqueMovimentoDetalhado : Entity<long>
    {
        public long? EstoqueId { get; set; }
        public string Estoque { get; set; }
        public long? GrupoId { get; set; }
        public string Grupo { get; set; }
        public long? ProdutoId { get; set; }
        public string CodProduto { get; set; }
        public string Produto { get; set; }
        public string Unidade { get; set; }
        public string Documento { get; set; }
        public DateTime Data { get; set; }
        public decimal QuantidadeEntrada { get; set; }
        public decimal QuantidadeSaida { get; set; }
        public decimal CustoUnitario { get; set; }
        public string NumeroSerie { get; set; }
        public string TipoMovimento { get; set; }
        public bool IsEntrada { get; set; }
        public long? EstTipoMovimentoId { get; set; }
        public string TipoOperacao { get; set; }
        public string CentroCusto { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }
        public string Pessoa { get; set; }
    }
}
