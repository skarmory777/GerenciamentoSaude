using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Exporting
{
    public interface IListarTiposParticipacoesExcelExporter
    {
        FileDto ExportToFile(List<TipoParticipacaoDto> list);
    }
}
