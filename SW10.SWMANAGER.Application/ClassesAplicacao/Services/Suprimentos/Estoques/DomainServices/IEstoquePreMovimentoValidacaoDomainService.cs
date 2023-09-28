using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoquePreMovimentoValidacaoDomainService : IDomainService
    {
        Task<List<ErroDto>> Validar(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens, bool isValidaProdutoEmInventario = false);

        List<ErroDto> ValidarItem(EstoquePreMovimentoItemDto preMovimentoItem);

        Task ValidarItemSaida(EstoquePreMovimentoItemDto preMovimentoItem, bool validarLoteValidade, bool validarDataVencimeneto, List<ErroDto> lista);


        bool ExisteLoteValidadePendente(EstoquePreMovimentoDto preMovimento);

        Task<List<ErroDto>> ValidarSaidaEstoque(EstoquePreMovimento preMovimentoSaida);

        List<ErroDto> ValidarBaixaVale(EstoqueMovimento movimentoBaixa, List<long> movimentoIds);

        List<ErroDto> ValidarFornecedoresBaixaVale(string baixasIds);

        List<ErroDto> ValidarItemDevolucao(EstoquePreMovimentoItemDto preMovimentoItem, bool validarLoteValidade,
            bool validarDataVencimeneto);
        
    }
}