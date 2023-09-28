using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Exporting
{
    public interface IListarMaterialsExcelExporter
    {
        FileDto ExportToFile(List<MaterialDto> MaterialsDtos);
    }
}
