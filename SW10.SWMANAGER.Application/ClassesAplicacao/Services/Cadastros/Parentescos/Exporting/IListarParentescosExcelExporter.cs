using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Exporting
{
    public interface IListarParentescosExcelExporter
    {
        FileDto ExportToFile(List<ParentescoDto> OrigensDtos);
    }
}
