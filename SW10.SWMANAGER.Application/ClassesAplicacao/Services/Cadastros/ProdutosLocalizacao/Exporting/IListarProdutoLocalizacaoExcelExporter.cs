using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Exporting
{
    public interface IListarProdutoLocalizacaoExcelExporter
    {
        FileDto ExportToFile(List<ProdutoLocalizacaoDto> ProdutoLocalizacaoDtos);
    }
}
