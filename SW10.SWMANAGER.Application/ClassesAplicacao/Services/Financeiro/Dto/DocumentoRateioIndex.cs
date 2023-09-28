namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class DocumentoRateioIndex
    {
        public long? Id { get; set; }
        public long IdGrid { get; set; }
        public long? EmpresaId { get; set; }
        public string EmpresaDescricao { get; set; }
        public long? ContaAdministrativaId { get; set; }
        public string ContaAdministrativaDescricao { get; set; }
        public long? CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }
        public decimal? Valor { get; set; }
        public string Observacao { get; set; }
        public bool IsImposto { get; set; }
    }
}
