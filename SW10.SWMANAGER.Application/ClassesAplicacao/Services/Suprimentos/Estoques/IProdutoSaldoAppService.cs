using System.Threading.Tasks;
using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public interface IProdutoSaldoAppService: IApplicationService
    {
        Task<DefaultReturn<ValidaProdutoSaldoDto>> ValidaSaldoPorProdutoLoteValidadeEstoque(ValidaProdutoSaldoDto input);
    }
}