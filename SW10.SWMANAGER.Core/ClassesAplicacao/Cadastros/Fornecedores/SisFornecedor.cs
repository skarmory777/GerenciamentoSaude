using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores
{
    [Table("SisFornecedor")]
    public class SisFornecedor : CamposPadraoCRUD
    {
        [ForeignKey("SisPessoa"), Column("SisPessoaId")]
        public long? SisPessoaId { get; set; }
        public SisPessoa SisPessoa { get; set; }
    }
}
