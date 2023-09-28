using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SolicitacaoAutorizacoes
{
    public class SolicitacaoAutorizacaoDto : CamposPadraoCRUDDto
    {
        public long? AtendimentoId { get; set; }

        public ICollection<SolicitacaoAutorizacaoItemDto> Items { get; set; }

        public long? SolicitacaoAutorizacaoStatusId { get; set; }

        public SolicitacaoAutorizacaoStatusDto SolicitacaoAutorizacaoStatus { get; set; }

    }
}
