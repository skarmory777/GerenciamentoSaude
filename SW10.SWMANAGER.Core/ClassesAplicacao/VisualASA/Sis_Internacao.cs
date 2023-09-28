using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    [Table("Sis_Internacao")]
    public class Sis_Internacao : CamposPadraoCRUD
    {
        public int? IDInternacao { get; set; }
        public int? IDSW { get; set; }
        public string CodInternacao { get; set; }
        public int? IDLeito { get; set; }
        public int? IDLeitoTipo { get; set; }
        public DateTime? DataAlta { get; set; }
        public int? IDAlta { get; set; }
        public bool? TemAcompanhante { get; set; }
        public string Responsavel { get; set; }
        public string EndResponsa { get; set; }
        public string CompResponsa { get; set; }
        public string CEPResponsa { get; set; }
        public int? IDBairroResponsa { get; set; }
        public int? IDCidadeResponsa { get; set; }
        public int? IDEstadoResponsa { get; set; }
        public string PaisResponsa { get; set; }
        public string IdtResponsa { get; set; }
        public string OrgEmisResponsa { get; set; }
        public DateTime? EmisIdtResponsa { get; set; }
        public string CPFResponsa { get; set; }
        public string CGCResponsa { get; set; }
        public int? IDEstadoPac { get; set; }
        public int? StatusPront { get; set; }
        public int? IDUsuarioPront { get; set; }
        public DateTime? DataPront { get; set; }
        public string NumObito { get; set; }
        public int? IDUsuarioAltaInc { get; set; }
        public DateTime? DataAltaInc { get; set; }
        public int? IDUsuarioAltaAlt { get; set; }
        public DateTime? DataAltaAlt { get; set; }
        public int? IDUsuarioAltaDel { get; set; }
        public DateTime? DataAltaDel { get; set; }
        public bool? IsEletiva { get; set; }
        public int? IDCIDObito { get; set; }
        public bool? IsGestacao { get; set; }
        public bool? IsAborto { get; set; }
        public bool? IsTransMat { get; set; }
        public bool? IsCompPuerperio { get; set; }
        public bool? IsAtendRNSalaParto { get; set; }
        public bool? IsCompNeoNatal { get; set; }
        public bool? IsBxPeso { get; set; }
        public bool? IsCesarea { get; set; }
        public bool? IsNormal { get; set; }
        public bool? IsInternacaoObstetrica { get; set; }
        public bool? IsObitoNeoNatal { get; set; }
        public int? SeObitoMulher { get; set; }
        public int? QtdeObitoNeonatalPrecoce { get; set; }
        public int? QtdeObitoNeonatalTardio { get; set; }
        public string NumDeclNascVivos1 { get; set; }
        public int? QtdeNascVivosTermo { get; set; }
        public int? QtdeNascMortos { get; set; }
        public int? QtdeNascVivosPrematuro { get; set; }
        public string NumDeclNascVivos2 { get; set; }
        public string NumDeclNascVivos3 { get; set; }
        public string NumDeclNascVivos4 { get; set; }
        public string NumDeclNascVivos5 { get; set; }
        public int? TvTelefone { get; set; }
        public int? QtdeAlta { get; set; }
        public int? QtdeTransf { get; set; }
        public string SisPreNatal { get; set; }
        public byte[] JustificativaSUS20 { get; set; }
        public byte[] JustificativaSUS21 { get; set; }
        public byte[] JustificativaSUS22 { get; set; }
        public int? IDAcompanhante { get; set; }
        public bool? IsAlergiaSzn { get; set; }
        public string QualAlergiaSzn { get; set; }
        public bool? TemCafe { get; set; }
        public bool? TemFralda { get; set; }
        public bool? TemRefeicao { get; set; }
        public bool? TemPernoite { get; set; }
        public bool? TemRefeicaoAcompanhante { get; set; }
        public DateTime? Cobertura { get; set; }
        public string DietaAtual { get; set; }
        public int? QuantFralda { get; set; }
        public DateTime? DataPrevisaoAlta { get; set; }
        public int? IDUsuarioPrevAltaInc { get; set; }
        public DateTime? DataPrevAltaInc { get; set; }
        public int? IDUsuarioPrevAltaAlt { get; set; }
        public DateTime? DataPrevAltaAlt { get; set; }
        public int? IDUsuarioPrevAltaDel { get; set; }
        public DateTime? DataPrevAltaDel { get; set; }
        //tenant id para identificar a origem do registro
        public int? TenantId { get; set; }
    }
}
