using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Exporting
{
    public interface IListarLaudoGruposExcelExporter
    {
        FileDto ExportToFile(List<LaudoGrupoDto> list);
    }
}
