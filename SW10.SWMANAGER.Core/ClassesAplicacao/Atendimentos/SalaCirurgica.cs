using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos
{
    [Table("AteSalaCirurgica")]
    public class SalaCirurgica : CamposPadraoCRUD
    {
        public string CorAgendamento { get; set; }
    }
}
