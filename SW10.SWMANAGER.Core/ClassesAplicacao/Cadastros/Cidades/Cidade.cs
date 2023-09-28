using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades
{
    [Table("SisCidade")]
    public class Cidade : CamposPadraoCRUD, IDescricao
    {
        [MaxLength(120)]
        public string Nome { get; set; }

        public Estado Estado { get; set; }
        [ForeignKey("Estado"), Column("SisEstadoId")]
        public long? EstadoId { get; set; }

        public bool IsCapital { get; set; }
    }
}
