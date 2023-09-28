using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("VWTeste")]
    public class VWTeste : Entity<long>
    {
        public long PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public long CidadeId { get; set; }
        public string NomeCidade { get; set; }
        public long EstadoId { get; set; }
        public string NomeEstado { get; set; }
    }
}
