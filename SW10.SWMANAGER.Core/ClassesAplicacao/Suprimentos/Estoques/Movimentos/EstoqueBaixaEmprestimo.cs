using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstBaixaEmprestimo")]
    public class EstoqueBaixaEmprestimo : CamposPadraoCRUD
    {
        public string Documento { get; set; }
        public long FornecedorId { get; set; }
        public long EmpresaId { get; set; }

        [ForeignKey("FornecedorId")]
        public Fornecedor Fornecedor { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
    }
}
