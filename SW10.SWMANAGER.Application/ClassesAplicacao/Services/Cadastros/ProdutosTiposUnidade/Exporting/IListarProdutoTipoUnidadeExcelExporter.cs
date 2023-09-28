using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Exporting
{
    public interface IListarProdutoTipoUnidadeExcelExporter
    {
        FileDto ExportToFile(List<ProdutoTipoUnidadeDto> produtoTipoUnidadeDtos);
    }
}
