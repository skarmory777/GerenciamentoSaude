using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Exporting
{
    public interface IListarTipoAtendimentoExcelExporter
    {
        FileDto ExportToFile(List<TipoAtendimentoDto> tipoAtendimentoDtos);
    }
}
