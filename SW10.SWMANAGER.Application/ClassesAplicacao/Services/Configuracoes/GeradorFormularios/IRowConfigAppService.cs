using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IRowConfigAppService : IApplicationService
    {
        Task<PagedResultDto<RowConfig>> Listar(ListarInput input);

        Task<ListResultDto<RowConfig>> ListarTodos();

        Task CriarOuEditar(RowConfig input);

        Task Excluir(RowConfig input);

        Task<RowConfig> Obter(long id);

    }
}
