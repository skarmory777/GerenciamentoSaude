using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public interface IControleProducaoAppService : IApplicationService
    {

        Task<PagedResultDto<ControleProducaoDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarControleProducao input);

        Task Excluir(CriarOuEditarControleProducao input);

        Task<CriarOuEditarControleProducao> Obter(long id);
    }
}
