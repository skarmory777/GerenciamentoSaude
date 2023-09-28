using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;
using Abp.Domain.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Validacoes
{
    public interface IMovimentoValidacaoDomainService : IDomainService
    {
        List<ErroDto> ValidarFornecedoresBaixaVale(string baixasIds);
        List<ErroDto> ValidarFornecedoresMovimentosItemBaixaConsignados(string baixasItensIds);

        List<ErroDto> ValidarConfirmacaoSolicitacao(EstoquePreMovimentoDto preMovimento);
    }
}
