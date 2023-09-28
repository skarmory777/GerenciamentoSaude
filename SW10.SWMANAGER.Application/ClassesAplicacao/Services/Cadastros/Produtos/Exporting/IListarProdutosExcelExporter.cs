using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Exporting
{
    public interface IListarProdutosExcelExporter
    {
        FileDto ExportToFile(List<ProdutoDto> list);
    }
}
