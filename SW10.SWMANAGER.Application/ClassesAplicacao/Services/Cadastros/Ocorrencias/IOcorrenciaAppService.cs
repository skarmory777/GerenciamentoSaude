using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias
{
    public interface IOcorrenciaAppService : IApplicationService
    {
        Task<OcorrenciaDto> CriarOuEditar(OcorrenciaDto input);

        Task<PagedResultDto<OcorrenciaListaDto>> Listar(OcorrenciaListaFiltroDto input);
    }
}