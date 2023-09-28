using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores
{
    [Table("FornecedorPessoaJuridica")]
    public class FornecedorPessoaJuridica : PessoaJuridica
    {
        public long FornecedorId { get; set; }
    }
}