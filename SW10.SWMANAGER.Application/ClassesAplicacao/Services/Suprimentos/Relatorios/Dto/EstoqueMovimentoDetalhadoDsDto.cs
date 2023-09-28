namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto
{
    public class EstoqueMovimentoDetalhadoDsDto
    {
        public string Estoque { get; set; }
        public string Grupo { get; set; }
        public string CodProduto { get; set; }
        public string Produto { get; set; }
        public string Data { get; set; }
        public string QuantidadeEntrada { get; set; }
        public string QuantidadeSaida { get; set; }
        public string CustoUnitario { get; set; }
        public string TipoOperacao { get; set; }
        public string Lote { get; set; }
        public string Validade { get; set; }
        public string Unidade { get; set; }
        public string Documento { get; set; }
        public string NumeroSerie { get; set; }
        public string SaldoInicial { get; set; }
        public string SaldoFinal { get; set; }
    }
}
