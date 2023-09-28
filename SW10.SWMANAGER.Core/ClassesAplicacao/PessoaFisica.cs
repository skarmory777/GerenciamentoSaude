using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public abstract class PessoaFisica : Pessoa
    {
        [MaxLength(100)]
        public string NomeCompleto { get; set; }

        public DateTime Nascimento { get; set; }

        public int? Sexo { get; set; }

        public int? CorPele { get; set; }

        [ForeignKey("ProfissaoId")]
        public Profissao Profissao { get; set; }
        public long? ProfissaoId { get; set; }

        public int? Escolaridade { get; set; }

        [StringLength(20)]
        public string Rg { get; set; }

        [StringLength(20)]
        public string Emissor { get; set; }

        public DateTime? Emissao { get; set; }

        [StringLength(14)]
        public string Cpf { get; set; }

        [ForeignKey("NaturalidadeId")]
        public Naturalidade Naturalidade { get; set; }
        public long? NaturalidadeId { get; set; }

        public long? NacionalidadeId { get; set; }

        [ForeignKey("NacionalidadeId")]
        public Nacionalidade Nacionalidade { get; set; }

        public int? EstadoCivil { get; set; }

        [StringLength(100)]
        public string NomeMae { get; set; }

        [StringLength(100)]
        public string NomePai { get; set; }

        public int? Religiao { get; set; }

        public byte[] Foto { get; set; }

        public string FotoMimeType { get; set; }
    }
}
