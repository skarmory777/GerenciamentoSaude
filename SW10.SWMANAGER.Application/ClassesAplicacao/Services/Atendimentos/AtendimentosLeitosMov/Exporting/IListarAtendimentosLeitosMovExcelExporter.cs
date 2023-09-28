using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Exporting
{
    public interface IListarAtendimentosLeitosMovExcelExporter
    {
        FileDto ExportToFile(List<AtendimentoLeitoMovDto> AtendimentoLeitoMovDto);
    }
}
