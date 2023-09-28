using System.Threading.Tasks;
using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public class ProdutoSaldoAppService : SWMANAGERAppServiceBase, IProdutoSaldoAppService
    {
        public async Task<DefaultReturn<ValidaProdutoSaldoDto>> ValidaSaldoPorProdutoLoteValidadeEstoque(ValidaProdutoSaldoDto input)
        {
            using (var produtoSaldoDomainService =
                IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
            {
                return  await produtoSaldoDomainService.Object.ValidaSaldoPorProdutoLoteValidadeEstoque(input).ConfigureAwait(false);
            }
        }
    }
}