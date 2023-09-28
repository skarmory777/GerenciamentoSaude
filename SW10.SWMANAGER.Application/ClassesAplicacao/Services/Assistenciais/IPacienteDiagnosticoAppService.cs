using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface IPacienteDiagnosticoAppService : IApplicationService
    {
        Task<List<PacienteDiagnosticosDto>> DiagnosticosPorPaciente(long pacienteId, long? atendimentoId);

        Task<PagedResultDto<PacienteDiagnosticosDto>> ListarIndexDiagnosticosPorPaciente(long pacienteId, long? atendimentoId);

        Task<PacienteDiagnosticosDto> ObterAsync(long id);

        Task UpsertPacienteDiagnostico(PacienteDiagnosticosDto model);

        Task Excluir(PacienteDiagnosticosDto modelDto);
    }
}
