using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Exporting
{
    public interface IListarProdutoSubstanciaExcelExporter
    {
        FileDto ExportToFile(List<ProdutoSubstanciaDto> produtoSubstanciaDtos);
    }
}