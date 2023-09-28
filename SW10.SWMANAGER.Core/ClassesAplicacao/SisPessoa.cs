using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposSanguineos;
using SW10.SWMANAGER.ClassesAplicacao.Pessoas;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    [Table("SisPessoa")]
    //TABELA ATUAL DO SISTEMA
    public class SisPessoa : CamposPadraoCRUD
    {

        private string _descricao;

        public override string Descricao
        {
            get { return NomeCompleto; }
            set { _descricao = value; }
        }


        [MaxLength(100)]
        public string NomeCompleto { get; set; }


        #region colunas Pessoa Físicao

        [StringLength(20)]
        public string Rg { get; set; }

        [StringLength(20)]
        public string Emissor { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EmissaoRg { get; set; }
        [Index("Sis_Idx_Nascimento")]
        [DataType(DataType.DateTime)]
        public DateTime? Nascimento { get; set; }

        [StringLength(14)]
        public string Cpf { get; set; }


        [StringLength(100)]
        public string NomeMae { get; set; }


        [StringLength(100)]
        public string NomePai { get; set; }

        #endregion

        #region Colunas Pessoa Jurídica

        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }

        #endregion

        #region Telefones

        [StringLength(80)]
        public string Telefone1 { get; set; }

        public int? DddTelefone1 { get; set; }

        [StringLength(80)]
        public string Telefone2 { get; set; }

        public int? DddTelefone2 { get; set; }

        [StringLength(80)]
        public string Telefone3 { get; set; }

        public int? DddTelefone3 { get; set; }

        [StringLength(80)]
        public string Telefone4 { get; set; }

        public int? DddTelefone4 { get; set; }

        #endregion

        #region Emails

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Email4 { get; set; }

        #endregion

        public byte[] Foto { get; set; }

        public string FotoMimeType { get; set; }

        [MaxLength(1)]
        public string FisicaJuridica { get; set; }

        public bool IsAtivo { get; set; }

        public long? ImportacaoId { get; set; }
        public string ImportacaoTabela { get; set; }

        public bool IsCredito { get; set; }
        public bool IsDebito { get; set; }

        //LISTA
        public List<Endereco> Enderecos { get; set; }

        public string Observacao { get; set; }

        //RELACIONAMENTOS
        #region Relacionamentos

        [ForeignKey("TipoLogradouroId")]
        public TipoLogradouro TipoLogradouro { get; set; }
        public long? TipoLogradouroId { get; set; }


        [ForeignKey("SexoId"), Column("SisSexoId")]
        public Sexo Sexo { get; set; }
        public long? SexoId { get; set; }


        [ForeignKey("TipoSanguineoId")]
        public TipoSanguineo TipoSanguineo { get; set; }
        public long? TipoSanguineoId { get; set; }


        [ForeignKey("ReligiaoId")]
        public Religiao Religiao { get; set; }
        public long? ReligiaoId { get; set; }


        [ForeignKey("CorPeleId")]
        public CorPele CorPele { get; set; }
        public long? CorPeleId { get; set; }


        [ForeignKey("EstadoCivilId")]
        public EstadoCivil EstadoCivil { get; set; }
        public long? EstadoCivilId { get; set; }


        [ForeignKey("EscolaridadeId")]
        public Escolaridade Escolaridade { get; set; }
        public long? EscolaridadeId { get; set; }


        [ForeignKey("NaturalidadeId")]
        public Naturalidade Naturalidade { get; set; }
        public long? NaturalidadeId { get; set; }


        [ForeignKey("NacionalidadeId")]
        public Nacionalidade Nacionalidade { get; set; }
        public long? NacionalidadeId { get; set; }


        [ForeignKey("TipoPessoaId")]
        public SisTipoPessoa TipoPessoa { get; set; }
        public long? TipoPessoaId { get; set; }

        [ForeignKey("ProfissaoId")]
        public Profissao Profissao { get; set; }
        public long? ProfissaoId { get; set; }


        //RELACIONAMENTOS TIPO DE TELEFONES 1-4
        [ForeignKey("TipoTelefone1Id")]
        public TipoTelefone TipoTelefone1 { get; set; }
        public long? TipoTelefone1Id { get; set; }

        [ForeignKey("TipoTelefone2Id")]
        public TipoTelefone TipoTelefone2 { get; set; }
        public long? TipoTelefone2Id { get; set; }

        [ForeignKey("TipoTelefone3Id")]
        public TipoTelefone TipoTelefone3 { get; set; }
        public long? TipoTelefone3Id { get; set; }

        [ForeignKey("TipoTelefone4Id")]
        public TipoTelefone TipoTelefone4 { get; set; }
        public long? TipoTelefone4Id { get; set; }

        #endregion
    }
}
