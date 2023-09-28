using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Exporting
{
    public interface IListarGruposCIDExcelExporter
    {
        FileDto ExportToFile(List<GrupoCIDDto> list);
    }
}
