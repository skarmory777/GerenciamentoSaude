using Abp.Application.Services;
using SW10.SWMANAGER.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Validacoes
{
    public interface IPreMovimentoValidacaoService : IApplicationService
    {
        List<ErroDto> ValidarFornecedoresBaixaVale(string baixasIds);
    }
}
