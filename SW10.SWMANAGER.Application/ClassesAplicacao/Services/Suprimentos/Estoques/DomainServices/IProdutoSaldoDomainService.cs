using System.Threading.Tasks;
using Abp.Domain.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices
{
    public interface IProdutoSaldoDomainService: IDomainService
    {
        void AtualizarSaldoMovimento(long movimentoId);
        Task AtualizarSaldoPreMovimentoItem(EstoquePreMovimentoItem preMovimentoItem);
        void AtualizarSaldoPreMovimentoItemLoteValidade(EstoquePreMovimentoLoteValidadeDto preMovimentoItemLoteValidade);

        void AtualizarSaldoPreMovimentoItemPreMovimento(EstoquePreMovimento preMovimento,
            EstoquePreMovimentoItem preMovimentoItem);
        void AtualizarSaldoPreMovimentoItemLoteValidadePreMovimento(EstoquePreMovimentoLoteValidade preMovimentoItemLoteValidade);
        Task<DefaultReturn<ValidaProdutoSaldoDto>> ValidaSaldoPorProdutoLoteValidadeEstoque(ValidaProdutoSaldoDto input);
    }
}