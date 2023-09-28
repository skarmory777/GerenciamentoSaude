using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Movimentos.Exporting
{
    public interface IListarPreMovimentosExcelExporter
    {
        FileDto ExportToFile(List<MovimentoIndexDto> pacientesDtos);
    }
}
