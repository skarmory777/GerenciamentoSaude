using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Exporting
{
    public interface IListarSetorsExcelExporter
    {
        FileDto ExportToFile(List<SetorDto> SetorsDtos);
    }
}
