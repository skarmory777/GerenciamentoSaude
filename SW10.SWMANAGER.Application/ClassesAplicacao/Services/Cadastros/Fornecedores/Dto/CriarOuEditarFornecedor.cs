using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto
{
    [AutoMap(typeof(Fornecedor))]
    public class CriarOuEditarFornecedor : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public virtual TipoPessoaDto TipoPessoa { get; set; }
        public virtual TipoCadastroExistenteDto TipoCadastroExistente { get; set; }
        public bool IsAtivo { get; set; }

        public virtual FornecedorPessoaFisicaDto FornecedorPessoaFisica { get; set; }
        public virtual FornecedorPessoaJuridicaDto FornecedorPessoaJuridica { get; set; }

        public virtual PacienteDto Paciente { get; set; }
        public virtual MedicoDto Medico { get; set; }
        public virtual ConvenioDto Convenio { get; set; }
        public virtual EmpresaDto Empresa { get; set; }

        public long TipoPessoaId { get; set; }
        public long? CadastroExistenteId { get; set; }
        public long? TipoCadastroExistenteId { get; set; }
        public long? FornecedorPessoaFisicaId { get; set; }
        public long? FornecedorPessoaJuridicaId { get; set; }
        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? EmpresaId { get; set; }
    }
}
