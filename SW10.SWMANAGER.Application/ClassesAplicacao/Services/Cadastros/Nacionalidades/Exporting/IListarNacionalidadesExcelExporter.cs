using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Exporting
{
    public interface IListarNacionalidadesExcelExporter
    {
        FileDto ExportToFile(List<NacionalidadeDto> NacionalidadesDtos);
    }
}
