using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    public class OrdemCompraIndexDto
    {
        public long Id { get; set; }

        public string Codigo { get; set; }

        public int PrazoPagamento { get; set; }

        public DateTime DataOrdemCompra { get; set; }

        public DateTime DataPrevistaEntrega { get; set; }

        public DateTime DataFinalEntrega { get; set; }

        public decimal ValorFrete { get; set; }

        public string EnderecoEntrega { get; set; }

        public decimal ValorDesconto { get; set; }

        public long EmpresaId { get; set; }
        public string Empresa { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public string UnidadeOrganizacional { get; set; }

        public long OrdemCompraStatusId { get; set; }
        public string OrdemCompraStatus { get; set; }
    }
}
