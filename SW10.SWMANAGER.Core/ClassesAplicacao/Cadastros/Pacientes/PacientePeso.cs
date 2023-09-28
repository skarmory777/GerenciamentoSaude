using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes
{
    [Table("PacientePeso")]
    public class PacientePeso : Peso
    {
        public long PacienteId { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

    }
}
