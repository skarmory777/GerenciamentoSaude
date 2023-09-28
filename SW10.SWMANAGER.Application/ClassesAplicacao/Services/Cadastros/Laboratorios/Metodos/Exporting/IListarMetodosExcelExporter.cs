using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Exporting
{
    public interface IListarMetodosExcelExporter
    {
        FileDto ExportToFile(List<MetodoDto> MetodosDtos);
    }
}
