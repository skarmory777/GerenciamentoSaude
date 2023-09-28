using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Exporting
{
    public interface IListarConsultorTabelaCamposExcelExporter
    {
        FileDto ExportToFile(List<ConsultorTabelaCampoDto> profissoesDtos);
    }
}
