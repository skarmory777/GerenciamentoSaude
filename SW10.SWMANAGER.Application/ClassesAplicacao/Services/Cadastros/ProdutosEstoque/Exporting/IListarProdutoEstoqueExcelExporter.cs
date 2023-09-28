using System.Collections.Generic;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Exporting
{
    public interface IListarProdutoEstoqueExcelExporter
    {
        FileDto ExportToFile(List<ProdutoEstoqueDto> produtoEstoqueDtos);
    }
}