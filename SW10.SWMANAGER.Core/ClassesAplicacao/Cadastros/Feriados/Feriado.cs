using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Feriados
{
    [Table("Feriado")]
    public class Feriado : CamposPadraoCRUD
    {
        [Index("Idx_DiaMesAno")]
        public DateTime? DiaMesAno { get; set; }

    }
}
