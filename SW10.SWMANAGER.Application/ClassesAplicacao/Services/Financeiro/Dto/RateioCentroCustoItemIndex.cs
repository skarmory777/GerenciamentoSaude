namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class RateioCentroCustoItemIndex
    {
        public long? Id { get; set; }
        public decimal PercentualRateio { get; set; }
        public long CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }

        public long? IdGrid { get; set; }
    }
}
