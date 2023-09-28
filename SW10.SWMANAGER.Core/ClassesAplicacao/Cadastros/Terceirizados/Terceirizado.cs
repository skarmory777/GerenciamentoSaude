using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados
{
    [Table("SisTerceirizado")]
    public class Terceirizado : CamposPadraoCRUD
    {
        // Novo modelo SisPessoa
        [ForeignKey("SisPessoa"), Column("SisPessoaId")]
        public long? SisPessoaId { get; set; }
        public SisPessoa SisPessoa { get; set; }
    }
}
