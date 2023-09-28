using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Exporting
{
    public interface IListarGrausInstrucoesExcelExporter
    {
        FileDto ExportToFile(List<GrauInstrucaoDto> GrausInstrucoesDtos);
    }
}
