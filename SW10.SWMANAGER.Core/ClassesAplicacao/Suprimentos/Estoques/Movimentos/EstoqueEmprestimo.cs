using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoqueEmprestimo")]
    public class EstoqueEmprestimo : CamposPadraoCRUD
    {        
        public long SisPessoaId { get; set; }

        [MaxLength(100)]
        public string ContatoNome { get; set; }

        [MaxLength(80)]
        public string ContatoTelefone { get; set; }

        [MaxLength(100)]
        public string ContatoEmail { get; set; }

        [ForeignKey("SisPessoaId")]
        public SisPessoa SisPessoa { get; set; }
    }
}
