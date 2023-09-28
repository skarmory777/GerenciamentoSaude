using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Exporting
{
    public interface IListarExamesExcelExporter
    {
        FileDto ExportToFile(List<ExameDto> ExamesDtos);
    }
}
