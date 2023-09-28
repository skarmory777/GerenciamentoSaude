namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoCirurgias
{
    public class AgendamentoDescontoViewModel
    {
        public long? Id { get; set; }
        public string ValorSemDesconto { get; set; }
        public decimal? ValorDescontoTotal { get; set; }
        public string DescontoJson { get; set; }
    }
}