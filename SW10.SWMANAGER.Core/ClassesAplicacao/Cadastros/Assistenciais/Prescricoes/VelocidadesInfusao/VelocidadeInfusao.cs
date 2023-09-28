using Abp.Domain.Entities.Auditing;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao
{
    [Table("AssVelocidadeInfusao")]
    public class VelocidadeInfusao : CamposPadraoCRUD, IDescricao
    {
        public virtual ICollection<VelocidadeInfusaoFormaAplicacao> FormaAplicacao { get; set; }
    }

    [Table("AssVelocidadeInfusaoFormaAplicacao")]
    public class VelocidadeInfusaoFormaAplicacao : FullAuditedEntity<long>
    {
        [ForeignKey("VelocidadeInfusao"), Column("VelocidadeInfusaoId")]
        public long VelocidadeInfusaoId { get; set; }

        public VelocidadeInfusao VelocidadeInfusao { get; set; }

        [ForeignKey("FormaApplicacao"), Column("FormaApplicacaoId")]
        public long FormaApplicacaoId { get; set; }

        public FormaAplicacao FormaApplicacao { get; set; }
    }
}
