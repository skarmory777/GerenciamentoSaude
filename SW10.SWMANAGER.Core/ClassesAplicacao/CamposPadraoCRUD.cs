using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public abstract class CamposPadraoCRUD : FullAuditedEntity<long>
    {
        public bool IsSistema { get; set; }

        [StringLength(10)]
        public virtual string Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual int? ImportaId { get; set; }

        [Index("Idx_IsDeleted")]
        public override bool IsDeleted { get => base.IsDeleted; set => base.IsDeleted = value; }
    }
}
