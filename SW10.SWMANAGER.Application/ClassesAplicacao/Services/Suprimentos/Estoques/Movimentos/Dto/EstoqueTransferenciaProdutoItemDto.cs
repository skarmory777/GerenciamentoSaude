namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoqueTransferenciaProdutoItemDto : CamposPadraoCRUDDto
    {
        public long PreMovimentoEntradaItemId { get; set; }
        public long PreMovimentoSaidaItemId { get; set; }
        public long EstoqueTransferenciaProdutoID { get; set; }
    }
}
