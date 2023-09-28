using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Exporting
{
    public interface IListarMotivosCaucaoExcelExporter
    {
        FileDto ExportToFile(List<MotivoCaucaoDto> MotivoCaucaoDtos);
    }
}
