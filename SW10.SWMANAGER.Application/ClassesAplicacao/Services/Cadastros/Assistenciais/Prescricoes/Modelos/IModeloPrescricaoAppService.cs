using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Modelos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Modelos
{
    public interface IModeloPrescricaoAppService : IApplicationService
    {
        Task<PagedResultDto<ModelePrescricaoIndex>> Listar(ModeloPrescricaoListarInput input);
    }
}
