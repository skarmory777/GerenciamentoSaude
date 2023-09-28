using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEmprestimoAppService : IApplicationService
    {
        Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarSolicitacaoEmprestimo(EstoquePreMovimentoDto input);
        Task<EstoquePreMovimentoDto> ObterSolicitacaoParaBaixa(long id);
        Task<PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>> ObterItensDaSolicitacao(long preMovimentoId);
        Task<DefaultReturn<EstoquePreMovimentoDto>> AtenderBaixa(EstoquePreMovimentoDto preMovimento);
    }
}
