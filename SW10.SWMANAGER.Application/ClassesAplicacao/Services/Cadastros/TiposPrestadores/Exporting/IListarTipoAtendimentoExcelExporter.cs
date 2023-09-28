using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Exporting
{
    public interface IListarTipoPrestadorExcelExporter
    {
        FileDto ExportToFile(List<TipoPrestadorDto> tipoAtendimentoDtos);
    }
}
