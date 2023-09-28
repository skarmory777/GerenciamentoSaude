using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Exporting
{
    public interface IExameListExcelExporter
    {
        FileDto ExportToFile(List<ExameListDto> exameListDtos);
    }
}