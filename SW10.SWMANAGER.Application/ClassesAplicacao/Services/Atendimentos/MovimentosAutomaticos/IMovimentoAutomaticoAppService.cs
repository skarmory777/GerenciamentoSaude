using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos
{
    public interface IMovimentoAutomaticoAppService : IApplicationService
    {
        Task<MovimentoAutomaticoDto> Obter(long id);
        Task<PagedResultDto<MovimentoAutomaticoIndex>> Listar(ListarMovimentoAutomaticoInput input);
        DefaultReturn<MovimentoAutomaticoDto> CriarOuEditar(MovimentoAutomaticoDto input);
        Task Excluir(long id);
        Task<MovimentoAutomatico> ObterMovimentoAutomaticoParaAtendimento(long atendimentoId);
    }
}
