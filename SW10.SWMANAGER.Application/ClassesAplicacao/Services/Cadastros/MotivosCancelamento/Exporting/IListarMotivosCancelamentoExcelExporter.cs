using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Exporting
{
    public interface IListarMotivosCancelamentoExcelExporter
    {
        FileDto ExportToFile(List<MotivoCancelamentoDto> MotivoCancelamentoDtos);
    }
}
