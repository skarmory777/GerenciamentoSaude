using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Exporting
{
    public interface IListarUnidadeExcelExporter
    {
        FileDto ExportToFile(List<UnidadeDto> unidadeDtos);
    }
}
