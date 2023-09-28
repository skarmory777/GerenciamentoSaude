using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas
{
    public interface IFaturamentoContaStatusAppService : IApplicationService
    {
        List<FaturamentoContaStatusDto> ListarTodos();

    }
}
