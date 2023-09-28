using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Exporting
{
    public interface IListarEntradasExcelExporter
    {
        FileDto ExportToFile(List<EntradaDto> list);
    }
}
