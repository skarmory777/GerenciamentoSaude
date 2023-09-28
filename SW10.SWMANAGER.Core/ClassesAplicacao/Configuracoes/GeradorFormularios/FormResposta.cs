using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios
{
    [Table("SisFormResposta")]
    public class FormResposta : CamposPadraoCRUD
    {
        [Index("Sis_Idx_DataResposta")]
        public DateTime DataResposta { get; set; }

        public long FormConfigId { get; set; }
        [MaxLength(500)]
        [Index("Sis_Idx_NomeClasse")]
        public string NomeClasse { get; set; }

        [MaxLength(500)]
        [Index("Sis_Idx_RegistroClasseId")]
        public string RegistroClasseId { get; set; }

        [ForeignKey("FormConfigId")]
        public FormConfig FormConfig { get; set; }

        [Index("Sis_Idx_IsPreenchido")]
        public bool IsPreenchido { get; set; }

        public List<FormData> ColRespostas { get; set; }

    }
}