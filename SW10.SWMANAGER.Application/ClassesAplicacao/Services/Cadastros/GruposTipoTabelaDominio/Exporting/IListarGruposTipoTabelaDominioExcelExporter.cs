using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposTipoTabelaDominio.Exporting
{
    public interface IListarGrupoTipoTabelaDominioExcelExporter
    {
        FileDto ExportToFile(List<GrupoTipoTabelaDominioDto> tipoTabelaDominioDtos);
    }
}