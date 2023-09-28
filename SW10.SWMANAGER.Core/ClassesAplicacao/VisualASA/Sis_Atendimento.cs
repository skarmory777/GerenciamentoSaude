using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    [Table("Sis_Atendimento")]
    public class Sis_Atendimento : CamposPadraoCRUD
    {
        public int? IDAtendimento { get; set; }
        public int? IDSW { get; set; }
        public int? IDEmpresa { get; set; }
        public int? IDFilial { get; set; }
        public int? IDConvenioAtend { get; set; }
        public int? IDOrigem { get; set; }
        public string CodAtendimento { get; set; }
        public int? IDPaciente { get; set; }
        public DateTime? DataAtendimento { get; set; }
        public int? IDMedico { get; set; }
        public int? IDEspecialidade { get; set; }
        public int? IDMedicoIndica { get; set; }
        public int? IDUsuarioInclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public int? IDUsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? IDUsuarioCancelamento { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int? IDAteMotCancelamento { get; set; }
        public bool? IsSincronizado { get; set; }
        public bool? IsAlterado { get; set; }
        public int? IDImportado { get; set; }
        public bool? Desativado { get; set; }
        public bool? System { get; set; }
        public bool? Hidden { get; set; }
        public int? IDFilialSin { get; set; }
        public string AgudaCronica { get; set; }
        public string PacienteCaixa { get; set; }
        public int? IDClinica { get; set; }
        public int? IDAtendimentoStatus { get; set; }
        public int? IDIndicadorAcidente { get; set; }
        public int? IDRevisaoEntrega { get; set; }
        public int? IDUsuarioRecebimento { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public int? IDUsuarioCancelaRecebimento { get; set; }
        public DateTime? DataCancelaRecebimento { get; set; }
        public string ObsRecebimento { get; set; }
        public int? IDUsuarioObsRecebimento { get; set; }
        public DateTime? DataObsRecebimento { get; set; }
        public bool? IsInternou { get; set; }
        public bool? IDUltUsuConfEmail { get; set; }
        public bool? IsSMSEnviado { get; set; }
        public bool? IsSMSConfirmado { get; set; }
        public int? IDMedicoConsulta { get; set; }
        public DateTime? DataMedicoConsulta { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
        public int? Idade { get; set; }
        public string JustificativaNumDeclNascVivo { get; set; }
        public int? IDAtendimentoInicial { get; set; }
        public DateTime? DataRetorno { get; set; }
        public byte[] ObsRetorno { get; set; }
        public int? IDUsuarioRetorno { get; set; }
        public int? IsEncaminhado { get; set; }
        public DateTime? DataConclusao { get; set; }
        public int? IDEspecialidadeMedIndica { get; set; }
        //tenant id para identificar a origem do registro
        public int? TenantId { get; set; }

    }
}
