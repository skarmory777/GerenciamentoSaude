using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Exporting
{
    public interface IListarProdutoGrupoTratamentoExcelExporter
    {
        FileDto ExportToFile(List<ProdutoGrupoTratamentoDto> produtoGrupoTratamentoDtos);
    }
}
