using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Exporting
{
    public interface IListarNaturalidadesExcelExporter
    {
        FileDto ExportToFile(List<NaturalidadeDto> NaturalidadesDtos);
    }
}
