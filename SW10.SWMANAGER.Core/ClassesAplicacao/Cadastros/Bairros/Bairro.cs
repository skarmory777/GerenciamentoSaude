using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Bairros
{
    [Table("SisBairro")]
    public class Bairro : CamposPadraoCRUD, IDescricao
    {
        [MaxLength(120)]
        public string Nome { get; set; }

        public Cidade Cidade { get; set; }
        [ForeignKey("Cidade"), Column("SisCidadeId")]
        public long? CidadeId { get; set; }

    }
}
