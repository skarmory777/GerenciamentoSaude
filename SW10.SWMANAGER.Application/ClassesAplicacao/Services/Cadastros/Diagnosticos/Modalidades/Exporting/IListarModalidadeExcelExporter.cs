using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Exporting
{
    public interface IListarModalidadeExcelExporter
    {
        FileDto ExportToFile(List<ModalidadeDto> list);
    }
}
