using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoSaldo))]
    public class ProdutoSaldoDto : CamposPadraoCRUDDto
    {
        //public string Descricao { get; set; }

        public long EstoqueId { get; set; }
        public long ProdutoId { get; set; }
        public long? LoteValidadeId { get; set; }
        public long? EmprestimoId { get; set; }
        public long? ConsignadoId { get; set; }
        public long? ValeId { get; set; }
        public decimal QuantidadeAtual { get; set; }
        public decimal QuantidadeEntradaPendente { get; set; }
        public decimal QuantidadeSaidaPendente { get; set; }
        public decimal SaldoFuturo { get; set; }

        public string NomeLaboratorio { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }

        public decimal QuantidadeGerencialAtual { get; set; }
        public decimal QuantidadeGerencialEntradaPendente { get; set; }
        public decimal QuantidadeGerencialSaidaPendente { get; set; }
        public decimal SaldoGerencialFuturo { get; set; }

        public virtual EstoqueDto Estoque { get; set; }
        public virtual LoteValidadeDto LoteValidade { get; set; }

        public string UnidadeReferencia { get; set; }
        public string UnidadeGerencial { get; set; }

        //public virtual Estoque Estoque { get; set; }
        //public virtual LoteValidade LoteValidade { get; set; }

    }
}
