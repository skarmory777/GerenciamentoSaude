namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class ValidaProdutoSaldoDto : CamposPadraoCRUDDto
    {
        public decimal Quantidade { get; set; }

        public bool IsEntrada { get; set; }

        public long EstoqueId { get; set; }
        public long? ProdutoId { get; set; }
        public long? LoteValidadeId { get; set; }
    }
}
 