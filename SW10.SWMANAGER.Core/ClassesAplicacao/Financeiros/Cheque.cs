using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinCheque")]
    public class Cheque : CamposPadraoCRUD
    {
        public long TalaoChequeId { get; set; }

        [ForeignKey("TalaoChequeId")]
        public TalaoCheque TalaoCheque { get; set; }

        public long Numero { get; set; }
        public string Nominal { get; set; }
        [Index("Fin_Idx_Data")]
        public DateTime? Data { get; set; }

    }
}
