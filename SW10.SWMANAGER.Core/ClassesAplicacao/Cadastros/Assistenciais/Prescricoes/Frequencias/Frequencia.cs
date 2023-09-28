using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias
{
    [Table("AssFrequencia")]
    public class Frequencia : CamposPadraoCRUD, IDescricao
    {
        public long Intervalo { get; set; }
        public string HoraInicialMedicacao { get; set; }
        public string Horarios { get; set; }
        public bool IsSos { get; set; }
    }
}
