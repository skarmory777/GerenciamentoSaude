using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    [Table("Sis_ContaMedica")]
    public class Sis_ContaMedica : CamposPadraoCRUD
    {
        public int? IDContaMedica { get; set; }
        public int? IDSW { get; set; }
        public int? IDAtendimento { get; set; }
        public int? IDConvenio { get; set; }
        public int? IDPlano { get; set; }
        public int? IDGuia { get; set; }
        public int? IDFormatoMatricula { get; set; }
        public string Matricula { get; set; }
        public string CodDependente { get; set; }
        public string NumeroGuia { get; set; }
        public string Titular { get; set; }
        public DateTime? DtPagamento { get; set; }
        public DateTime? ValCarteira { get; set; }
        public string IdentAcompanhante { get; set; }
        public int? StatusEntrega { get; set; }
        public string SenhaAutorizacao { get; set; }
        public int? DiasAutorizados { get; set; }
        public byte[] Observacao { get; set; }
        public int? IDPendenciaMotivo { get; set; }
        public DateTime? DataUltimaConferencia { get; set; }
        public int? IDUsuarioConferencia { get; set; }
        public bool? IsSincronizado { get; set; }
        public bool? IsAlterado { get; set; }
        public int? IDImportado { get; set; }
        public int? IDFilialSin { get; set; }
        public string NumeroSeq { get; set; }
        public int? IDMedico { get; set; }
        public string GuiaPrincipal { get; set; }
        public int? IDLeitoTipo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? IDAlta { get; set; }
        public bool? IsAutorizador { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public string TipoAtendimento { get; set; }
        public int? IDEmpresaPac { get; set; }
        public DateTime? DiaSerie1 { get; set; }
        public DateTime? DiaSerie2 { get; set; }
        public DateTime? DiaSerie3 { get; set; }
        public DateTime? DiaSerie4 { get; set; }
        public DateTime? DiaSerie5 { get; set; }
        public DateTime? DiaSerie6 { get; set; }
        public DateTime? DiaSerie7 { get; set; }
        public DateTime? DiaSerie8 { get; set; }
        public DateTime? DiaSerie9 { get; set; }
        public DateTime? DiaSerie10 { get; set; }
        public DateTime? DataEntrFolhaSala { get; set; }
        public DateTime? DataEntrDescCir { get; set; }
        public DateTime? DataEntrBolAnest { get; set; }
        public DateTime? DataEntrCDFilme { get; set; }
        public bool? IsSemAutorizacao { get; set; }
        public bool? IsAutorizado { get; set; }
        public string IndicacaoClinica { get; set; }
        public string TrilhaCartao { get; set; }
        public DateTime? DataValidadeSenha { get; set; }
        public string GuiaOperadora { get; set; }
        public int? IDUsuarioResponsavel { get; set; }
        public DateTime? DataUsuarioResponsavel { get; set; }
        public int? IDUsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? Ordem { get; set; }
        public bool? IsImprimeGuia { get; set; }
        public bool? IsComplementar { get; set; }
        //tenant id para identificar a origem do registro
        public int? TenantId { get; set; }
    }
}
