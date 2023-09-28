using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoqueSolicitacaoItemAppService : IApplicationService
    {
        Task<EstoquePreMovimentoItemSolicitacaoDto> Obter(long id);
    }
}
