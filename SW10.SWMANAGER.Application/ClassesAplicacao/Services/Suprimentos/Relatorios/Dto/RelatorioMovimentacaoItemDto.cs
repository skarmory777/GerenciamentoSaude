namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto
{
    public class RelatorioMovimentacaoItemDto
    {
        public string Documento { get; set; }
        public string Grupo { get; set; }
        public string Classe { get; set; }
        public string SubClass { get; set; }
        public string Produto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
    }
}
