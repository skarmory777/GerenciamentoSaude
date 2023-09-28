using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Exporting
{
    public interface IListarMotivosTransferenciaLeitoExcelExporter
    {
        FileDto ExportToFile(List<MotivoTransferenciaLeitoDto> MotivoTransferenciaLeitoDtos);
    }
}
