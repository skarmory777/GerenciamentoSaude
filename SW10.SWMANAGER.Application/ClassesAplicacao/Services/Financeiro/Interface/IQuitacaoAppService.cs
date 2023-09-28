using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface IQuitacaoAppService : IApplicationService
    {
        Task<QuitacaoDto> ObterPorLancamento(List<long> ids);
        DefaultReturn<QuitacaoDto> CriarOuEditar(QuitacaoDto input);
        Task<ListResultDto<QuitacaoIndex>> ListarQuitacoesPorLancamento(ListarQuitacaoLancamentoInput input);
        Task<ListResultDto<ListarQuitacao>> ListarQuitacoes(ListarQuitacoesInput input);
        Task<PagedResultDto<ListarQuitacao>> ListarQuitacoesNaoConsolidadas(ListarQuitacoesNaoConsolidadasInput input);
        Task<DefaultReturn<QuitacaoDto>> Excluir(long id);
    }
}
