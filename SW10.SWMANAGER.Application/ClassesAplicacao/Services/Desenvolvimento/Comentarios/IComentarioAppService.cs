using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public interface IComentarioAppService : IApplicationService
    {
        Task CriarOuEditar(ComentarioDto input);

        Task Excluir(ComentarioDto input);

        Task<ComentarioDto> Obter(long id);

        Task<PagedResultDto<ComentarioDto>> ListarTodos();

        Task<PagedResultDto<ComentarioDto>> ListarPorTarefa(long tarefaId);

        Task<long> GetUsuarioLogadoAsync();
    }
}
