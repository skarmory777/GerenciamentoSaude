using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Exporting
{
    public interface IListarTiposVinculosEmpregaticiosExcelExporter
    {
        FileDto ExportToFile(List<TipoVinculoEmpregaticioDto> list);
    }
}
