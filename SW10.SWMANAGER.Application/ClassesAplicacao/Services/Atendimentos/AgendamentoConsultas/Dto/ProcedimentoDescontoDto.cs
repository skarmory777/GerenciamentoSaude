namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class ProcedimentoDescontoDto
    {
        public long Id { get; set; }
        public long FaturamentoItemId { get; set; }
        public string Procedimento { get; set; }
        public decimal? ValorSemDesconto { get; set; }
        public decimal? ValorComDesconto { get; set; }
    }
}
