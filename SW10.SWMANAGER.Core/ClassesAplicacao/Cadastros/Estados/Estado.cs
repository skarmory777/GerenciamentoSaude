using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados
{
    [Table("SisEstado")]
    public class Estado : CamposPadraoCRUD
    {
        [MaxLength(75)]
        public string Nome { get; set; }

        [MaxLength(5)]
        public string Uf { get; set; }

        public Pais Pais { get; set; }
        [ForeignKey("Pais"), Column("SisPaisId")]
        public long? PaisId { get; set; }
    }
}
