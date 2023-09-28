using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores
{
    [Table("FornecedorPessoaFisica")]
    public class FornecedorPessoaFisica : PessoaFisica
    {
        public long FornecedorId { get; set; }
    }
}