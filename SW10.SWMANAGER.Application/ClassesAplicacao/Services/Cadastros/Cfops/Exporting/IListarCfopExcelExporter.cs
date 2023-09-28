using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Exporting
{
    public interface IListarCfopExcelExporter
    {
        FileDto ExportToFile(List<CfopDto> unidadeDtos);
    }
}
