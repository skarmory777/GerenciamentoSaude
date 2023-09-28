using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    //[AutoMap(typeof(ProdutoSaldo))]
    public class ProdutoSaldoMinReportDto : CamposPadraoCRUDDto
    {
        public long? GrupoId { get; set; }
        public long? ProdutoId { get; set; }

        public decimal QuantidadeAtual { get; set; }
        public decimal QuantidadeEntradaPendente { get; set; }
        public decimal QuantidadeSaidaPendente { get; set; }

        public decimal QuantidadeGerencialAtual { get; set; }
        public decimal QuantidadeGerencialEntradaPendente { get; set; }
        public decimal QuantidadeGerencialSaidaPendente { get; set; }

        public decimal SaldoFuturo
        {
            get { return QuantidadeAtual + QuantidadeEntradaPendente - QuantidadeSaidaPendente; }
            set {; }
        }

        public decimal SaldoGerencialFuturo { get; set; }

        public virtual Estoque Estoque { get; set; }

        public string DescricaoProduto { get; set; }

        public string UnidadeReferencia { get; set; }
        public string UnidadeGerencial { get; set; }

    }
}
