using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos
{
    public interface IStatusSolicitacaoProcedimentoAppService : IApplicationService
    {
        Task<ListResultDto<StatusSolicitacaoProcedimentoDto>> ListarTodos();
    }
}
