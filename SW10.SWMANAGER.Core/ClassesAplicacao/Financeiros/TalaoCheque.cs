using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinTalaoCheque")]
    public class TalaoCheque : CamposPadraoCRUD
    {
        public long ContaCorrenteId { get; set; }

        [ForeignKey("ContaCorrenteId")]
        public ContaCorrente ContaCorrente { get; set; }
        [Index("Fin_Idx_DataAbertura")]
        public DateTime DataAbertura { get; set; }
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }
    }
}
