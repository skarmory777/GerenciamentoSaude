using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Exporting
{
    public interface IListarProfissoesExcelExporter
    {
        FileDto ExportToFile(List<ProfissaoDto> profissoesDtos);
    }
}
