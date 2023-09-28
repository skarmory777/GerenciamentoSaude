using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Exporting
{
    public interface IListarKitExamesExcelExporter
    {
        FileDto ExportToFile(List<KitExameDto> KitExamesDtos);
    }
}
