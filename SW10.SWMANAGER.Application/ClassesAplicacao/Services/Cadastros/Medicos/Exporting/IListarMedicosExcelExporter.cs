using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Exporting
{
    public interface IListarMedicosExcelExporter
    {
        FileDto ExportToFile(List<ListarMedicoIndex> intervalosDtos);
    }
}
