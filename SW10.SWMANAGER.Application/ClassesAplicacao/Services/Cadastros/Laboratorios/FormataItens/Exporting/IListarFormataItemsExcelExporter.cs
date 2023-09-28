using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Exporting
{
    public interface IListarFormataItemsExcelExporter
    {
        FileDto ExportToFile(List<FormataItemDto> FormataItemsDtos);
    }
}
