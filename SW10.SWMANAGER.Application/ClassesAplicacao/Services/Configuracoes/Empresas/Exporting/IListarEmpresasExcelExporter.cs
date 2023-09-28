using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Exporting
{
    public interface IListarEmpresasExcelExporter
    {
        FileDto ExportToFile(List<EmpresaDto> empresasDtos);
    }
}
