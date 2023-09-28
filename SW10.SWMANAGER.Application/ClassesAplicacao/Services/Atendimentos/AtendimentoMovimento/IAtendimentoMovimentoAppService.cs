using Abp.Application.Services;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentoMovimento
{
    public interface IAtendimentoMovimentoAppService : IApplicationService
    {
        Task<AtendimentoMovimentoDto> Obter(long atendimentoId);

        Task<AtendimentoMovimentoDto> AssumirAtendimento(long atendimentoId);

        Task<AtendimentoMovimentoDto> FinalizarAtendimento(long atendimentoId);
    }
}
