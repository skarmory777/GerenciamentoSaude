using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinBanco")]
    public class Banco : CamposPadraoCRUD
    {
        public List<Agencia> Agencias { get; set; }
    }
}
