using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Exporting
{
    public interface IListarProdutoPortariaExcelExporter
    {
        FileDto ExportToFile(List<ProdutoPortariaDto> produtoPortariaDtos);
    }
}