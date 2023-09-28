using Abp.Domain.Entities;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public abstract class VersaoTissRelacao : Entity<long>
    {
        public long VersaoTissId { get; set; }

        public bool Incluido { get; set; }

        public bool Excluido { get; set; }

        [ForeignKey("VersaoTissId")]
        public VersaoTiss VersaoTiss { get; set; }
    }
}
