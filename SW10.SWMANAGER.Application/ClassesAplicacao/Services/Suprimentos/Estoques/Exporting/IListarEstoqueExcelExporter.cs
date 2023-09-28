using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Exporting
{
    public interface IListarEstoqueExcelExporter
    {
        FileDto ExportToFile(List<EstoqueDto> produtoEstoqueDtos);
    }
}