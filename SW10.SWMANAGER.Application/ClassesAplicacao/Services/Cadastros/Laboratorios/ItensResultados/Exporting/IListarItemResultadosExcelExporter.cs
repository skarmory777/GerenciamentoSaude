using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Exporting
{
    public interface IListarItemResultadosExcelExporter
    {
        FileDto ExportToFile(List<ItemResultadoDto> ItemResultadosDtos);
    }
}
