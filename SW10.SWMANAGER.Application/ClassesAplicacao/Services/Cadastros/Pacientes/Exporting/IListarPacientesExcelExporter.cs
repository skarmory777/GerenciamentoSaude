using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Exporting
{
    public interface IListarPacientesExcelExporter
    {
        FileDto ExportToFile(List<PacienteDto> pacientesDtos);
    }
}
