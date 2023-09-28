using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Exporting
{
    public interface IListarProdutoPalavraChaveExcelExporter
    {
        FileDto ExportToFile(List<ProdutoPalavraChaveDto> produtoPalavraChaveDtos);
    }
}