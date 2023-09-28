using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(RateioCentroCustoItem))]
    public class RateioCentroCustoItemDto : CamposPadraoCRUDDto
    {
        public long? RateioCentroCustoId { get; set; }
        public RateioCentroCustoDto RateioCentroCusto { get; set; }
        public decimal PercentualRateio { get; set; }
        public long CentroCustoId { get; set; }
        public CentroCustoDto CentroCusto { get; set; }
        public string CentroCustoDescricao { get; set; }

        public long? IdGrid { get; set; }
    }
}
