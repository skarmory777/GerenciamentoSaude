using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Exporting
{
    public interface IListarIndicacoesExcelExporter
    {
        FileDto ExportToFile(List<IndicacaoDto> list);
    }
}
