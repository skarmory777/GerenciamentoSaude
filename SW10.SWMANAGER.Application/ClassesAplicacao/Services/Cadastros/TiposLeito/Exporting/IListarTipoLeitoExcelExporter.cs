using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTiposGrupoCentroCustoLeitos.Exporting
{
    public interface IListarTipoLeitoExcelExporter
    {
        FileDto ExportToFile(List<TipoLeitoDto> tipoLeitosDtos);
    }
}
