using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Exporting
{
    public interface IListarTipoTabelaDominioExcelExporter
    {
        FileDto ExportToFile(List<TipoTabelaDominioDto> tipoTabelaDominioDtos);
    }
}