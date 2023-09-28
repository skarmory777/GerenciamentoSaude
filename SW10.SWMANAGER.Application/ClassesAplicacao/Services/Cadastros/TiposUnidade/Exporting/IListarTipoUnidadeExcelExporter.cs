using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Exporting
{
    public interface IListarTipoUnidadeExcelExporter
    {
        FileDto ExportToFile(List<TipoUnidadeDto> tipoUnidadeDtos);
    }
}
