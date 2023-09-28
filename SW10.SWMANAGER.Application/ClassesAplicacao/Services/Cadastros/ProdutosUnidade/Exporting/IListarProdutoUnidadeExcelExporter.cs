using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Exporting
{
    public interface IListarProdutoUnidadeExcelExporter
    {
        FileDto ExportToFile(List<ProdutoUnidadeDto> produtoUnidadeDtos);
    }
}
