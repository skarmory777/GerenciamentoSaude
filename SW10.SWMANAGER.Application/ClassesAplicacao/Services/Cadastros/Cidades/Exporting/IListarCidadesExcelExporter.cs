using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Exporting
{
    public interface IListarCidadesExcelExporter
    {
        FileDto ExportToFile(List<CidadeDto> CidadesDtos);
    }
}
