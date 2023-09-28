using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Exporting
{
    public interface IListarProdutoLaboratorioExcelExporter
    {
        FileDto ExportToFile(List<ProdutoLaboratorioDto> produtoLaboratorioDtos);
    }
}
