using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops
{
    [Table("Cfop")]
    public class Cfop : CamposPadraoCRUD
    {
        public long Numero { get; set; }
        public bool Tipo { get; set; }
        [Index("Idx_Vigencia")]
        public DateTime Vigencia { get; set; }
    }
}
