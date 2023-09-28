using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Exporting
{
    public interface IListarPlanosExcelExporter
    {
        FileDto ExportToFile(List<PlanoDto> profissoesDtos);
    }
}
