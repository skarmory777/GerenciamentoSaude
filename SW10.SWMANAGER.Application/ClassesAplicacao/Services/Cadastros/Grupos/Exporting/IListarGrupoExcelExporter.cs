using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Exporting
{
    public interface IListarGrupoExcelExporter
    {
        FileDto ExportToFile(List<GrupoDto> produtoEspecieDtos);
    }
}