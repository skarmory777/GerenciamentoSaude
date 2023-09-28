using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Exporting
{
    public interface IListarMotivosAltaExcelExporter
    {
        FileDto ExportToFile(List<MotivoAltaDto> list);
    }
}
