using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    [Table("Sis_Pessoa")]
    public class Sis_Pessoa : CamposPadraoCRUD
    {
        public int? IDPessoa { get; set; }
        public int? IDSW { get; set; }
        public int? IDPessoaTipo { get; set; }
        public string CodPessoa { get; set; }
        public string Pessoa { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public int? IDBairro { get; set; }
        public int? IDCidade { get; set; }
        public int? IDEstado { get; set; }
        public string Pais { get; set; }
        public string CEP { get; set; }
        [Index("Sis_Idx_Nascimento")]
        public DateTime? Nascimento { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public int? IDInstrucao { get; set; }
        public int? IDProfissao { get; set; }
        public int? IDCobranca { get; set; }
        public int? IDContaTesouraria { get; set; }
        public int? IDMeioPagamento { get; set; }
        public int? IDDocumentoTipo { get; set; }
        public int? IDCentroCustoLocal { get; set; }
        public int? IDNaturalidade { get; set; }
        public string Nacionalidade { get; set; }
        public string Identidade { get; set; }
        public string OrgaoEmissor { get; set; }
        public DateTime? EmissaoIdentidade { get; set; }
        public string CPF { get; set; }
        public string RazaoSocial { get; set; }
        public string Nominal { get; set; }
        public string CGC { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string HomePage { get; set; }
        public string Juridico { get; set; }
        public string IsRecolheISS { get; set; }
        public string TotalQuitado { get; set; }
        public string SaldoAtual { get; set; }
        public string TotalPrevisto { get; set; }
        public int? NumeroLancamentos { get; set; }
        public DateTime? DataUltimoLancamento { get; set; }
        public DateTime? DataInclusao { get; set; }
        public int? IDUsuarioInclusao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
        public int? IDUsuarioAlteracao { get; set; }
        public bool? IsMalaDireta { get; set; }
        public bool? IsSincronizado { get; set; }
        public bool? IsAlterado { get; set; }
        public int? IDImportado { get; set; }
        public bool? Desativado { get; set; }
        public bool? System { get; set; }
        public bool? Hidden { get; set; }
        public int? IDBanco1 { get; set; }
        public string Agencia1 { get; set; }
        public string ContaCorrente1 { get; set; }
        public int? IDBanco2 { get; set; }
        public string Agencia2 { get; set; }
        public string ContaCorrente2 { get; set; }
        public int? IDFilialSin { get; set; }
        public int? IDTipoLogradouro { get; set; }
        public int? ContaPadrao { get; set; }
        public DateTime? DataExclusao { get; set; }
        public int? IDUsuarioExclusao { get; set; }
        public string Numero { get; set; }
        public int? IDCNAE { get; set; }
        public string Titular1 { get; set; }
        public string Titular2 { get; set; }
        public bool? IsFuncionario { get; set; }
        public bool? IsAgendaTel { get; set; }
        public byte[] Foto { get; set; }
        public string ObsPessoa { get; set; }
        public int? IDExterno { get; set; }
        public int? IDNFDescricao { get; set; }
        //tenant id para identificar a origem do registro
        public int? TenantId { get; set; }


    }
}
