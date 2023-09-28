using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Medicos
{
    public class MedicoEspecialidadesViewModel
    {
        public ICollection<MedicoEspecialidadeDto> MedicoEspecialidades { get; set; }

        public ICollection<EspecialidadeDto> Especialidades { get; set; }
    }
}