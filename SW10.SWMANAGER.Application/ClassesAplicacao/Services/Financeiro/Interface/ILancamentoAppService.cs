using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface ILancamentoAppService : IApplicationService
    {
        List<LancamentoDto> ObterLancamentos(List<long> Ids);
    }
}
