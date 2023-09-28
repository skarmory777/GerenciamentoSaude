using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Exporting
{
    public interface IListarVersoesTissExcelExporter
    {
        FileDto ExportToFile(List<VersaoTissDto> versoesTissDtos);
    }
}
