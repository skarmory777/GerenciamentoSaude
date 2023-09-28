using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Exporting
{
    public interface IListarTabelaDominioExcelExporter
    {
        FileDto ExportToFile(List<TabelaDominioDto> tabelaDominioDtos);
    }
}