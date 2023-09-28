using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Exporting
{
    public interface IListarFormatasExcelExporter
    {
        FileDto ExportToFile(List<FormataDto> FormatasDtos);
    }
}
