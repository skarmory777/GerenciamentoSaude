using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas
{
    [Table("ConsultorTabelaCampoRelacao")]
    public class ConsultorTabelaCampoRelacao : Entity<long>
    {
        public long ConsultorTabelaId { get; set; }

        public long ConsultorTabelaCampoId { get; set; }

        [ForeignKey("ConsultorTabelaId")]
        public ConsultorTabela ConsultorTabela { get; set; }

        [ForeignKey("ConsultorTabelaCampoId")]
        public ConsultorTabelaCampo ConsultorTabelaCampo { get; set; }
    }
}
