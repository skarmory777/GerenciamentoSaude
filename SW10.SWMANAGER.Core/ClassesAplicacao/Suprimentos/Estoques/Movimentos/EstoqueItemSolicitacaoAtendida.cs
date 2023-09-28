using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstItemSolicitacaoAtendida")]
    public class EstoqueItemSolicitacaoAtendida : CamposPadraoCRUD
    {
        public long SolicitacaoItemId { get; set; }
        public long PreMovimentoItemId { get; set; }

        [ForeignKey("SolicitacaoItemId")]
        public EstoqueSolicitacaoItem EstoqueSolicitacaoItem { get; set; }

        [ForeignKey("PreMovimentoItemId")]
        public EstoquePreMovimentoItem EstoquePreMovimentoItem { get; set; }
    }
}
