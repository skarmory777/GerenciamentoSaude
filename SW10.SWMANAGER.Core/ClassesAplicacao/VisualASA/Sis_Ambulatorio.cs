using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    [Table("Sis_Ambulatorio")]
    public class Sis_Ambulatorio : CamposPadraoCRUD
    {
        public int? IDAmbulatorio { get; set; }
        public int? IDSW { get; set; }
        public string CodAmbulatorio { get; set; }
        [Index("Sis_Idx_DataInicio")]
        public DateTime? DataInicio { get; set; }
        public bool? IsRevisao { get; set; }
        public bool? IsHoraMarcada { get; set; }
        [Index("Sis_Idx_DataRetorno")]
        public DateTime? DataRetorno { get; set; }
        public int? IDAtendRevisao { get; set; }
        public int? IDAlta { get; set; }
        public int? StatusProntoAtend { get; set; }
        public string NumeroSeq { get; set; }
        public string TipoConsulta { get; set; }
        public bool? IsVacina { get; set; }
        [Index("Sis_Idx_DataExame")]
        public DateTime? DataExame { get; set; }
        public int? IDUsuarioLiberacao { get; set; }
        [Index("Sis_Idx_DataLiberacao")]
        public DateTime? DataLiberacao { get; set; }
        public int? IDPrioridadeAtendimento { get; set; }
        public bool? IsAltaRevelia { get; set; }
        public int? IDUsuarioRevelia { get; set; }
        [Index("Sis_Idx_DataSolicitacao")]
        public DateTime? DataSolicitacao { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamento { get; set; }
        public string DadosClinicos { get; set; }
        public string PrimConsulta { get; set; }
        public bool? IsAlergiaSzn { get; set; }
        public string QualAlergiaSzn { get; set; }
        [Index("Sis_Idx_DataPreAtend")]
        public DateTime? DataPreAtend { get; set; }
        public int? CodAmbulatorioSUS { get; set; }
        public int? IDSetor { get; set; }
        [Index("Sis_Idx_DataAltaAmbulatorial")]
        public DateTime? DataAltaAmbulatorial { get; set; }
        public int? IDAltaAmbulatorial { get; set; }
        public int? IDUsuarioAltaInc { get; set; }
        public int? IDMedPreAtend { get; set; }
        public bool? IsAtendendo { get; set; }
        public int? IDMedicoAtendendo { get; set; }
        public DateTime? DataAtendAmbulatorial { get; set; }
        public int? IDProtocoloEmergencia { get; set; }
        public DateTime? DataIniPreAtend { get; set; }
        public DateTime? DataIniInfoClinicas { get; set; }
        public DateTime? DataFimPreAtend { get; set; }
        public DateTime? DataFimInfoClinicas { get; set; }
        public DateTime? DataAltaMedica { get; set; }
        public DateTime? DataAltaAdministrativa { get; set; }
        //tenant id para identificar a origem do registro
        public int? TenantId { get; set; }

    }
}
