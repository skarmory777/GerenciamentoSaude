using SW10.SWMANAGER.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Enfermagens
{
    [Table("AssSinalVital")]
    public class SinalVital : CamposPadraoCRUD
    {
        [Index("Ass_Idx_DataRegistro")]
        public DateTime DataRegistro { get; set; }

        [ForeignKey("User"), Column("SisUserId")]
        public long UserId { get; set; }


        [ForeignKey("UserId")]
        public User Prestador { get; set; }

        public decimal Peso { get; set; }

        public decimal Altura { get; set; }

        public decimal PerimetroCefalico { get; set; }

        public virtual decimal Imc { get { return Peso / (Altura * Altura); } }

        public decimal Temperatura { get; set; }

        public string PressaoArterialSistolica { get; set; }

        public string FrequenciaCardiaca { get; set; }

        public string FrequenciaRespiratoria { get; set; }

        public string SaturacaoO2 { get; set; }

        public bool IsInsulina { get; set; }

        public bool IsJejum { get; set; }

        public bool IsAcordado { get; set; }

        public bool IsSonolento { get; set; }

        public bool IsSedado { get; set; }

        public bool IsComatoso { get; set; }

        public bool IsResponsivo { get; set; }

        public bool IsDeambulada { get; set; }

        public bool IsDeambuladaAuxilio { get; set; }

        public bool IsRestritoLeito { get; set; }

        public ICollection<SinalVitalFormConfig> FormulariosComplementares { get; set; }

    }
}
