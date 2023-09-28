using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SolicitacaoAutorizacoes
{
    [Table("FatSolicitacaoAutorizacao")]
    public class SolicitacaoAutorizacao : CamposPadraoCRUD
    {
        public long? AtendimentoId { get; set; }

        public ICollection<SolicitacaoAutorizacaoItem> Items { get; set; }

        public long? SolicitacaoAutorizacaoStatusId { get; set; }

        public SolicitacaoAutorizacaoStatus SolicitacaoAutorizacaoStatus { get; set; }

    }
}
