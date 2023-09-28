using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public interface IDocItemAppService : IApplicationService
    {

        Task<PagedResultDto<DocItemDto>> ListarTodos();

        Task CriarOuEditar(DocItemDto input);

        Task Excluir(DocItemDto input);

        Task<DocItemDto> Obter(long id);
    }
}
