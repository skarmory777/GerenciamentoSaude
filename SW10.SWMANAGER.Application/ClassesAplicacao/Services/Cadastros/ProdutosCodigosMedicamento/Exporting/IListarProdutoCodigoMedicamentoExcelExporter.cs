using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Exporting
{
    public interface IListarProdutoCodigoMedicamentoExcelExporter
    {
        FileDto ExportToFile(List<ProdutoCodigoMedicamentoDto> produtoCodigoMedicamentoDtos);
    }
}
