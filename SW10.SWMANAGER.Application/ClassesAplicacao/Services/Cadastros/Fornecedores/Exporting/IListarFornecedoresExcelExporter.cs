using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Exporting
{
    public interface IListarFornecedoresExcelExporter
    {
        FileDto ExportToFile(List<FornecedorDto> fornecedoresDtos);
    }
}
