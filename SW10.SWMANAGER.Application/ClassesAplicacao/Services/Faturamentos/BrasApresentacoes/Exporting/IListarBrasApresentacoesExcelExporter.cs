using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes.Exporting
{
    public interface IListarBrasApresentacoesExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoBrasApresentacaoDto> BrasApresentacoesDtos);
    }
}
