using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface IPacienteAlergiasAppService : IApplicationService
    {
        Task<List<PacienteAlergiasDto>> AlergiasPorPaciente(long pacienteId, long? atendimentoId);

        Task<PagedResultDto<PacienteAlergiasDto>> ListarIndexAlergiasPorPaciente(long pacienteId, long? atendimentoId);

        Task<PacienteAlergiasDto> ObterAsync(long id);

        Task UpsertPacienteAlergia(PacienteAlergiasDto model);

        Task Excluir(PacienteAlergiasDto modelDto);
    }
}
