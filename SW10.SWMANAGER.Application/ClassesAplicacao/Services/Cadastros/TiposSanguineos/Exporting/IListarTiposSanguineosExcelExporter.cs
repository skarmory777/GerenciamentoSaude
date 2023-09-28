using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Exporting
{
    public interface IListarTiposSanguineosExcelExporter
    {
        FileDto ExportToFile(List<TipoSanguineoDto> list);
    }
}
