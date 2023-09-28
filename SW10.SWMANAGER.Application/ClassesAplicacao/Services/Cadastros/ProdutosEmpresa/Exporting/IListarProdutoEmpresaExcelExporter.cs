using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Exporting
{
    public interface IListarProdutoEmpresaExcelExporter
    {
        FileDto ExportToFile(List<ProdutoEmpresaDto> produtoEmpresaDtos);
    }
}
