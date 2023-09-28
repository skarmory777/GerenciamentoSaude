using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Exporting
{
    public interface IListarTipoAcomodacaoExcelExporter
    {
        FileDto ExportToFile(List<TipoAcomodacaoDto> tipoAcomodacaoDtos);
    }
}
