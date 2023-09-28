namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto
{
    public class EstoqueMovimentoResumidoDsDto
    {
        public string Estoque { get; set; }
        public string Grupo { get; set; }
        public string CodProduto { get; set; }
        public string Produto { get; set; }
        public string QtdSaldoInicial { get; set; }
        public string QtdEntrada { get; set; }
        public string QtdSaida { get; set; }
        public string QtdFinal { get; set; }
        public string QtdEntradaApos { get; set; }
        public string QtdSaidaApos { get; set; }
        public string QtdSaldoAtual { get; set; }
    }
}
