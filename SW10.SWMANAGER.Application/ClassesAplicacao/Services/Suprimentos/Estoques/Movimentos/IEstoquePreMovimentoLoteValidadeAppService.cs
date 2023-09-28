using Abp.Application.Services;
using NFe.Classes.Informacoes.Detalhe;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IEstoquePreMovimentoLoteValidadeAppService : IApplicationService
    {
        Task<decimal> ObterQuantidadeRestanteLoteValidade(long preMovimentoItemId);
        List<InformacaoLoteValidadeTodosDto> ObterLotesValidadesPreMovimento(long preMovimentoId, List<det> NFeItens);
        DefaultReturn<InformacaoLoteValidadeTodosDto> AtualizarLotesValidades(List<InformacaoLoteValidadeTodosDto> lotesValidades);
    }
}
