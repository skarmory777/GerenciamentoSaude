using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Exporting
{
    public interface IListarRegiaoExcelExporter
    {
        FileDto ExportToFile(List<RegiaoDto> tipoAtendimentoDtos);
    }
}
