using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores
{
    [Table("Fornecedor")]
    public class Fornecedor : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("TipoPessoaId")]
        public TipoPessoa TipoPessoa { get; set; }

        [ForeignKey("TipoCadastroExistenteId")]
        public TipoCadastroExistente TipoCadastroExistente { get; set; }
        public bool IsAtivo { get; set; }

        [ForeignKey("FornecedorPessoaFisicaId")]
        public FornecedorPessoaFisica FornecedorPessoaFisica { get; set; }

        [ForeignKey("FornecedorPessoaJuridicaId")]
        public FornecedorPessoaJuridica FornecedorPessoaJuridica { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }
        [ForeignKey("MedicoId")]
        public Medico Medico { get; set; }
        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        public long TipoPessoaId { get; set; }
        public long? CadastroExistenteId { get; set; }
        public long? TipoCadastroExistenteId { get; set; }
        public long? FornecedorPessoaFisicaId { get; set; }
        public long? FornecedorPessoaJuridicaId { get; set; }
        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? EmpresaId { get; set; }

        // Novo modelo SisPessoa
        // Novo modelo SisPessoa
        [ForeignKey("SisPessoa"), Column("SisPessoaId")]
        public long? SisPessoaId { get; set; }
        public SisPessoa SisPessoa { get; set; }
    }
}
