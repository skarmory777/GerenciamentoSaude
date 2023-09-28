using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("vwRptEstMovResumido")]
    public class VWRptEstoqueMovimentoResumido : Entity<long>
    {
        public long? EstoqueId { get; set; }
        public string Estoque { get; set; }
        public long? GrupoId { get; set; }
        public string Grupo { get; set; }
        public long? ProdutoId { get; set; }
        public string CodProduto { get; set; }
        public string Produto { get; set; }
        //public string Unidade { get; set; }
        //public DateTime Data { get; set; }
        public decimal QtdSaldoInicial { get; set; }
        public decimal QtdEntrada { get; set; }
        public decimal QtdSaida { get; set; }
        public decimal QtdFinal { get; set; }
        public decimal QtdEntradaApos { get; set; }
        public decimal QtdSaidaApos { get; set; }
        public decimal QtdSaldoAtual { get; set; }
    }
}
