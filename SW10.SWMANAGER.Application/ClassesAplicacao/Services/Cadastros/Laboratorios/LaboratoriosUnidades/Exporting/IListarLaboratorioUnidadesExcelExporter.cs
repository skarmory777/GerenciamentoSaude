using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Exporting
{
    public interface IListarLaboratorioUnidadesExcelExporter
    {
        FileDto ExportToFile(List<LaboratorioUnidadeDto> LaboratorioUnidadesDtos);
    }
}
