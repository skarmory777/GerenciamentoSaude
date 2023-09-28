using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    [AutoMap(typeof(CmpOrdemCompra))]
    public class OrdemCompraDto : CamposPadraoCRUDDto
    {
        public string OrdemCompraItensJson { get; set; }
        public int PrazoPagamento { get; set; }
        public DateTime DataOrdemCompra { get; set; }

        public DateTime DataPrevistaEntrega { get; set; }

        public DateTime DataFinalEntrega { get; set; }

        public decimal ValorFrete { get; set; }

        public string EnderecoEntrega { get; set; }

        public decimal ValorDesconto { get; set; }
        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public long OrdemCompraStatusId { get; set; }
        public OrdemCompraStatus OrdemCompraStatus { get; set; }
    }
}
