using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia.Exporting
{
    public interface IListarInstituicaoTransferenciaExcelExporter
    {
        FileDto ExportToFile(List<InstituicaoTransferenciaDto> InstituicaoTransferenciaDtos);
    }
}
