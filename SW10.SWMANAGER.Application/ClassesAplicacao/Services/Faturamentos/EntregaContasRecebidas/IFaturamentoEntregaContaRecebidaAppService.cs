using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Inputs;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas
{
    public interface IFaturamentoEntregaContaRecebidaAppService : IApplicationService
    {
        Task ConciliarContasRecebidas(ConcilicarContasRecebidasInput input);

        Task<PagedResultDto<VisualizacaoContaRecebidaDto>> ListarContasRecebidasPorQuitacao(ContasRecebidasPorQuitacaoInput input);
    }
}
