using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias
{
    public interface ITipoOcorrenciaAppService : IApplicationService
    {
        Task CriarOuEditar(TipoOcorrenciaDto input);

        Task<PagedResultDto<TipoOcorrenciaListaDto>> Listar(ListarInput input);
    }
}