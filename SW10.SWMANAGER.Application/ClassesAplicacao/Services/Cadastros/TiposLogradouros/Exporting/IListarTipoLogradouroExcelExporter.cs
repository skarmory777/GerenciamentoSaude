using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Exporting
{
    public interface IListarTipoLogradouroExcelExporter
    {
        FileDto ExportToFile(List<TipoLogradouroDto> tipoLogradouroDtos);
    }
}
