using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Exporting
{
    public interface IListarProdutoAcaoTerapeuticaExcelExporter
    {
        FileDto ExportToFile(List<ProdutoAcaoTerapeuticaDto> produtoAcaoTerapeuticaDtos);
    }
}