using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Exporting
{
    public interface IListarGrupoSubClasseExcelExporter
    {
        FileDto ExportToFile(List<GrupoSubClasseDto> GrupoSubclasseDtos);
    }
}