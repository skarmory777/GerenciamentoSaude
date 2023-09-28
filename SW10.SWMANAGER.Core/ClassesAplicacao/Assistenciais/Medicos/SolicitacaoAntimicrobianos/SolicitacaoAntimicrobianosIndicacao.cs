using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssSolicitacaoAntimicrobianosIndicacoes")]
    public class SolicitacaoAntimicrobianosIndicacao : FullAuditedEntity<long>
    {
        public long TipoSolicitacaoAntimicrobianosIndicacaoId { get; set; }

        public TipoSolicitacaoAntimicrobianosIndicacao TipoIndicacao { get; set; }

        public long SolicitacaoAntimicrobianoId { get; set; }

        public SolicitacaoAntimicrobiano SolicitacaoAntimicrobiano { get; set; }
    }
}
