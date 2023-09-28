namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    public class FormatacaoItemIndexDto
    {
        public long Id { get; set; }
        public string CodigoItem { get; set; }
        public string DescricaoItem { get; set; }
        public long ItemId { get; set; }
        public string Unidade { get; set; }
        public string Referencia { get; set; }
        public string Resultado { get; set; }
        public long? UnidadeId { get; set; }
        public long? LaudoResultadoId { get; set; }
        public string Exame { get; set; }
        public long ResultadoExameId { get; set; }
        public long GridId { get; set; }
        public long TipoResultadoId { get; set; }
        public int? CasaDecimal { get; set; }
        public long? TabelaId { get; set; }
        public string ResultadoVisualizacao { get; set; }
        public long? ExameStatusId { get; set; }
    }
}
