using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov
{
    public interface IAtendimentoLeitoMovAppService : IApplicationService
    {
        // Task Editar(AtendimentoLeitoMovDto input);

        Task Criar(AtendimentoLeitoMovDto input);

        Task Excluir(long id);

        AtendimentoLeitoMovDto Obter(long? id, long? atendimentoId = null);

        AtendimentoLeitoMovDto Obter(long atendimentoId);

        Task<PagedResultDto<AtendimentoLeitoMovIndexDto>> ListarFiltro(ListarAtendimentosLeitosMovInput input);

        Task Editar(AtendimentoLeitoMovDto atendimentoLeitoMovDto);

        Task ObterEditarAtendimento(AtendimentoDto atendimentoDto);

        Task<DefaultReturn<AtendimentoLeitoMovDto>> TransferirLeito(long leitoOrigemId, long leitoDestinoId, long atendimentoId, DateTime dataHoraTransferencia, long? atendimentoDestinoId);

        Task<DefaultReturn<AtendimentoLeitoMovDto>> ExcluirMovimentoLeito(long id);

        Task<DefaultReturn<AtendimentoLeitoMovDto>> AltarDataMovimentoLeito(long id, DateTime novaData);
    }
}
