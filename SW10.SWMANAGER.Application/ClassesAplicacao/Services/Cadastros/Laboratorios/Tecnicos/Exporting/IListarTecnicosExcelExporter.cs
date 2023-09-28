using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Exporting
{
    public interface IListarTecnicosExcelExporter
    {
        FileDto ExportToFile(List<TecnicoDto> TecnicosDtos);
    }
}
