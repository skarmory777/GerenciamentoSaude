using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssPrescricaoItemHora")]
    public class PrescricaoItemHora : CamposPadraoCRUD
    {
        [ForeignKey("PrescricaoItemResposta"), Column("AssPrescricaoItemRespostaId")]
        public long? PrescricaoItemRespostaId { get; set; }
        public PrescricaoItemResposta PrescricaoItemResposta { get; set; }
        public int DiaMedicamento { get; set; }

        [Index("Ass_Idx_DataMedicamento")]
        public DateTime DataMedicamento { get; set; }
        public string Hora { get; set; }
        [ForeignKey("HoraDia"), Column("SisHoraDiaId")]
        public long? HoraDiaId { get; set; }
        public HoraDia HoraDia { get; set; }
    }
}
