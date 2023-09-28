using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Diagnosticos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposSanguineos;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes
{
    [Table("SisPaciente")]
    public class Paciente : CamposPadraoCRUD  //PessoaFisica
    {
        public int CodigoPaciente { get; set; }

        public long? Prontuario { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public bool? IsDoador { get; set; }

        public long? Cns { get; set; }

        public string Indicacao { get; set; }

        [ForeignKey("TipoSanguineo"), Column("SisTipoSanguineoId")]
        public long? TipoSanguineoId { get; set; }

        public TipoSanguineo TipoSanguineo { get; set; }

        public string MemedId { get; set; }

        public ICollection<PacientePeso> PacientePesos { get; set; }

        public ICollection<PacienteDiagnosticos> PacienteDiagnosticos { get; set; }

        public ICollection<PacienteAlergias> PacienteAlergias { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        // NOVO MODELO SISPESSOA
        [ForeignKey("SisPessoa"), Column("SisPessoaId")]
        public long? SisPessoaId { get; set; }
        public SisPessoa SisPessoa { get; set; }


        [MaxLength(100)]
        public string NomeCompleto
        {
            get { return this.SisPessoa?.NomeCompleto; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomeCompleto = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? Nascimento
        {
            get { return this.SisPessoa?.Nascimento; }
            set { if (this.SisPessoa != null) this.SisPessoa.Nascimento = value; }
        }

        [ForeignKey("SexoId")]
        public Sexo Sexo
        {
            get { return this.SisPessoa?.Sexo; }
            set { if (this.SisPessoa != null) this.SisPessoa.Sexo = value; }
        }
        public long? SexoId
        {
            get { return this.SisPessoa?.SexoId; }
            set { if (this.SisPessoa != null) this.SisPessoa.SexoId = value; }
        }

        [ForeignKey("CorPeleId")]
        public CorPele CorPele
        {
            get { return this.SisPessoa?.CorPele; }
            set { if (this.SisPessoa != null) this.SisPessoa.CorPele = value; }
        }
        public long? CorPeleId
        {
            get { return this.SisPessoa?.CorPeleId; }
            set { if (this.SisPessoa != null) this.SisPessoa.CorPeleId = value; }
        }


        [ForeignKey("ProfissaoId")]
        public Profissao Profissao
        {
            get { return this.SisPessoa?.Profissao; }
            set { if (this.SisPessoa != null) this.SisPessoa.Profissao = value; }
        }
        public long? ProfissaoId
        {
            get { return this.SisPessoa?.ProfissaoId; }
            set { if (this.SisPessoa != null) this.SisPessoa.ProfissaoId = value; }
        }

        [ForeignKey("EscolaridadeId")]
        public Escolaridade Escolaridade
        {
            get { return this.SisPessoa?.Escolaridade; }
            set { if (this.SisPessoa != null) this.SisPessoa.Escolaridade = value; }
        }
        public long? EscolaridadeId
        {
            get { return this.SisPessoa?.EscolaridadeId; }
            set { if (this.SisPessoa != null) this.SisPessoa.EscolaridadeId = value; }
        }


        [StringLength(20)]
        public string Rg
        {
            get { return this.SisPessoa?.Rg; }
            set { if (this.SisPessoa != null) this.SisPessoa.Rg = value; }
        }

        [StringLength(20)]
        public string Emissor
        {
            get { return this.SisPessoa?.Emissor; }
            set { if (this.SisPessoa != null) this.SisPessoa.Emissor = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? Emissao
        {
            get { return this.SisPessoa?.EmissaoRg; }
            set { if (this.SisPessoa != null) this.SisPessoa.EmissaoRg = value; }
        }

        [StringLength(14)]
        public string Cpf
        {
            get { return this.SisPessoa?.Cpf; }
            set { if (this.SisPessoa != null) this.SisPessoa.Cpf = value; }
        }

        [ForeignKey("NaturalidadeId")]
        public Naturalidade Naturalidade
        {
            get { return this.SisPessoa?.Naturalidade; }
            set { if (this.SisPessoa != null) this.SisPessoa.Naturalidade = value; }
        }

        public long? NaturalidadeId
        {
            get { return this.SisPessoa?.NaturalidadeId; }
            set { if (this.SisPessoa != null) this.SisPessoa.NaturalidadeId = value; }
        }


        [ForeignKey("NacionalidadeId")]
        public Nacionalidade Nacionalidade
        {
            get { return this.SisPessoa?.Nacionalidade; }
            set { if (this.SisPessoa != null) this.SisPessoa.Nacionalidade = value; }
        }
        public long? NacionalidadeId
        {
            get { return this.SisPessoa?.NacionalidadeId; }
            set { if (this.SisPessoa != null) this.SisPessoa.NacionalidadeId = value; }
        }


        [ForeignKey("EstadoCivilId")]
        public EstadoCivil EstadoCivil
        {
            get { return this.SisPessoa?.EstadoCivil; }
            set { if (this.SisPessoa != null) this.SisPessoa.EstadoCivil = value; }
        }
        public long? EstadoCivilId
        {
            get { return this.SisPessoa?.EstadoCivilId; }
            set { if (this.SisPessoa != null) this.SisPessoa.EstadoCivilId = value; }
        }

        [StringLength(100)]
        public string NomeMae
        {
            get { return this.SisPessoa?.NomeMae; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomeMae = value; }
        }

        [StringLength(100)]
        public string NomePai
        {
            get { return this.SisPessoa?.NomePai; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomePai = value; }
        }


        [ForeignKey("ReligiaoId")]
        public Religiao Religiao
        {
            get { return this.SisPessoa?.Religiao; }
            set { if (this.SisPessoa != null) this.SisPessoa.Religiao = value; }
        }
        public long? ReligiaoId
        {
            get { return this.SisPessoa?.ReligiaoId; }
            set { if (this.SisPessoa != null) this.SisPessoa.ReligiaoId = value; }
        }

        public byte[] Foto
        {
            get { return this.SisPessoa?.Foto; }
            set { if (this.SisPessoa != null) this.SisPessoa.Foto = value; }
        }

        public string FotoMimeType
        {
            get { return this.SisPessoa?.FotoMimeType; }
            set { if (this.SisPessoa != null) this.SisPessoa.FotoMimeType = value; }
        }

        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get { return this.SisPessoa?.Email; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email = value; }
        }

        public string Email2
        {
            get { return this.SisPessoa?.Email2; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email2 = value; }
        }

        public string Email3
        {
            get { return this.SisPessoa?.Email3; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email3 = value; }
        }

        public string Email4
        {
            get { return this.SisPessoa?.Email4; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email4 = value; }
        }

        [StringLength(80)]
        public string Telefone1
        {
            get { return this.SisPessoa?.Telefone1; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone1 = value; }
        }

        [ForeignKey("TipoTelefone1Id")]
        public TipoTelefone TipoTelefone1
        {
            get { return this.SisPessoa?.TipoTelefone1; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone1 = value; }
        }
        public long? TipoTelefone1Id
        {
            get { return this.SisPessoa?.TipoTelefone1Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone1Id = value; }
        }


        public int? DddTelefone1
        {
            get { return this.SisPessoa?.DddTelefone1; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone1 = value; }
        }

        [StringLength(80)]
        public string Telefone2
        {
            get { return this.SisPessoa?.Telefone2; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone2 = value; }
        }


        [ForeignKey("TipoTelefone2Id")]
        public TipoTelefone TipoTelefone2
        {
            get { return this.SisPessoa?.TipoTelefone2; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone2 = value; }
        }
        public long? TipoTelefone2Id
        {
            get { return this.SisPessoa?.TipoTelefone2Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone2Id = value; }
        }

        public int? DddTelefone2
        {
            get { return this.SisPessoa?.DddTelefone2; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone2 = value; }
        }

        [StringLength(80)]
        public string Telefone3
        {
            get { return this.SisPessoa?.Telefone3; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone3 = value; }
        }

        [ForeignKey("TipoTelefone3Id")]
        public TipoTelefone TipoTelefone3
        {
            get { return this.SisPessoa?.TipoTelefone3; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone3 = value; }
        }
        public long? TipoTelefone3Id
        {
            get { return this.SisPessoa?.TipoTelefone3Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone3Id = value; }
        }



        public int? DddTelefone3
        {
            get { return this.SisPessoa?.DddTelefone3; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone3 = value; }
        }

        [StringLength(80)]
        public string Telefone4
        {
            get { return this.SisPessoa?.Telefone4; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone4 = value; }
        }

        [ForeignKey("TipoTelefone4Id")]
        public TipoTelefone TipoTelefone4
        {
            get { return this.SisPessoa?.TipoTelefone4; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone4 = value; }
        }
        public long? TipoTelefone4Id
        {
            get { return this.SisPessoa?.TipoTelefone4Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone4Id = value; }
        }

        public int? DddTelefone4
        {
            get { return this.SisPessoa?.DddTelefone4; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone4 = value; }
        }


        [StringLength(9)]
        public string Cep
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Cep : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Cep = value; }
        }

        [ForeignKey("CidadeId")]
        public Cidade Cidade
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Cidade : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Cidade = value; }
        }
        public long? CidadeId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].CidadeId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].CidadeId = value; }
        }

        [StringLength(30)]
        public string Complemento
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Complemento : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Complemento = value; }
        }

        [ForeignKey("EstadoId")]
        public Estado Estado
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Estado : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Estado = value; }
        }
        public long? EstadoId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].EstadoId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].EstadoId = value; }
        }

        [ForeignKey("PaisId")]
        public Pais Pais
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Pais : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Pais = value; }
        }
        public long? PaisId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].PaisId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].PaisId = value; }
        }

        [StringLength(80)]
        public string Logradouro
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Logradouro : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Logradouro = value; }
        }

        [StringLength(20)]
        public string Numero
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Numero : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Numero = value; }
        }

        public long? TipoLogradouroId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].TipoLogradouroId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].TipoLogradouroId = value; }
        }

        [ForeignKey("TipoLogradouroId")]
        public TipoLogradouro TipoLogradouro
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].TipoLogradouro : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].TipoLogradouro = value; }
        }

        [StringLength(40)]
        public string Bairro
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Bairro : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Bairro = value; }
        }
        public Guid? AnexoListaId { get; set; }
    }
}

