using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Exporting
{
    public interface IListarTipoEntradaExcelExporter
    {
        FileDto ExportToFile(List<TipoEntradaDto> unidadeDtos);
    }
}
