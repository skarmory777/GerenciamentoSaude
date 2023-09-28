using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
    [AutoMap(typeof(PacientePeso))]
    public class PacientePesoDto : PesoDto
    {
        public long PacienteId { get; set; }

        //[ForeignKey("PacienteId")]
        //public virtual PacienteDto Paciente { get; set; }

    }
}
