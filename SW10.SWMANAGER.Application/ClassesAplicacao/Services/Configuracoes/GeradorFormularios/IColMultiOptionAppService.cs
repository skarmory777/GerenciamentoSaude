using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IColMultiOptionAppService : IApplicationService
    {
        Task<PagedResultDto<ColMultiOption>> Listar(ListarInput input);

        Task<ListResultDto<ColMultiOption>> ListarTodos();

        Task CriarOuEditar(ColMultiOption input);

        Task Excluir(ColMultiOption input);

        Task<ColMultiOption> Obter(long id);

    }
}
