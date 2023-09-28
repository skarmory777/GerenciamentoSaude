using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Exporting
{
    public interface IListarKitsExcelExporter
    {
        FileDto ExportToFile(List<KitDto> KitsDtos);
    }
}
