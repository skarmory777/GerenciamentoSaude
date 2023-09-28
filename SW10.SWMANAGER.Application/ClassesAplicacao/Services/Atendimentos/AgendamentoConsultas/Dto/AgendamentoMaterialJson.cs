namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class AgendamentoMaterialJson
    {
        public long? Id { get; set; }
        public decimal QuantidadeMaterial { get; set; }
        public long IdGrid { get; set; }
        public long? FaturamentoItemId { get; set; }
        public string Descricao { get; set; }
    }
}
