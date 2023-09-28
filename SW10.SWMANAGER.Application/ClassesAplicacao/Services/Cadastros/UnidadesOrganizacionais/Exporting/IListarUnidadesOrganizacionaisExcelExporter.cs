using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Exporting
{
    public interface IListarUnidadesOrganizacionaisExcelExporter
    {
        FileDto ExportToFile(List<UnidadeOrganizacionalDto> unidadesDtos);
    }
}
