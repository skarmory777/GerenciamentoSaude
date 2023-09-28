using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
    public class PacientePesosViewModel
    {
        public long PacienteId { get; set; }

        public ICollection<PacientePesoDto> PacientePesos { get; set; }
    }
}