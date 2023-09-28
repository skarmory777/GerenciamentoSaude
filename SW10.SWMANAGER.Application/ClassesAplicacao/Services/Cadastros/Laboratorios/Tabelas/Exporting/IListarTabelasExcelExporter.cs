using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Exporting
{
    public interface IListarTabelasExcelExporter
    {
        FileDto ExportToFile(List<TabelaDto> TabelasDtos);
    }
}
