using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ExamesStatus
{
    public interface IExameStatusAppService : IApplicationService
    {
        List<ExameStatusDto> ObterTodos();
    }
}
